using HotelManagment.Database_Model;
using HotelManagment.Helpers;
using HotelManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagment.Controllers
{
    public class UserController : Controller
    {
        HostelManagmentEntities entity = new HostelManagmentEntities();
        Common helper = new Common();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdatePassword(string old,string newpass)
        {
            AjaxModel result = new AjaxModel();
            int userid = 0;
            try
            {
                var user = (User)Session["CurrentUser"];
                userid = user.Id;
                if (helper.Decode(user.Password) != old)
                {
                    helper.ManageLogs(user.Id, "old password mismatch while updating it with new password..");
                    result.Success = false;
                    result.Message = "Old Password is not correct";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                user.Password = helper.Encode(newpass);
                entity.SaveChanges();
                helper.ManageLogs(user.Id, "Password updated successfully.");
                result.Success = true;
                result.Message = "Password updated successfully.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                helper.ManageLogs(userid, ex.Message);
                result.Success = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}