using LMS.Data;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();
        //Get All The Enrolled Courses 
        public IActionResult Index()
        {

            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var userProgresses = _context.progresses
                .Where(p => p.User_Id == userId)
                .Select(p => new EnrolledCoursesViewModel{
                    Crs_Id = p.Module.Crs_Id,
                    CourseTitle = p.Module.Course.Title,
                })
                .Distinct()
                .ToList();
            if (userProgresses == null) { return NotFound(); }
            return View(userProgresses);
        }
        //GET: Student/Details/1
        //This EndPoint is for getting the course details and the modules that is in the course 
        public IActionResult Details(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            if (_context.progresses != null)
            {
                var userCourseProgresses = _context.progresses
                .Where(p => p.Module.Crs_Id == id)
                .Select(p => new EnrolledCoursesViewModel
                {
                    Crs_Id = p.Module.Crs_Id,
                    CourseTitle = p.Module.Course.Title,
                    CourseDescription = p.Module.Course.Description,
                    Module_Id = p.Module_Id,
                    ModuleTitle = p.Module.Title,
                    Progresses = _context.progresses
                        .Where(pr => pr.User_Id == userId && pr.Module.Crs_Id == id)
                        .Include(pr => pr.Module)
                        .ToList()
                })
                .FirstOrDefault();
                if (userCourseProgresses == null)
                {
                    return NotFound();
                }
                return View(userCourseProgresses);
            }
            else
            {
                return NotFound();
            }
        }
        //GET: Course/Edit/1
        //Just to get the course with id and Direct to the Edit Page
        public IActionResult Edit(int? id)
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            if (_context.progresses == null || id == null)
            {
                return NotFound();
            }
            var progress = _context.progresses
                            .Include(pr=>pr.Module)
                            .FirstOrDefault(pr => pr.Module_Id == id && pr.User_Id == userId);
            if (progress == null)
            {
                return NotFound();
            }
            return View(progress);
        }
        //POST: Student/Edit/1
        //[HttpPost]
        //public IActionResult Edit(int id, [Bind("Module.Title,Completion_Status")] Progress progress)
        //{
        //    int userId = HttpContext.Session.GetInt32("UserId").Value;
        //    //if (id != progress.Module_Id && userId != progress.User_Id)
        //    //{
        //    //    return NotFound();
        //    //}
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.progresses.Update(progress);
        //            _context.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(progress);
        //}
        //private bool ProgressExists(int id)
        //{
        //    return _context.progresses.Any(e => e.Module_Id == id);
        //}
        [HttpPost]
        public IActionResult Edit(int id)
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var existingProgress = _context.progresses
                    .Include(pr => pr.Module)
                    .FirstOrDefault(pr => pr.Module_Id == id && pr.User_Id == userId);
            if (existingProgress == null)
            {
                return NotFound();
            }
            existingProgress.Completion_Status = "Completed";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.progresses.Update(existingProgress);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Details));
                }
                catch (DbUpdateConcurrencyException)
                {
                }
            }

            return View();
        }
    }
}
