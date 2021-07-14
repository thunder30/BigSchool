using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using System.Data.Entity;
using BigSchool.ViewModel;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;
        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var upComingCourses = _dbContext.Courses
                .Include(c => c.Lecturer)
                .Include(c => c.Category)
                .Where(c => c.DateTime > DateTime.Now);            

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = upComingCourses,
                ShowAction = User.Identity.IsAuthenticated
            };

            //var userId = User.Identity.GetUserId();
            //if (User.Identity.IsAuthenticated)
            //{
            //    List<Course> list = upComingCourses.ToList();
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        var courseId = list.
            //        // kiểm tra đã theo đăng kí khoá học này chưa
            //        bool b = _dbContext.Attendances.Any(x => x.AttendeeId == userId && x.CourseId == )
            //    }
                
            //}

            //if (_dbContext.Attendances.Any())

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}