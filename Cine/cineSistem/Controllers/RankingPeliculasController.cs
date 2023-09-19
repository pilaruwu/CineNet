using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cineSistem.Models;
using Microsoft.AspNetCore.Authorization;

namespace cineSistem.Controllers
{
    [Authorize]
    public class RankingPeliculasController : Controller
    {
        private readonly CineSistemContext _context;

        public RankingPeliculasController(CineSistemContext context)
        {
            _context = context;
        }

        // GET: RankingPeliculas
        public async Task<IActionResult> Index()
        {
            var cineSistemContext = _context.RankingPeliculas.Include(r => r.IdPelNavigation);
            return View(await cineSistemContext.ToListAsync());
        }
        public IActionResult Grafica()
        {
            var peliculasCalificadas = _context.Peliculas
                .Select(p => new
                {
                    Nombre = p.Name,
                    CalificacionPromedio = p.RankingPeliculas.Average(rp => rp.Ranking)
                })
                .OrderByDescending(p => p.CalificacionPromedio)
                .Take(2) // Obtener las 5 mejores calificadas (ajusta según tus necesidades)
                .ToList();

            // Separar los nombres de las películas y las calificaciones promedio
            var nombresPeliculas = peliculasCalificadas.Select(p => p.Nombre).ToArray();
            var calificacionesPromedio = peliculasCalificadas.Select(p => Math.Round(p.CalificacionPromedio, 2)).ToArray();

            ViewBag.NombresPeliculas = nombresPeliculas;
            ViewBag.CalificacionesPromedio = calificacionesPromedio;

            return View();
        }
        // GET: RankingPeliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RankingPeliculas == null)
            {
                return NotFound();
            }

            var rankingPelicula = await _context.RankingPeliculas
                .Include(r => r.IdPelNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rankingPelicula == null)
            {
                return NotFound();
            }

            return View(rankingPelicula);
        }

        // GET: RankingPeliculas/Create
        public IActionResult Create()
        {
            ViewData["IdPel"] = new SelectList(_context.Peliculas, "IdPel", "IdPel");
            return View();
        }

        // POST: RankingPeliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPel,Ranking")] RankingPelicula rankingPelicula)
        {
            
                _context.Add(rankingPelicula);
                await _context.SaveChangesAsync();


            ViewData["IdPel"] = new SelectList(_context.Peliculas, "IdPel", "IdPel", rankingPelicula.IdPeli);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Calificar(int idPel, int rank, string nombre)
        {
            var nuevoRanking = new RankingPelicula
            {
                IdPeli = idPel,
                Ranking = rank
            };
            _context.Add(nuevoRanking);
            _context.SaveChangesAsync();
            TempData["MensajeExito"] = "El ranking de la pelicula "+nombre+" se ha guardado con exito.";
            return RedirectToAction("Index", "Peliculas");
        }
        // GET: RankingPeliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RankingPeliculas == null)
            {
                return NotFound();
            }

            var rankingPelicula = await _context.RankingPeliculas.FindAsync(id);
            if (rankingPelicula == null)
            {
                return NotFound();
            }
            ViewData["IdPel"] = new SelectList(_context.Peliculas, "IdPel", "IdPel", rankingPelicula.IdPeli);
            return View(rankingPelicula);
        }

        // POST: RankingPeliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPel,Ranking")] RankingPelicula rankingPelicula)
        {
            if (id != rankingPelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rankingPelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankingPeliculaExists(rankingPelicula.Id))
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
            ViewData["IdPel"] = new SelectList(_context.Peliculas, "IdPel", "IdPel", rankingPelicula.IdPeli);
            return View(rankingPelicula);
        }

        // GET: RankingPeliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RankingPeliculas == null)
            {
                return NotFound();
            }

            var rankingPelicula = await _context.RankingPeliculas
                .Include(r => r.IdPelNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rankingPelicula == null)
            {
                return NotFound();
            }

            return View(rankingPelicula);
        }

        // POST: RankingPeliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RankingPeliculas == null)
            {
                return Problem("Entity set 'CineSistemContext.RankingPeliculas'  is null.");
            }
            var rankingPelicula = await _context.RankingPeliculas.FindAsync(id);
            if (rankingPelicula != null)
            {
                _context.RankingPeliculas.Remove(rankingPelicula);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RankingPeliculaExists(int id)
        {
          return (_context.RankingPeliculas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
