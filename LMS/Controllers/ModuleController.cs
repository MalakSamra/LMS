using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LMS.Controllers
{
    public class ModuleController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();
        public IActionResult Index()
        {
            if (_context.modules == null)
            {
                return NotFound();
            }
            else
            {
                return View(_context.modules.Include(m=>m.Course).ToList());
            }
        }
        public IActionResult Details(int id)
        {
            if (_context.modules != null)
            {
                var module = _context.modules
                        .Include(m => m.Course)
                        .FirstOrDefault(m => m.Id == id);
                if (module == null)
                {
                    return NotFound();
                }
                return View(module);
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize(Roles = "admin")]
        //GET --> Just to direct to the create page 
        public IActionResult Create()
        {
            return View();
        }
        //POST: Module/Create
        //Binding --> For The Protection Of Overposting Attack 
        [HttpPost]
        public IActionResult Create([Bind("Id,Title,Description,Crs_Id")] Module module)
        {
            if (ModelState.IsValid)
            {
                _context.modules.Add(module);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(module);
        }
        [Authorize(Roles = "admin")]
        //GET: Module/Edit/1
        //Just to get the course with id and Direct to the Edit Page
        public IActionResult Edit(int? id)
        {
            if (_context.modules == null || id == null)
            {
                return NotFound();
            }
            var module = _context.modules.Find(id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }
        //POST: Module/Edit/1
        [HttpPost]
        public IActionResult Edit(int id, Module module)
        {
            if (id != module.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.modules.Update(module);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(module.Id))
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
            return View(module);
        }
        private bool ModuleExists(int id)
        {
            return _context.modules.Any(e => e.Id == id);
        }
        [Authorize(Roles = "admin")]
        // GET: Module/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.modules == null)
            {
                return NotFound();
            }

            var module = _context.modules
                .FirstOrDefault(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.modules == null)
            {
                return Problem("Entity set 'modules'  is null.");
            }
            var module = _context.modules.Find(id);
            if (module != null)
            {
                _context.modules.Remove(module);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
