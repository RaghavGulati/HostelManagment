using HotelManagment.Database_Model;
using HotelManagment.Models;
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
            AdminModal modal = new AdminModal();
            var logs = (from log in entity.AccessLogs select log).ToList();
            modal.Logs = logs;
            return View(modal);
        }
    }
}