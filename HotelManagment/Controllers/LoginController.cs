using HotelManagment.Database_Model;
using HotelManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagment.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Checking the user credentials, Also Checks User is Admin or not.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            HostelManagmentEntities entity = new HostelManagmentEntities();
            AjaxModel model = new AjaxModel();
            var user = (from usr in entity.Users where usr.Email == email select usr).FirstOrDefault();
            if (user == null || !String.Equals(user.Password, password))
            {
                model.Success = false;
                model.Message = "InValid Credentials..";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            model.Success = true;
            model.Message = user.IsAdmin ? "Admin" : "User";
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignUp()
        {
            return View();
        }
    }
}