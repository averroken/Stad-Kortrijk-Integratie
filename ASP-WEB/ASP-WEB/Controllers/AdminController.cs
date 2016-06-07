using ASP_WEB.DAL.Repository;
using ASP_WEB.Helpers;
using ASP_WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ASP_WEB.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private const int pageSize = 5;

        GenericRepository<Theme> repoTheme = new GenericRepository<Theme>();
        GenericRepository<Office> repoOffice = new GenericRepository<Office>();
        SubthemeRepository repoSubtheme = new SubthemeRepository();
        FaqRepository repoFaq = new FaqRepository();
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=integratiekortrijk;AccountKey=W0gcFRQX42eNg/msSVLvYydtYY3stHagwjVDaFvsFoaLEUjXuQ4rJHavDn8pwfrggkN8qyZJDMkOyAYIcwJt0Q==");

        /// <summary>
        /// Shows list of different editing and adding possibilities
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region Themes
        public ActionResult Themes(int? page)
        {
            IEnumerable<Theme> themes = repoTheme.All();
            int pageNumber = (page ?? 1);
            return View(themes.OrderBy(i => i.ThemeID).ToPagedList(pageNumber, pageSize));
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
        public ActionResult Subthemes(int? page)
        {
            IEnumerable<Subtheme> subthemes = repoSubtheme.All();
            int pageNumber = (page ?? 1);
            return View(subthemes.OrderBy(i => i.SubthemeID).ToPagedList(pageNumber, pageSize));
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
        [ValidateInput(false)]
        public ActionResult EditSubtheme(FormCollection frm, HttpPostedFileBase file)
        {
            Subtheme newSubtheme = repoSubtheme.GetByID(frm["SubthemeID"]);
            newSubtheme.Description = frm["Description"];
            newSubtheme.Name = frm["Name"];
            newSubtheme.ThemeID = Convert.ToInt32(frm["ThemeID"]);
            if (frm.GetValues("OfficeIDs") != null)
            {
                newSubtheme.OfficeID = new List<int>();
                foreach (var item in frm.GetValues("OfficeIDs"))
                {

                    newSubtheme.OfficeID.Add(Convert.ToInt32(item));
                }
            }

            #region Foto Bijwerken
            if (file != null)
            {

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");
                container.CreateIfNotExists();
                CloudBlockBlob oudeBlob = container.GetBlockBlobReference(newSubtheme.FotoURL);
                oudeBlob.DeleteIfExists();

                string[] url = file.FileName.Split('.');
                newSubtheme.FotoURL = Guid.NewGuid().ToString() + "." + url[1];
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(newSubtheme.FotoURL);
                blockBlob.UploadFromStream(file.InputStream);

            }
            #endregion

            repoSubtheme.Update(newSubtheme);
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
            //foreach (var item in frm.GetValues("OfficeIDs"))
            //{

            //    newSubtheme.OfficeID.Add(Convert.ToInt32(item));
            //}
            if (vm.subtheme.Office == null) vm.subtheme.Office = new List<Office>();
            foreach (var item in frm.GetValues(nameof(vm.subtheme.OfficeID)))
            {
                vm.subtheme.OfficeID.Add(Convert.ToInt32(item));
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
        public ActionResult Offices(int? page)
        {
            IEnumerable<Office> offices = repoOffice.All();
            int pageNumber = (page ?? 1);

            return View(offices.OrderBy(i => i.OfficeID).ToPagedList(pageNumber, pageSize));
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
            office.Monday = frm[nameof(Office.Monday)];
            office.Tuesday = frm[nameof(Office.Tuesday)];
            office.Wednesday = frm[nameof(Office.Wednesday)];
            office.Thursday = frm[nameof(Office.Thursday)];
            office.Friday = frm[nameof(Office.Friday)];
            office.Saturday = frm[nameof(Office.Saturday)];
            office.Sunday = frm[nameof(Office.Sunday)];
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
            office.Monday = frm[nameof(Office.Monday)];
            office.Tuesday = frm[nameof(Office.Tuesday)];
            office.Wednesday = frm[nameof(Office.Wednesday)];
            office.Thursday = frm[nameof(Office.Thursday)];
            office.Friday = frm[nameof(Office.Friday)];
            office.Saturday = frm[nameof(Office.Saturday)];
            office.Sunday = frm[nameof(Office.Sunday)];
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

        public ActionResult Faqs(int? page)
        {
            IEnumerable<Faq> Faq = repoFaq.All();
            int pageNumber = (page ?? 1);
            return View(Faq.OrderBy(f => f.FaqID).ToPagedList(pageNumber, pageSize));
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
            faq.Answer = frm[nameof(faq.Answer)];
            faq.SubthemeID = Convert.ToInt32(frm[nameof(faq.SubthemeID)]);
            faq.ThemeID = Convert.ToInt32(frm[nameof(faq.ThemeID)]);
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
            faq.ThemeID = Convert.ToInt32(frm[nameof(faq.ThemeID)]);
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
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Users()
        {
            List<User> vm = new List<User>();
            List<ApplicationUser> users = UserManager.Users.ToList();
            foreach (var gebruiker in users)
            {
                User user = new User();
                user.ID = gebruiker.Id;
                user.Email = gebruiker.Email;
                user.UserName = gebruiker.UserName;
                var inrole = UserManager.IsInRole(user.ID, Roles.ADMINISTRATOR.ToString());
                if (inrole) user.IsAdmin = true;
                else user.IsAdmin = false;
                if (!String.IsNullOrWhiteSpace(gebruiker.PasswordHash)) vm.Add(user);

            }
            return View(vm);
        }

        public ActionResult DeleteUser(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                UserManager.RemovePassword(id);
            }
            return RedirectToAction(nameof(Users));
        }

        public ActionResult ToAdmin(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                IdentityResult removeFromRole = UserManager.RemoveFromRole(id, Roles.USER.ToString());
                UserManager.AddToRole(id, Roles.ADMINISTRATOR.ToString());

            }
            return RedirectToAction(nameof(Users));
        }

        public ActionResult ToUser(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                IdentityResult removeFromRole = UserManager.RemoveFromRole(id, Roles.ADMINISTRATOR.ToString());
                UserManager.AddToRole(id, Roles.USER.ToString());
            }
            return RedirectToAction(nameof(Users));
        }
        #endregion

    }
}