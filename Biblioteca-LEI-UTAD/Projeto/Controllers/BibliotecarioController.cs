using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trabalho.Models;

namespace Trabalho.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class BibliotecarioController : Controller
    {
        private readonly Lab2Context _Context;

        public BibliotecarioController(Lab2Context lab2Context)
        {
            _Context = lab2Context;
        }
        public async Task<IActionResult> Informacoes()
        {
            var biblioteca = await _Context.Bibliotecas.ToListAsync();
            return View(biblioteca);
        }
    }
}