using ASP_WEB.DAL.Repository;
using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_WEB.Controllers
{
    /// <summary>
    /// SubthemeController
    /// </summary>
    public class SubthemeController : Controller
    {
        SubthemeRepository repoSubtheme = new SubthemeRepository();
        // GET: Subtheme
        /// <summary>
        /// Returns a view with all Subthemes of a theme
        /// </summary>
        /// <param name="id">ThemeID</param>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            int themeID = (int) id;
            List<Subtheme> Subthemes = new List<Subtheme>();
            Subthemes = repoSubtheme.GetSubthemeByTheme(themeID);
            return View(Subthemes);
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
                return RedirectToAction("Index", "Home");
            }

            int subthemeID = (int)id;
            Subtheme subtheme = repoSubtheme.GetByID(subthemeID);
            // ICollection<Office> Offices = subtheme.Office.ToList();
            return View(subtheme);

        }
    }
}