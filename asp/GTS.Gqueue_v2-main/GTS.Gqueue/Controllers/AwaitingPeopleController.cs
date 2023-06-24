using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GTS.Gqueue.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GTS.Gqueue.Controllers
{
    public class AwaitingPeopleController : Controller
    {
        private readonly IUnitOfWork _uow;

        public AwaitingPeopleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AwaitingPeople
        [Route("Queues/{queueId}/[controller]")]
        public async Task<IActionResult> Index(Guid queueId)
        {
            ViewBag.Queue = await _uow.QueueRepository.GetByIdAsync(queueId);
            return View(await _uow.AwaitingPersonRepository.GetAllFromQueueAsync(queueId));
        }

        // GET: AwaitingPeople/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awaitingPerson = await _uow.AwaitingPersonRepository.GetByIdAsync(id.Value);
            if (awaitingPerson == null)
            {
                return NotFound();
            }

            return View(awaitingPerson);
        }

        // GET: AwaitingPeople/Create
        public async Task<IActionResult> Create(Guid queueId)
        {
            ViewBag.QueueId = queueId;
            ViewBag.QueueSelectItems = new SelectList((await _uow.QueueRepository.GetAllAsync()).AsEnumerable(), "Id", "Name", queueId);
            return View();
        }

        // POST: AwaitingPeople/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Order,QueueId,Dequeued")] AwaitingPerson awaitingPerson)
        {
            if (ModelState.IsValid)
            {
                awaitingPerson.Id = Guid.NewGuid();
                awaitingPerson.Order = await _uow.AwaitingPersonRepository.GetOrderAsync(awaitingPerson.QueueId);
                await _uow.AwaitingPersonRepository.AddAsync(awaitingPerson);
                return RedirectToAction(nameof(Index), new { QueueId = awaitingPerson.QueueId });
            }

            ViewBag.QueueId = awaitingPerson.QueueId;
            ViewBag.QueueSelectItems = new SelectList((await _uow.QueueRepository.GetAllAsync()).AsEnumerable(), "Id", "Name", awaitingPerson.QueueId);
            return View(awaitingPerson);
        }

        // GET: AwaitingPeople/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awaitingPerson = await _uow.AwaitingPersonRepository.GetByIdAsync(id.Value);
            if (awaitingPerson == null)
            {
                return NotFound();
            }

            ViewBag.QueueSelectItems = new SelectList((await _uow.QueueRepository.GetAllAsync()).AsEnumerable(), "Id", "Name", awaitingPerson.QueueId);
            return View(awaitingPerson);
        }

        // POST: AwaitingPeople/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Order,QueueId,Dequeued")] AwaitingPerson awaitingPerson)
        {
            if (id != awaitingPerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _uow.AwaitingPersonRepository.UpdateAsync(awaitingPerson);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await AwaitingPersonExistsAsync(awaitingPerson.Id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                ViewBag.QueueSelectItems = new SelectList((await _uow.QueueRepository.GetAllAsync()).AsEnumerable(), "Id", "Name", awaitingPerson.QueueId);
                return RedirectToAction(nameof(Index), new { QueueId = awaitingPerson.QueueId });
            }

            return View(awaitingPerson);
        }

        // GET: AwaitingPeople/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var awaitingPerson = await _uow.AwaitingPersonRepository.GetByIdAsync(id.Value);
            if (awaitingPerson == null)
            {
                return NotFound();
            }

            return View(awaitingPerson);
        }

        // POST: AwaitingPeople/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var awaitingPerson = await _uow.AwaitingPersonRepository.GetByIdAsync(id);
            await _uow.AwaitingPersonRepository.RemoveAsync(awaitingPerson);
            return RedirectToAction(nameof(Index), new { QueueId = awaitingPerson.QueueId });
        }

        [HttpPost("Queues/{queueId}/[controller]/[action]/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Dequeue(Guid id)
        {
            var awaitingPerson = await _uow.AwaitingPersonRepository.GetByIdAsync(id);
            awaitingPerson.Dequeued = true;
            await _uow.AwaitingPersonRepository.UpdateAsync(awaitingPerson);
            return PartialView("_IndexPartial", await _uow.AwaitingPersonRepository.GetAllFromQueueAsync(awaitingPerson.QueueId));
        }

        [HttpPost("Queues/{queueId}/[controller]/[action]/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Requeue(Guid id)
        {
            var awaitingPerson = await _uow.AwaitingPersonRepository.GetByIdAsync(id);
            awaitingPerson.Dequeued = false;
            await _uow.AwaitingPersonRepository.UpdateAsync(awaitingPerson);
            return PartialView("_IndexPartial", await _uow.AwaitingPersonRepository.GetAllFromQueueAsync(awaitingPerson.QueueId));
        }

        private async Task<bool> AwaitingPersonExistsAsync(Guid id)
        {
            return await _uow.AwaitingPersonRepository.GetByIdAsync(id) != null;
        }
    }
}
