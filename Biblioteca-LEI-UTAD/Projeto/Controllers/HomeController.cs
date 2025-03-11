using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Trabalho.Models;

namespace Trabalho.Controllers
{
    public class HomeController : Controller
    {
        private readonly Lab2Context _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(Lab2Context context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var biblioteca = await _context.Bibliotecas.ToListAsync();
            return View(biblioteca);
        }

        public async Task<IActionResult> Biblioteca(string titulo, string autor, string genero)
        {
            var query = _context.Livros
                .Include(l => l.IdAutorNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(titulo))
            {
                query = query.Where(l => EF.Functions.Like(l.Titulo, $"%{titulo}%"));
            }

            if (!string.IsNullOrEmpty(autor))
            {
                query = query.Where(l => EF.Functions.Like(l.IdAutorNavigation.NameAutor, $"%{autor}%"));
            }

            if (!string.IsNullOrEmpty(genero))
            {
                query = query.Where(l => EF.Functions.Like(l.Genero, $"%{genero}%"));
            }

            var livros = await query.ToListAsync();

            ViewData["Titulo"] = titulo;
            ViewData["Autor"] = autor;
            ViewData["Genero"] = genero;

            return View(livros);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
