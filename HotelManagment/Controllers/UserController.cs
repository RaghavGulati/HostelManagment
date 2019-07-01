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
    public class UserController : Controller
    {
        HostelManagmentEntities entity = new HostelManagmentEntities();
        Common helper = new Common();
        // GET: User
        public ActionResult Index()
        {
            UserModel session = (UserModel)Session["CurrentUser"];
            if (session == null || !session.IsAdmin)
            {
                return RedirectToAction("Index", "Login");
            }
            var users = (from usr in entity.Users select usr).ToList();
            return View(users);
        }

        public ActionResult Profile(int? Id)
        {
            var sessionUser = CheckUserSession();
            if (sessionUser == null)
                return RedirectToRoute("default");

            UserModel model = new UserModel();
            User user = new User();
            if(Id==null || Id==0){
                user = (from usr in entity.Users where usr.Id == sessionUser.UserId select usr).FirstOrDefault();
            }
            else
            {
                user = (from usr in entity.Users where usr.Id == Id select usr).FirstOrDefault();
            }
            
            model.Countries = (from cntry in entity.Countries select cntry).ToList();
            model.State = (from state in entity.States select state).ToList();
            model.Cities = (from city in entity.Cities select city).ToList();
            
            model.UserId = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.IsActive = user.IsActive;
            model.IsAdmin = user.IsAdmin;
            model.Mobile = user.Mobile;
            model.Dob = Convert.ToDateTime(user.Dob);
            model.Gender = user.Gender;
            var address = (from add in entity.User_Address where add.UserId == model.UserId || add.IsPrimary == true select add).FirstOrDefault(); //user.User_Address.Where(x => x.IsPrimary == true).FirstOrDefault();
            if(address != null)
            {
                model.AddressId = address.Id;
                model.Address1 = address.Address1;
                model.Address2 = address.Address2;
                model.PostCode = address.PostCode;
                model.CityId = address.CityId;
                model.StateId = address.StateId;
                model.CountryId = address.CountryId;
                model.IsPrimary = address.IsPrimary;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateProfile(UserModel model)
        {
            AjaxModel result = new AjaxModel();
            try
            {
                var user = (from usr in entity.Users where usr.Id == model.UserId select usr).FirstOrDefault();
                if(user== null)
                {
                    result.Success = false;
                    result.Message = "User account not found. Please contact administrator.";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.IsActive = model.IsActive;
                user.IsAdmin = model.IsAdmin;
                user.Mobile = model.Mobile;
                user.Dob = model.Dob;
                user.IsProfileCompleted = true;//model.AddressId == 0 ? true : user.IsProfileCompleted;
                user.Gender = model.Gender;
                model.IsProfileCompleted = true;
                Session["CurrentUser"] = model; 
                User_Address address = new User_Address();
                if (model.AddressId > 0)
                {
                    address = (from add in entity.User_Address where add.Id == model.AddressId select add).FirstOrDefault();
                }
                address.IsPrimary = model.AddressId == 0 ? true : model.IsPrimary;
                address.PostCode = model.PostCode;
                address.Address1 = model.Address1;
                address.UserId = model.UserId;
                address.Address2 = model.Address2;
                address.CityId = model.CityId;
                address.StateId = model.StateId;
                address.CountryId = model.CountryId;
                if (model.AddressId == 0)
                {
                    entity.User_Address.Add(address);
                }
                entity.SaveChanges();
                helper.ManageLogs(model.UserId, "Profile updated by user.");
                result.Success = true;
                result.Message = "Profile updated successfully.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
          
        }
        

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(string old,string newpass)
        {
            AjaxModel result = new AjaxModel();
            int userid = 0;
            try
            {
                var user = CheckUserSession();
                if(user==null)
                    return RedirectToRoute("default");

                userid = user.UserId;
                if (helper.Decode(user.Password) != old)
                {
                    helper.ManageLogs(user.UserId, "old password mismatch while updating it with new password..");
                    result.Success = false;
                    result.Message = "Old Password is not correct";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                user.Password = helper.Encode(newpass);
                entity.SaveChanges();
                helper.ManageLogs(user.UserId, "Password updated successfully.");
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


        private UserModel CheckUserSession()
        {
            var user = (UserModel)Session["CurrentUser"];
            return user;
        }
    }
}