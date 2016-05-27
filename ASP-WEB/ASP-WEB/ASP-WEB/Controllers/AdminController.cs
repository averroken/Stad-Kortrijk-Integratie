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
            Theme theme = repoTheme.GetByID(Convert.ToInt32(frm["ThemeID"]));
            theme.Name = frm["Name"];

            if (theme.FotoURL != frm["FotoURL"])
            {
                string[] url = frm["FotoURL"].Split('.');

                theme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];
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
            string[] url = file.FileName.Split('.');
            theme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];
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