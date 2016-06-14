using ASP_WEB.DAL.Repository;
using ASP_WEB.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            List<Faq> faq = repoFaq.All().OrderBy(f => f.SubthemeID).ToList();
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

            //List<Faq> faqs = repoFaq.Search(searchString);
            //FaqSubtheme list = new FaqSubtheme();
            //list.Faq = faqs;
            //list.Subtheme = subthemes;
            return View(subthemes);
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(FormCollection frm)
        {
            string Name = frm["Name"];
            string Email = frm["Email"];
            string Subject = frm["Subject"];
            string Description = frm["Description"];

            MailMessage o = new MailMessage("matthieu19@msn.com", "louisguy.meersseman19@gmail.com", Subject, "Name: " + Name + "</br> Email: " + Email + "</br> Subject: " + Subject + "</br> Description: " + Description);
            o.IsBodyHtml = true;
            NetworkCredential netCred = new NetworkCredential("matthieu19@msn.com", "azeAZE$69");
            SmtpClient smtpobj = new SmtpClient("smtp.live.com", 587);
            smtpobj.EnableSsl = true;
            smtpobj.Credentials = netCred;
            smtpobj.Send(o);
            return RedirectToAction(nameof(Contact));
        }
    }
}