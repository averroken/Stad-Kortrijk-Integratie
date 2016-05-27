using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP_WEB.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        GenericRepository<Theme> repoTheme = new GenericRepository<Theme>();
        // GET: Admin
        /// <summary>
        /// Shows list of different editing and adding possibilities
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region Themes
        public ActionResult Themes()
        {
            IEnumerable<Theme> themes = repoTheme.All();
            return View(themes);
        }

        public ActionResult EditTheme(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Themes");
            }
            int ID = (int)id;
            Theme theme = repoTheme.GetByID(ID);
            return View(theme);

        }
        [HttpPost]
        public ActionResult EditTheme(FormCollection frm, HttpPostedFileBase file)
        {
            Theme theme = repoTheme.GetByID(frm["id"]);
            theme.Name = frm["name"];

            if (theme.FotoURL != frm["fotoURL"])
            {
                theme.FotoURL = new Guid().ToString();
            }

            repoTheme.Update(theme);
            repoTheme.SaveChanges();
            //TODO file save to cloud blob storage + change filename to theme.FotoURL
            return RedirectToAction("Themes");
        }
        public ActionResult CreateTheme()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTheme(FormCollection frm, HttpPostedFileBase file)
        {
            Theme theme = new Theme();
            theme.Name = frm["name"];
            theme.FotoURL = new Guid().ToString();
            repoTheme.Insert(theme);
            repoTheme.SaveChanges();
            //TODO file save to cloud blob storage + change filename to theme.FotoURL
            return RedirectToAction("Themes");
        }
        public ActionResult DetailsTheme(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Themes");
            }
            int ID = (int)id;
            Theme theme = repoTheme.GetByID(ID);
            return View(theme);
        }
        #endregion
    }
}