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
    public class SubthemeRepository : GenericRepository<Subtheme>, IGenericRepository<Subtheme>
    {

        public List<Subtheme> GetSubthemeByTheme(int themeID)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Where(st => st.ThemeID == themeID).Include(st => st.Office).Include(st => st.Theme).ToList<Subtheme>();
            }
        }

        public override IEnumerable<Subtheme> All()
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Include(st => st.Office).Include(st => st.Theme).ToList<Subtheme>();
            }
        }

        public override Subtheme GetByID(object id)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                int ID = Convert.ToInt32(id.ToString());
                return context.Subtheme.Where(st => st.SubthemeID == ID).Include(st => st.Office).Include(st => st.Theme).SingleOrDefault<Subtheme>();
            }
        }

        public List<Subtheme> Search(string searchString)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Subtheme.Where(st => st.Description.Contains(searchString) || st.Name.Contains(searchString)).Include(st => st.Office).Include(st => st.Theme).ToList<Subtheme>();
            }
        }

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
                foreach (var office in subtheme.OfficeID)
                {
                    currentSubtheme.Office.Add(repoOffice.GetByID(office));
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
        }

        public override void Delete(object id)
        {
            Subtheme subtheme = new Subtheme { SubthemeID = Convert.ToInt32(id.ToString()) };
            Delete(subtheme);
        }
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
