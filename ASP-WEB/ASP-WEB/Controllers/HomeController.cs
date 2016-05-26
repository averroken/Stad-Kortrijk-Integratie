using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_WEB.Controllers
{
    public class HomeController : Controller
    {
        GenericRepository<Theme> repoTheme = new GenericRepository<Theme>();
        GenericRepository<Faq> repoFaq = new GenericRepository<Faq>();

        public ActionResult Index()
        {
            IEnumerable<Theme> Themes = new List<Theme>();
            Themes = repoTheme.All();

            return View(Themes);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Subtheme", new { id = id });
        }

        public ActionResult List()
        {
            IEnumerable<Theme> Themes = new List<Theme>();
            Themes = repoTheme.All();

            return View(Themes);
        }

        public ActionResult Map()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            IEnumerable<Faq> faq = repoFaq.All();
            return View(faq);
        }
    }
}