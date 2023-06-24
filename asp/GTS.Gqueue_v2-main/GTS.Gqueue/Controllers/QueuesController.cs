using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GTS.Gqueue.Entities;

namespace GTS.Gqueue.Controllers
{
    public class QueuesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public QueuesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Queues
        public async Task<IActionResult> Index()
        {
            return View(await _uow.QueueRepository.GetAllAsync());
        }

        // GET: Queues/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queue = await _uow.QueueRepository.GetByIdAsync(id.Value);
            if (queue == null)
            {
                return NotFound();
            }

            return View(queue);
        }

        // GET: Queues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Queues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Queue queue)
        {
            if (ModelState.IsValid)
            {
                queue.Id = Guid.NewGuid();
                await _uow.QueueRepository.AddAsync(queue);
                return RedirectToAction(nameof(Index));
            }

            return View(queue);
        }

        // GET: Queues/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queue = await _uow.QueueRepository.GetByIdAsync(id.Value);
            if (queue == null)
            {
                return NotFound();
            }

            return View(queue);
        }

        // POST: Queues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Queue queue)
        {
            if (id != queue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _uow.QueueRepository.UpdateAsync(queue);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await QueueExistsAsync(queue.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(queue);
        }

        // GET: Queues/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queue = await _uow.QueueRepository.GetByIdAsync(id.Value);
            if (queue == null)
            {
                return NotFound();
            }

            return View(queue);
        }

        // POST: Queues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var queue = await _uow.QueueRepository.GetByIdAsync(id);
            await _uow.QueueRepository.RemoveAsync(queue);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> QueueExistsAsync(Guid id)
        {
            return await _uow.QueueRepository.GetByIdAsync(id) != null;
        }
    }
}
