using HotelManagment.Database_Model;
using HotelManagment.Models;
using HotelManagment.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagment.Controllers
{
    public class LoginController : Controller
    {
        HostelManagmentEntities entity = new HostelManagmentEntities();
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
            AjaxModel model = new AjaxModel();
            var user = FindUser(email);//(from usr in entity.Users where usr.Email == email select usr).FirstOrDefault();
            if (user == null || !String.Equals(Decode(user.Password), password))
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
            UserModel model = new UserModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUser(UserModel model)
        {
            //User user = new User();
            //user.FirstName = model.FirstName;
            //user.LastName = model.LastName;
            //user.Mobile = model.Mobile;
            //user.IsAdmin = false;
            //user.Password = Encode(model.Password.ToString());
            //user.Email = model.Email;
            //user.Address = model.Address;
            //entity.Users.Add(user);
            //entity.SaveChanges();   
            return Content("");
        }

        public ActionResult ForgotPassword()        
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult UpdatePassword(string email, string newPassword)
        {
            var user = FindUser(email);
            user.Password = Encode(newPassword);
            entity.SaveChanges();
            return View();
        }

        private User FindUser(string email)
        {
            var user = (from usr in entity.Users where usr.Email == email select usr).FirstOrDefault();
            return user;
        }
        private string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        private static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }
    }
}