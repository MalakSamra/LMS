using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LMS.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();
        //GET
        public IActionResult Index()
        {
            if(_context.courses == null)
            {
                return NotFound();
            }
            else
            {
                return View(_context.courses.ToList());
            }
        }
        //GET: Course/Details/1
        public IActionResult Details(int id)
        {
            if (_context.courses != null)
            {
                var crs = _context.courses
                        .Include(c=>c.Modules)
                        .FirstOrDefault(c => c.Id == id);
                if (crs == null)
                {
                    return NotFound();
                }
                return View(crs);
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
        //POST: Course/Create
        //Binding --> For The Protection Of Overposting Attack 
        [HttpPost]
        public IActionResult Create([Bind("Title,Description")]Course course)
        {
            if (ModelState.IsValid)
            {
                _context.courses.Add(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        [Authorize(Roles = "admin")]
        //GET: Course/Edit/1
        //Just to get the course with id and Direct to the Edit Page
        public IActionResult Edit(int? id)
        {
            if(_context.courses == null || id == null)
            {
                return NotFound();
            }
            var crs = _context.courses.Find(id);
            if (crs == null)
            {
                return NotFound();
            }
            return View(crs);
        }
        //POST: Course/Edit/1
        [HttpPost]
        public IActionResult Edit(int id, Course course)
        {
            if(id != course.Id) 
            { 
                return NotFound(); 
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.courses.Update(course);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }
        private bool CourseExists(int id)
        {
            return _context.courses.Any(e => e.Id == id);
        }
        [Authorize(Roles = "admin")]
        // GET: Course/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.courses == null)
            {
                return NotFound();
            }

            var crs =  _context.courses
                .FirstOrDefault(c => c.Id == id);
            if (crs == null)
            {
                return NotFound();
            }

            return View(crs);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.courses == null)
            {
                return Problem("Entity set 'courses'  is null.");
            }
            var crs = _context.courses.Find(id);
            if (crs != null)
            {
                _context.courses.Remove(crs);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
