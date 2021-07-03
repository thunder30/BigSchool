using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BigSchool.Models;

namespace BigSchool.ViewModel
{
    public class CourseViewModels
    {
        [Required(ErrorMessage = "Địa điểm đang để trống.")]
        public string Place { get; set; }

        [Required(ErrorMessage = "Ngày đang để trống.")]
        [FutureDate(ErrorMessage = "Ngày có định dạng dd/M/yyyy và phải lớn hơn hôm nay.")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Giờ đang để trống.")]
        [ValidTime(ErrorMessage = "Giờ có định dạng HH:mm.")]
        public string Time { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn khoá học.")]
        public byte Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}