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
    public class HostelController : Controller
    {
        HostelManagmentEntities entity = new HostelManagmentEntities();
        Common helper = new Common();
        // GET: Hostel
        public ActionResult Index()
        {
            UserModel session = (UserModel)Session["CurrentUser"];
            if(session == null || !session.IsAdmin)
            {
                return RedirectToAction("Index", "Login");
            }
            var rooms = (from room in entity.HostelRooms orderby room.Id descending select room).ToList();

            return View(rooms);
        }

        public ActionResult AddRoom(int? Id)
        {
            UserModel session = (UserModel)Session["CurrentUser"];
            if (session == null || !session.IsAdmin)
            {
                return RedirectToAction("Index", "Login");
            }
            HostelRoom model = new HostelRoom();
            if (Id > 0)
            {
                model = (from room in entity.HostelRooms where room.Id == Id select room).FirstOrDefault();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveRoom(HostelRoom model)
        {
            AjaxModel result = new AjaxModel();
            try
            {
                UserModel session = (UserModel)Session["CurrentUser"];
                if (model == null)
                {
                    result.Success = false;
                    result.Message = "Internal Server Error.";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var info = (from room in entity.HostelRooms where room.RoomNo == model.RoomNo select room).FirstOrDefault();

                if (info != null && model.Id==0)
                {
                    result.Success = false;
                    result.Message = "Room Number already exist.";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result.Success = true;
                result.Message = model.Id == 0 ? "New Room Added Succesfully." : "Room Updated Successfully.";
                if (model.Id == 0)
                {
                    helper.ManageLogs(session.UserId, "New room added by " + session.FirstName + " " + session.LastName);
                }
                else
                {
                    helper.ManageLogs(session.UserId, "Room Number "+model.RoomNo+" updated by " + session.FirstName + " " + session.LastName);
                }
                entity.HostelRooms.Add(model);
                entity.SaveChanges();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public  ActionResult RoomsRequest()
        {
            var user = (from usr in entity.Users where usr.RoomRequested == true select usr).ToList();
            return View(user);
        }


        public ActionResult AssignRoom(int userId)
        {
            var user = (from usr in entity.Users where usr.Id == userId select usr).FirstOrDefault();
            return View(user);
        }
    }
}