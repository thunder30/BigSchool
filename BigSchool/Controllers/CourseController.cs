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
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
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
            return RedirectToAction("Mine");
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

            //danh sach giang vien dang theo doi                        
            var listLecturer = _dbContext.Followings.Where(x => x.FollowerId == userId && x.FolloweeId != userId)
                .Include(l => l.Followee)
                .ToList();
            ViewBag.ListLecturer = listLecturer;

            return View(viewModel);
        }


        // danh sách khoá học được tạo bởi người dùng
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(c => c.LecturerId == userId && c.DateTime > DateTime.Now && !c.IsCanceled)
                .Include(l => l.Lecturer)
                .Include(c => c.Category)
                .ToList();
            return View(courses);
        }

        // danh sách giảng viên đang theo dõi
        public ActionResult LecturerFollowing()
        {
            var userId = User.Identity.GetUserId();

            var model = _dbContext.Followings.Where(x => x.FollowerId == userId && x.FolloweeId != userId)
                .Include(l => l.Followee)
                .ToList();

            return View(model);
        }
        
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == id && c.LecturerId == userId);

            var viewModel = new CourseViewModels
            {
                Categories = _dbContext.Categories.ToList(),
                Date = course.DateTime.ToString("dd/M/yyyy"),
                Time = course.DateTime.ToString("HH:mm"),
                Category = course.CategoryId,
                Place = course.Place,
                Heading = "Edit Course",
                Id = course.Id
            };

            return View("Create", viewModel);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModels model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _dbContext.Categories.ToList();
                return View("Create", model);
            }

            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == model.Id && c.LecturerId == userId);

            course.Place = model.Place;
            course.DateTime = model.GetDateTime();
            course.CategoryId = model.Category;

            _dbContext.SaveChanges();

            return RedirectToAction("Mine");

        }
    }
}