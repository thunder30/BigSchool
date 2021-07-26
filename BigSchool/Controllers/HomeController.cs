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
                .Where(c => c.DateTime > DateTime.Now && !c.IsCanceled);            

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = upComingCourses,
                ShowAction = User.Identity.IsAuthenticated
            };

            //danh sach giang vien dang theo doi            
            var userId = User.Identity.GetUserId();
            var listLecturer = _dbContext.Followings.Where(x => x.FollowerId == userId && x.FolloweeId != userId)
                .Include(l => l.Followee)
                .ToList();
            ViewBag.ListLecturer = listLecturer;

            //danh sach khoa hoc dang tham gia
            var listCourse = _dbContext.Attendances.Where(x => x.AttendeeId == userId).ToList();
            ViewBag.ListCourse = listCourse;

            #region api Attending
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

            #endregion

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