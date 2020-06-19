using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibVisWeb.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {

            ViewBag.manifest = "https://visaolibertaria.com/dist/img/manifest.json";
            ViewBag.shortcut = "https://visaolibertaria.com/dist/img/favicon.ico";
            ViewBag.icon = "https://visaolibertaria.com/dist/img/icon.png";
            ViewBag.Title = "Visão Libertária";

            if (RacLib.Base.conf.alternativeSite == 1)
            {

                ViewBag.manifest = "https://visaolibertaria.com/dist/img/manifest_ancapsu.json";
                ViewBag.shortcut = "https://visaolibertaria.com/dist/img/favicon_ancapsu.ico";
                ViewBag.icon = "https://visaolibertaria.com/dist/img/icon_ancapsu.png";
                ViewBag.Title = "ancap.su";

            }

            return View();
            
        }

        public IActionResult Error()
        {

            ViewBag.manifest = "https://visaolibertaria.com/dist/img/manifest.json";
            ViewBag.shortcut = "https://visaolibertaria.com/dist/img/favicon.ico";
            ViewBag.icon = "https://visaolibertaria.com/dist/img/icon.png";
            ViewBag.Title = "Visão Libertária - Error";
            
            if (RacLib.Base.conf.alternativeSite == 1)
            {

                ViewBag.manifest = "https://visaolibertaria.com/dist/img/manifest_ancapsu.json";
                ViewBag.shortcut = "https://visaolibertaria.com/dist/img/favicon_ancapsu.ico";
                ViewBag.icon = "https://visaolibertaria.com/dist/img/icon_ancapsu.png";
                ViewBag.Title = "ancap.su - Error";

            }
            
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            return View();

        }

    }

}
