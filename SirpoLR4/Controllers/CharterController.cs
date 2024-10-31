using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SirpoLR4.DataAccess;
using SirpoLR4.Models;
using System.Threading.Tasks;

namespace SirpoLR4.Controllers
{
    public class CharterController : Controller
    {
        private readonly CharterDbContext _context;

        public CharterController(CharterDbContext context)
        {
            _context = context;
        }

        // GET: CharterController
        public async Task<IActionResult> Index()
        {
            var charters = await _context.Charters.ToListAsync();
            return View(charters);
        }

        // GET: CharterController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var charter = await _context.Charters.FindAsync(id);
            if (charter == null)
                return NotFound();

            return View(charter);
        }

        // GET: CharterController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CharterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Charter charter)
        {
            if (ModelState.IsValid)
            {
                charter.Id = Guid.NewGuid();
                charter.CreatedAt = DateTime.UtcNow;
                charter.CharterDateTime = charter.CharterDateTime.ToUniversalTime(); // Преобразование в UTC
                _context.Charters.Add(charter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(charter);
        }

        // GET: CharterController/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var charter = await _context.Charters.FindAsync(id);
            if (charter == null)
                return NotFound();

            return View(charter);
        }

        // POST: CharterController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Charter charter)
        {
            if (id != charter.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    // Сохраняем изначальное значение CreatedAt
                    charter.CreatedAt = (await _context.Charters.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)).CreatedAt;

                    // Преобразуем CharterDateTime в UTC
                    charter.CharterDateTime = charter.CharterDateTime.ToUniversalTime();

                    _context.Update(charter);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Charters.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }
            }
            return View(charter);
        }


        // GET: CharterController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var charter = await _context.Charters.FindAsync(id);
            if (charter == null)
                return NotFound();

            return View(charter);
        }

        // POST: CharterController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var charter = await _context.Charters.FindAsync(id);
            if (charter != null)
            {
                _context.Charters.Remove(charter);
                await _context.SaveChangesAsync();

                // Устанавливаем сообщение об успешном удалении
                TempData["SuccessMessage"] = "Чартер успешно удален.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
