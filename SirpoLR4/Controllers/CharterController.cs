using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SirpoLR4.DataAccess;
using SirpoLR4.Models;

namespace SirpoLR4.Controllers
{
    public class CharterController : Controller
    {
        private readonly CharterDbContext _context;

        public CharterController(CharterDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, string searchField)
        {
            var charters = _context.Charters.AsQueryable();

            if (!string.IsNullOrEmpty(searchString) && searchField == "OrderNumber")
            {
                if (int.TryParse(searchString, out int orderNumber))
                {
                    charters = charters.Skip(orderNumber - 1).Take(1); // Получаем чартера по порядковому номеру
                }
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                switch (searchField)
                {
                    case "CititesPath":
                        charters = charters.Where(c => c.CititesPath.Contains(searchString));
                        break;
                    case "Price":
                        if (int.TryParse(searchString, out int price))
                        {
                            charters = charters.Where(c => c.Price == price);
                        }
                        break;
                }
            }

            return View(await charters.ToListAsync());
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
