using ASP_WEB.DAL.Repository;
using ASP_WEB.Models;
using Microsoft.Ajax.Utilities;
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
        FaqRepository repoFaq = new FaqRepository();
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

            if (file != null)
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");
                container.CreateIfNotExists();

                string[] url = file.FileName.Split('.');
                //oude fotourl
                CloudBlockBlob oudeBlob = container.GetBlockBlobReference(theme.FotoURL);
                oudeBlob.DeleteIfExists();
                //nieuwe fotourl
                theme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(theme.FotoURL);
                blockBlob.UploadFromStream(file.InputStream);
            }

            repoTheme.Update(theme);
            repoTheme.SaveChanges();
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


            if (file != null)
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");
                container.CreateIfNotExists();

                string[] url = file.FileName.Split('.');
                //oude fotourl
                if (theme.FotoURL == null) theme.FotoURL = "123";
                CloudBlockBlob oudeBlob = container.GetBlockBlobReference(theme.FotoURL);
                oudeBlob.DeleteIfExists();
                //nieuwe fotourl
                theme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(theme.FotoURL);
                blockBlob.UploadFromStream(file.InputStream);
            }
            repoTheme.Insert(theme);
            repoTheme.SaveChanges();
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
            if (theme.FotoURL == null) theme.FotoURL = "012345678909876543210.png";
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(theme.FotoURL);

            blockBlob.DeleteIfExists();

            repoTheme.Delete(ID);
            repoTheme.SaveChanges();

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
            SubthemesEditViewModel vm = new SubthemesEditViewModel();
            vm.subtheme = subtheme;
            vm.offices = repoOffice.All().ToList();
            vm.themes = repoTheme.All().ToList();
            return View(vm);
        }
        
        [HttpPost]
        public ActionResult EditSubtheme(FormCollection frm, HttpPostedFileBase file)
        {
            SubthemesEditViewModel vm = new SubthemesEditViewModel();
            vm.subtheme = new Subtheme();
            vm.subtheme.OfficeID = new List<int>();
            vm.subtheme.SubthemeID = Convert.ToInt32(frm[nameof(vm.subtheme.SubthemeID)]);
            #region Foto Bijwerken
            if (file != null)
            {

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");
                container.CreateIfNotExists();
                if (vm.subtheme.FotoURL == null) vm.subtheme.FotoURL = "123";
                CloudBlockBlob oudeBlob = container.GetBlockBlobReference(vm.subtheme.FotoURL);
                oudeBlob.DeleteIfExists();

                string[] url = file.FileName.Split('.');
                vm.subtheme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(vm.subtheme.FotoURL);
                blockBlob.UploadFromStream(file.InputStream);

            }
            else
            {
                vm.subtheme.FotoURL = repoSubtheme.GetByID(vm.subtheme.SubthemeID).FotoURL;
            }
            #endregion
            vm.subtheme.Description = frm[nameof(vm.subtheme.Description)].ToString();
            vm.subtheme.ThemeID = Convert.ToInt32(frm[nameof(vm.subtheme.ThemeID)]);
            if (frm.AllKeys.Contains("OfficeIDs"))
            {
                foreach (var item in frm.GetValues("OfficeIDs"))
                {
                    vm.subtheme.OfficeID.Add(Convert.ToInt32(item));
                }

                if (vm.subtheme.Office == null) vm.subtheme.Office = new List<Office>();
                foreach (var item in vm.subtheme.OfficeID)
                {
                    vm.subtheme.Office.Add(repoOffice.GetByID(item));
                }
            }
            
            vm.subtheme.Name = frm[nameof(vm.subtheme.Name)];
            repoSubtheme.Update(vm.subtheme);

            repoSubtheme.SaveChanges();
            
            return RedirectToAction(nameof(Subthemes));

        }

        public ActionResult CreateSubtheme()
        {
            SubthemesEditViewModel vm = new SubthemesEditViewModel();

            vm.themes = repoTheme.All().ToList();
            vm.offices = repoOffice.All().ToList();

            return View(vm);
        }
        [HttpPost]
        public ActionResult CreateSubtheme(FormCollection frm, HttpPostedFileBase file)
        {


            SubthemesEditViewModel vm = new SubthemesEditViewModel();
            vm.subtheme = new Subtheme();
            vm.subtheme.OfficeID = new List<int>();
            vm.subtheme.ThemeID = Convert.ToInt32(frm[nameof(vm.subtheme.ThemeID)]);
            vm.subtheme.Description = frm[nameof(vm.subtheme.Description)].ToString();
            foreach (int item in frm[nameof(vm.subtheme.OfficeID)])
            {
                vm.subtheme.OfficeID.Add(item);
            }
            vm.subtheme.Name = frm[nameof(vm.subtheme.Name)];
            #region Foto Bijwerken
            if (file != null)
            {

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");
                container.CreateIfNotExists();
                if (vm.subtheme.FotoURL == null) vm.subtheme.FotoURL = "123";
                CloudBlockBlob oudeBlob = container.GetBlockBlobReference(vm.subtheme.FotoURL);
                oudeBlob.DeleteIfExists();

                string[] url = file.FileName.Split('.');
                vm.subtheme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(vm.subtheme.FotoURL);
                blockBlob.UploadFromStream(file.InputStream);

            }
            #endregion
            repoSubtheme.Insert(vm.subtheme);
            repoSubtheme.SaveChanges();

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

        public ActionResult DeleteSubtheme(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Subthemes));
            }
            int ID = (int)id;
            Subtheme sub = repoSubtheme.GetByID(ID);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("images");

            container.CreateIfNotExists();
            if (sub.FotoURL == null) sub.FotoURL = "012345678909876543210.png";
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(sub.FotoURL);

            blockBlob.DeleteIfExists();

            repoSubtheme.Delete(ID);
            repoSubtheme.SaveChanges();

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
                return RedirectToAction(nameof(Offices));
            }
            int ID = (int)id;
            Office office = repoOffice.GetByID(ID);
            return View(office);

        }
        [HttpPost]
        public ActionResult EditOffice(FormCollection frm)
        {
            Office office = repoOffice.GetByID(Convert.ToInt32(frm[nameof(Office.OfficeID)]));
            office.Name = frm[nameof(Office.Name)];
            office.City = frm[nameof(Office.City)];
            office.EmailAddress = frm[nameof(Office.EmailAddress)];
            office.HouseNumber = frm[nameof(Office.HouseNumber)];
            office.OpeningHours = frm[nameof(Office.OpeningHours)];
            office.PhoneNumber = frm[nameof(Office.PhoneNumber)];
            office.Street = frm[nameof(Office.Street)];
            office.URL = frm[nameof(Office.URL)];
            office.ZipCode = Convert.ToInt32(frm[nameof(Office.ZipCode)]);

            repoOffice.Update(office);
            repoOffice.SaveChanges();

            return RedirectToAction(nameof(Offices));
        }

        public ActionResult CreateOffice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOffice(FormCollection frm)
        {
            Office office = new Office();
            office.Name = frm[nameof(Office.Name)];
            office.City = frm[nameof(Office.City)];
            office.EmailAddress = frm[nameof(Office.EmailAddress)];
            office.HouseNumber = frm[nameof(Office.HouseNumber)];
            office.OpeningHours = frm[nameof(Office.OpeningHours)];
            office.PhoneNumber = frm[nameof(Office.PhoneNumber)];
            office.Street = frm[nameof(Office.Street)];
            office.URL = frm[nameof(Office.URL)];
            office.ZipCode = Convert.ToInt32(frm[nameof(Office.ZipCode)]);
            repoOffice.Insert(office);
            repoOffice.SaveChanges();

            return RedirectToAction(nameof(Offices));
        }

        public ActionResult DetailsOffice(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Offices));
            }
            int ID = (int)id;
            Office office = repoOffice.GetByID(ID);
            return View(office);
        }

        public ActionResult DeleteOffice(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Offices));
            }
            int ID = (int)id;
            repoOffice.Delete(ID);
            repoOffice.SaveChanges();
            return RedirectToAction(nameof(Offices));
        }
        #endregion

        #region FAQ

        public ActionResult Faqs()
        {
            return View(repoFaq.All());
        }

        public ActionResult CreateFaq()
        {
            FaqSubtheme vm = new FaqSubtheme();
            vm.Subtheme = repoSubtheme.All().ToList();
            vm.Theme = repoTheme.All().ToList();
            return View(vm);
        }
        [HttpPost]
        public ActionResult CreateFaq(FormCollection frm)
        {
            Faq faq = new Faq();
            faq.Question = frm[nameof(faq.Question)];
            faq.SubthemeID = Convert.ToInt32(frm[nameof(faq.SubthemeID)]);
            faq.ThemeID = Convert.ToInt32(frm[nameof(faq.ThemeID)]);
            faq.Answer = frm[nameof(faq.Answer)];
            repoFaq.Insert(faq);
            repoFaq.SaveChanges();
            return RedirectToAction(nameof(Faqs));
        }
        public ActionResult EditFaq(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Faqs));
            }

            int ID = (int)id;
            Faq faq = repoFaq.GetByID(ID);
            FaqSubtheme vm = new FaqSubtheme();
            vm.faqEen = faq;
            vm.Subtheme = repoSubtheme.All().ToList();
            vm.Theme = repoTheme.All().ToList();

            return View(vm);

        }
        [HttpPost]
        public ActionResult EditFaq(FormCollection frm)
        {
            Faq faq = new Faq();
            faq.Answer = frm[nameof(faq.Answer)];
            faq.FaqID = Convert.ToInt32(frm[nameof(faq.FaqID)]);
            faq.Question = frm[nameof(faq.Question)];
            faq.SubthemeID = Convert.ToInt32(frm[nameof(faq.SubthemeID)]);
            faq.ThemeID = Convert.ToInt32(frm.GetValues("ThemeID")[0]);
            repoFaq.Update(faq);
            repoFaq.SaveChanges();
            return RedirectToAction(nameof(Faqs));
        }
        public ActionResult DeleteFaq(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Faqs));
            }
            int ID = (int)id;
            repoFaq.Delete(ID);
            repoFaq.SaveChanges();
            return RedirectToAction(nameof(Faqs));
        }
        public ActionResult DetailsFaq(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Faqs));
            }
            int ID = (int)id;
            Faq faq = new Faq();
            faq = repoFaq.GetByID(ID);
            return View(faq);
        }
        #endregion

        #region Users and Roles

        #endregion

    }
}