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
    /// <summary>
    /// HomeController
    /// </summary>
    public class HomeController : Controller
    {
        GenericRepository<Theme> repoTheme = new GenericRepository<Theme>();
        FaqRepository repoFaq = new FaqRepository();
        SubthemeRepository repoSubtheme = new SubthemeRepository();
        /// <summary>
        /// Returns a view with all the themes
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            IEnumerable<Theme> Themes = new List<Theme>();
            Themes = repoTheme.All();

            return View(Themes);
        }
        /// <summary>
        /// Returns a view with the details of a subtheme
        /// </summary>
        /// <param name="id">SubthemeID</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Subtheme", new { id = id });
        }
        /// <summary>
        /// Returns the map view
        /// </summary>
        /// <returns></returns>
        public ActionResult Map()
        {
            return View();
        }
        /// <summary>
        /// Returns a view with all FAQs and all themes
        /// </summary>
        /// <returns></returns>
        public ActionResult FAQ()
        {
            FaqSubtheme vm = new FaqSubtheme();
            List<Faq> faq = repoFaq.All().OrderBy(f => f.SubthemeID).ToList();
            vm.Faq = faq;
            List<Theme> themes = repoTheme.All().ToList();
            vm.Theme = themes;
            return View(vm);
        }
        /// <summary>
        /// Shows the search results of a given page
        /// </summary>
        /// <param name="searchString">SearchString in Dutch</param>
        /// <param name="page">PageNumber</param>
        /// <returns></returns>
        public ActionResult Search(string searchString, int? page)
        {
            if (String.IsNullOrWhiteSpace(searchString))
            {
                return RedirectToAction("Index");
            }
            //TODO: searchstring eventueel bewerken
            List<Subtheme> subthemes = repoSubtheme.Search(searchString);
            ViewBag.searchString = searchString;
            int pageSize = 5;
            //List<Faq> faqs = repoFaq.Search(searchString);
            //FaqSubtheme list = new FaqSubtheme();
            //list.Faq = faqs;
            //list.Subtheme = subthemes;
            int pageNumber = (page ?? 1);
            var pagedlist = subthemes.ToPagedList(pageNumber, pageSize);
            return View(pagedlist);
        }
        /// <summary>
        /// Returns teh Contact View
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }
        /// <summary>
        /// Sends a mail with the content of the contact form
        /// </summary>
        /// <param name="frm">FormData</param>
        /// <returns></returns>
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