using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MWR.Controllers
{
    public class ErrorHandlerController : Controller
    {
        // GET: ErroHandler
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}