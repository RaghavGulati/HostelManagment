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
        public ActionResult AddCourse()
        {
            Courses model = new Courses();
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