using ASP_WEB.DAL.Repository;
using ASP_WEB.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
        GenericRepository<Office> repoOffice = new GenericRepository<Office>();
        SubthemeRepository repoSubtheme = new SubthemeRepository();
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=integratiekortrijk;AccountKey=W0gcFRQX42eNg/msSVLvYydtYY3stHagwjVDaFvsFoaLEUjXuQ4rJHavDn8pwfrggkN8qyZJDMkOyAYIcwJt0Q==");

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
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");
                container.CreateIfNotExists();


                string[] url = frm["FotoURL"].Split('.');
                //oude fotourl
                CloudBlockBlob oudeBlob = container.GetBlockBlobReference(theme.FotoURL);
                oudeBlob.DeleteIfExists();
                //nieuwe fotourl
                theme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];


                repoTheme.Update(theme);
                repoTheme.SaveChanges();

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(theme.FotoURL);
                blockBlob.UploadFromStream(file.InputStream);
            }
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
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("images");
            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(theme.FotoURL);
            blockBlob.UploadFromStream(file.InputStream);
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

        public ActionResult DeleteTheme(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Themes");
            }

            int ID = (int)id;
            Theme theme = repoTheme.GetByID(ID);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("images");

            container.CreateIfNotExists();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(theme.FotoURL);

            blockBlob.Delete();

            repoTheme.Delete(ID);

            return RedirectToAction("Themes");
        }
        #endregion

        #region Subthemes
        public ActionResult Subthemes()
        {
            IEnumerable<Subtheme> subthemes = repoSubtheme.All();
            return View(subthemes);
        }

        public ActionResult EditSubtheme(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Subthemes));
            }
            int ID = (int)id;
            Subtheme subtheme = repoSubtheme.GetByID(ID);
            return View(subtheme);
        }
        //TODO EditSubtheme
        [HttpPost]
        public ActionResult EditSubtheme(FormCollection frm, HttpPostedFileBase file)
        {


            return RedirectToAction(nameof(Subthemes));
        }

        public ActionResult CreateSubtheme()
        {
            return View();
        }
        //TODO CreateSubtheme
        [HttpPost]
        public ActionResult CreateSubtheme(FormCollection frm, HttpPostedFileBase file)
        {

            return RedirectToAction(nameof(Subthemes));
        }
        public ActionResult DetailsSubtheme(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Subthemes));
            }
            int ID = (int)id;
            Subtheme subtheme = repoSubtheme.GetByID(ID);
            return View(subtheme);
        }

        [HttpPost]
        public ActionResult DeleteSubtheme(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Subthemes));
            }
            int ID = (int)id;
            repoSubtheme.Delete(ID);

            return RedirectToAction(nameof(Subthemes));
        }
        #endregion

        #region Office
        public ActionResult Offices()
        {
            IEnumerable<Office> offices = repoOffice.All();
            return View(offices);
        }

        public ActionResult EditOffice(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Offices");
            }
            int ID = (int)id;
            Office office = repoOffice.GetByID(ID);
            return View(office);

        }
        [HttpPost]
        public ActionResult EditOffice(FormCollection frm)
        {
            Office office = repoOffice.GetByID(Convert.ToInt32(frm["OfficeID"]));
            office.Name = frm["Name"];
            office.City = frm["City"];
            office.EmailAddress = frm["EmailAddress"];
            office.HouseNumber = frm["HouseNumber"];
            office.OpeningHours = frm["OpeningHours"];
            office.PhoneNumber = frm["PhoneNumber"];
            office.Street = frm["Street"];
            office.URL = frm["URL"];
            office.ZipCode = Convert.ToInt32(frm["ZipCode"]);

            repoOffice.Update(office);
            repoOffice.SaveChanges();

            return RedirectToAction("Offices");
        }

        public ActionResult CreateOffice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOffice(FormCollection frm)
        {
            Office office = new Office();
            office.Name = frm["Name"];
            office.City = frm["City"];
            office.EmailAddress = frm["EmailAddress"];
            office.HouseNumber = frm["HouseNumber"];
            office.OpeningHours = frm["OpeningHours"];
            office.PhoneNumber = frm["PhoneNumber"];
            office.Street = frm["Street"];
            office.URL = frm["URL"];
            office.ZipCode = Convert.ToInt32(frm["ZipCode"]);
            repoOffice.Insert(office);
            repoOffice.SaveChanges();

            return RedirectToAction("Offices");
        }

        public ActionResult DetailsOffice(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Offices");
            }
            int ID = (int)id;
            Office office = repoOffice.GetByID(ID);
            return View(office);
        }
        #endregion
        //TODO FAQ
        #region FAQ

        #endregion


    }
}