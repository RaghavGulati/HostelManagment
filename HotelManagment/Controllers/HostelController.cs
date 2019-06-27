using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagment.Controllers
{
    public class HostelController : Controller
    {
        // GET: Hostel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddRoom()
        {
            return View();
        }

        public ActionResult AssignRoom()
        {
            return View();
        }
    }
}