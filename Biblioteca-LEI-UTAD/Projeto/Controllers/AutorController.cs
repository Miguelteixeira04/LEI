using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trabalho.Models;

namespace Trabalho.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class AutorController : Controller
    {
        private readonly Lab2Context _context;

        public AutorController(Lab2Context context)
        {
            _context = context;
        }

        // GET: Autor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Autors.ToListAsync());
        }

        // GET: Autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors
                .FirstOrDefaultAsync(m => m.IdAutor == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameAutor")] Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            // Gera um ID único para o autor
            int novoIdAutor;
            do
            {
                novoIdAutor = new Random().Next(10000, 99999); // Gera um ID de 5 dígitos
            } while (await _context.Autors.AnyAsync(a => a.IdAutor == novoIdAutor)); // Verifica unicidade no banco

            autor.IdAutor = novoIdAutor;

            try
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Livro");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar o autor: " + ex.Message);
                return View(autor);
            }
        }

        // GET: Autor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAutor,NameAutor")] Autor autor)
        {
            if (id != autor.IdAutor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.IdAutor))
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
            return View(autor);
        }

        // GET: Autor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors
                .FirstOrDefaultAsync(m => m.IdAutor == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor != null)
            {
                _context.Autors.Remove(autor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autors.Any(e => e.IdAutor == id);
        }
    }
}
