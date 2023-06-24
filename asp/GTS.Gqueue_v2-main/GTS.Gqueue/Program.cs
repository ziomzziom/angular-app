using GTS.Gqueue;
using GTS.Gqueue.Repositories;
using GTS.Gqueue.Repositories.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSqlite<ApplicationContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddTransient<ApplicationContext, ApplicationContext>();
builder.Services.AddTransient<IQueueRepository, QueueRepository>();
builder.Services.AddTransient<IAwaitingPersonRepository, AwaitingPersonRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbContext.Database.Migrate();
}

//Error handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        // Log the exception here
        await context.Response.WriteAsync("An unexpected error has occurred.");
    });
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Queues}/{action=Index}/{id?}");

app.Run();
