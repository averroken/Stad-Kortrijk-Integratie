using ASP_WEB.DAL.Context;
using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.DAL.Repository
{
    /// <summary>
    /// Repository for subthemes
    /// </summary>
    public class SubthemeRepository : GenericRepository<Subtheme>, IGenericRepository<Subtheme>
    {
        /// <summary>
        /// Gets all subthemes of a theme
        /// </summary>
        /// <param name="themeID">ThemeID</param>
        /// <returns></returns>
        public List<Subtheme> GetSubthemeByTheme(int themeID)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Where(st => st.ThemeID == themeID).Include(st => st.Office).Include(st => st.Theme).ToList<Subtheme>();
            }
        }
        /// <summary>
        /// Gets all subthemes
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Subtheme> All()
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Include(st => st.Office).Include(st => st.Theme).ToList<Subtheme>();
            }
        }
        /// <summary>
        /// Gets a subtheme
        /// </summary>
        /// <param name="id">SubthemeID</param>
        /// <returns></returns>
        public override Subtheme GetByID(object id)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                int ID = Convert.ToInt32(id.ToString());
                return context.Subtheme.Where(st => st.SubthemeID == ID).Include(st => st.Office).Include(st => st.Theme).SingleOrDefault<Subtheme>();
            }
        }
        /// <summary>
        /// Searches if searchString is in the description or the name of a subtheme
        /// </summary>
        /// <param name="searchString">searchString in Dutch</param>
        /// <returns></returns>
        public List<Subtheme> Search(string searchString)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Where(st => st.Description.Contains(searchString) || st.Name.Contains(searchString)).Include(st => st.Office).Include(st => st.Theme).ToList<Subtheme>();
            }
        }
        /// <summary>
        /// Inserts a subtheme
        /// </summary>
        /// <param name="subtheme">Subthema</param>
        /// <returns></returns>
        public override Subtheme Insert(Subtheme subtheme)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                GenericRepository<Office> repoOffice = new GenericRepository<Office>(context);
                foreach (int item in subtheme.OfficeID)
                {
                    subtheme.Office.Add(repoOffice.GetByID(item));
                }
                context.Subtheme.Add(subtheme);
                context.SaveChanges();
            }
            return subtheme;
        }
        /// <summary>
        /// Updates a subtheme
        /// </summary>
        /// <param name="subtheme">Subtheme</param>
        public override void Update(Subtheme subtheme)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                var currentSubtheme = (from s in context.Subtheme.Include(s => s.Office)
                                       where s.SubthemeID == subtheme.SubthemeID
                                       select s).SingleOrDefault<Subtheme>();
                currentSubtheme.Office.Clear();
                context.SaveChanges();

                GenericRepository<Office> repoOffice = new GenericRepository<Office>(context);
                GenericRepository<Theme> repoTheme = new GenericRepository<Theme>(context);
                if (subtheme.OfficeID != null)
                {
                    foreach (var office in subtheme.OfficeID)
                    {
                        currentSubtheme.Office.Add(repoOffice.GetByID(office));
                    }
                }
                //currentSubtheme.Office.Clear();
                //context.SaveChanges();
                currentSubtheme.Name = subtheme.Name;
                currentSubtheme.FotoURL = subtheme.FotoURL;
                currentSubtheme.Description = subtheme.Description;
                currentSubtheme.Theme = repoTheme.GetByID(subtheme.ThemeID);

                context.Entry<Subtheme>(currentSubtheme).State = EntityState.Modified;

                context.SaveChanges();
            }

            #region Comments

            //     public void UpdateContact(Contact contact)
            //{
            //    using (Labo1Context context = new Labo1Context())
            //    {
            //        var currentContact = (from c in context.Contacts.Include(c => c.Departement)
            //                              where c.ContactId == contact.ContactId
            //                              select c).SingleOrDefault<Contact>();
            //        currentContact.Departement.Clear();
            //        context.SaveChanges();
            //using (IntegratieContext context = new IntegratieContext())
            //{
            //    var currentSubtheme = (from c in context.Subtheme.Include(c => c.Office)
            //                           where c.SubthemeID == subtheme.SubthemeID
            //                           select c).SingleOrDefault<Subtheme>();
            //    currentSubtheme.Office.Clear();
            //    context.SaveChanges();
            //}
            //if (subtheme.Office != null)
            //{
            //    using (IntegratieContext context = new IntegratieContext())
            //    {
            //        //if (subtheme.Office == null) subtheme.Office = new List<Office>();
            //        foreach (var dep in subtheme.Office)
            //        {
            //            context.Entry<Office>(dep).State = EntityState.Added;
            //            context.Entry<Subtheme>(subtheme).State = EntityState.Modified;
            //            context.SaveChanges();
            //        }
            //    }

            //    }

            //    using (Labo1Context context = new Labo1Context())
            //    {
            //        foreach (var dep in contact.Departement)
            //            context.Entry<Departement>(dep).State = EntityState.Added;
            //        context.Entry<Contact>(contact).State = EntityState.Modified;
            //        context.SaveChanges();
            //    }

            //}
            //         base.Update(entityToUpdate);
            //    }
            //}
            #endregion
        }
        /// <summary>
        /// Deletes a subtheme
        /// </summary>
        /// <param name="id">SubthemeID</param>
        public override void Delete(object id)
        {
            Subtheme subtheme = new Subtheme { SubthemeID = Convert.ToInt32(id.ToString()) };
            Delete(subtheme);
        }
        /// <summary>
        /// Deletes a subtheme
        /// </summary>
        /// <param name="subtheme">Subtheme</param>
        public override void Delete(Subtheme subtheme)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                context.Entry<Subtheme>(subtheme).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}
