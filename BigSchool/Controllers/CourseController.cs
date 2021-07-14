using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using BigSchool.ViewModel;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CourseController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Course
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModels
            {
                Categories = _dbContext.Categories.ToList()
            };
            return View(viewModel);
        }
        //POST: Course
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModels viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }


            var course = new Course()
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryId = viewModel.Category,
                Place = viewModel.Place
            };

            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // Danh sách khoá học đang tham gia
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category)
                .ToList();
            
            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
            return View(courses);
        }
    }
}