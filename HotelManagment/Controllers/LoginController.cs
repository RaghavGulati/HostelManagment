using HotelManagment.Database_Model;
using HotelManagment.Helpers;
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
        Common helper = new Common();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string email, string password)
        {
            AjaxModel model = new AjaxModel();
            int userid = 0;
            try
            {
                var user = FindUser(email);//(from usr in entity.Users where usr.Email == email select usr).FirstOrDefault();
                if (user == null || !String.Equals(helper.Decode(user.Password), password))
                {
                    model.Success = false;
                    model.Message = "InValid Credentials..";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (user.IsActive == false)
                {
                    model.Success = false;
                    model.Message = "Your account is not yet approved by administrator. Please try later.";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                userid = user.Id;
                helper.ManageLogs(user.Id, "User Login to the account");

                UserModel usermodel = new UserModel();
                usermodel.UserId = user.Id;
                usermodel.IsAdmin = user.IsAdmin;
                usermodel.IsActive = user.IsActive;
                usermodel.Email = user.Email;
                usermodel.FirstName = user.FirstName;
                usermodel.LastName = user.LastName;
                Session["CurrentUser"] = usermodel;

                model.Success = true;
                model.Message = user.IsAdmin ? "Admin" : "User";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                helper.ManageLogs(userid, "User Login to the account");
                model.Success = false;
                model.Message = ex.Message;
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult CreateUser(string email, string name, string password)
        {
            AjaxModel model = new AjaxModel();
            try
            {
                var existuser = FindUser(email);
                if (existuser != null)
                {
                    model.Success = false;
                    model.Message = "Email already exist in database.";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                User user = new User();
                user.FirstName = name;
                user.IsAdmin = false;
                user.Password = helper.Encode(password);
                user.Email = email;
                user.IsProfileCompleted = false;
                user.CreatedOn = DateTime.Now;
                user.IsActive = false;
                entity.Users.Add(user);
                entity.SaveChanges();
                helper.ManageLogs(user.Id, "New User Created");
                model.Success = true;
                model.Message = "Your account has been created successfull. Please wait untill administrator apporves your account.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                model.Success = false;
                model.Message = ex.Message;
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UpdatePassword(string email, string newPassword)
        {
            var user = FindUser(email);
            user.Password = helper.Encode(newPassword);
            entity.SaveChanges();
            helper.ManageLogs(user.Id, "Password updated by user.");
            return View();
        }

        [HttpPost]
        public JsonResult ResetPasswords(string email)
        {
            AjaxModel result = new AjaxModel();
            int userid = 0;
            try
            {
                var user = FindUser(email);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = "Email doesn't exist..";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                userid = user.Id;
                Random random = new Random();
                int value = random.Next(10000);
                user.RecoveryPassword = value;
                entity.SaveChanges();
                helper.ManageLogs(user.Id, "Password reset code genrated by user.");
                result.Success = true;
                result.Message = "Password recovery code has been send to your email. Please enter the code.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                helper.ManageLogs(userid, ex.Message);
                result.Success = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        private User FindUser(string email)
        {
            UserModel model = new UserModel();
            var user = (from usr in entity.Users where usr.Email.ToLower().ToString() == email.ToLower().ToString() select usr).FirstOrDefault();
            return user;
        }


    }
}