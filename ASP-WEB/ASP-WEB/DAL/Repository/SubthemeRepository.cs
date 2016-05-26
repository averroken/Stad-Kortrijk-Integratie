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
            using (IntegratieContext context= new IntegratieContext())
            {
                return context.Subtheme.Where(st => st.ThemeID == themeID).ToList<Subtheme>();
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

    }
}
