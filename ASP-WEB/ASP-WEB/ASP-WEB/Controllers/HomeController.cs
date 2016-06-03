using ASP_WEB.DAL.Repository;
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
        FaqRepository repoFaq = new FaqRepository();
        SubthemeRepository repoSubtheme = new SubthemeRepository();

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

        public ActionResult Map()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            FaqSubtheme vm = new FaqSubtheme();
            List<Faq> faq = repoFaq.All().OrderBy(f => f.SubthemeID).OrderBy(f => f.Theme).ToList();
            vm.Faq = faq;
            List<Theme> themes = repoTheme.All().ToList();
            vm.Theme = themes;
            return View(vm);
        }

        public ActionResult Search(string searchString)
        {
            if (String.IsNullOrWhiteSpace(searchString))
            {
                return RedirectToAction("Index");
            }
            //TODO: searchstring eventueel bewerken
            List<Subtheme> subthemes = repoSubtheme.Search(searchString);
            List<Faq> faqs = repoFaq.Search(searchString);
            FaqSubtheme list = new FaqSubtheme();
            list.Faq = faqs;
            list.Subtheme = subthemes;
            return View(list);
        }
    }
}