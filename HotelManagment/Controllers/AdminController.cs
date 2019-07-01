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
    public class AdminController : Controller
    {
        HostelManagmentEntities entity = new HostelManagmentEntities();
        // GET: Admin
        public ActionResult Index()
        {
            UserModel session = (UserModel)Session["CurrentUser"];
            if (session == null || !session.IsAdmin)
            {
                return RedirectToAction("Index", "Login");
            }
            AdminModal modal = new AdminModal();
            var logs = (from log in entity.AccessLogs orderby log.Id descending select log).ToList();
            modal.Logs = logs;
            return View(modal);
        }
    }
}