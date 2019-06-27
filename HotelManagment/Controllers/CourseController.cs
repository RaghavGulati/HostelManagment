using HotelManagment.Database_Model;
using HotelManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagment.Controllers
{
    public class CourseController : Controller
    {
        HostelManagmentEntities entity = new  HostelManagmentEntities();
        // GET: Course
        public ActionResult AllCourses()
        {
            var courses = (from cors in entity.Courses select cors).ToList();
            return View(courses);
        }
        

        public ActionResult AddCourse(int? Id)
        {
            Courses model = new Courses();
            if (Id > 0)
            {
                model = (from cors in entity.Courses where cors.Id == Id select cors).FirstOrDefault();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveCourse(Courses model)
        {
            AjaxModel result = new AjaxModel();
            try
            {
                if (model == null)
                {
                    result.Success = false;
                    result.Message = "Internal Server Error. Please try again later.";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (model.Id == 0)
                {
                    entity.Courses.Add(model);
                }
                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                entity.SaveChanges();
                result.Success = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}