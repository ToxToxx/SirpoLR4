using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SirpoLR4.DataAccess;
using SirpoLR4.Models;
using System.Diagnostics;

namespace SirpoLR4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CharterDbContext _context;

        public HomeController(ILogger<HomeController> logger, CharterDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var charters = await _context.Charters.AsNoTracking().ToListAsync();
            return View(charters);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
