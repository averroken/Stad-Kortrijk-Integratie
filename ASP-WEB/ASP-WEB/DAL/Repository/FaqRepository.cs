using ASP_WEB.DAL.Context;
using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ASP_WEB.DAL.Repository
{
    /// <summary>
    /// Repository for FAQs
    /// </summary>
    public class FaqRepository : GenericRepository<Faq>, IGenericRepository<Faq>
    {
        /// <summary>
        /// Gets all FAQs
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Faq> All()
        {
            return context.Faq.Include(f => f.Theme).Include(f => f.Subtheme).ToList<Faq>();
        }
        /// <summary>
        /// Gets all FAQs of a theme
        /// </summary>
        /// <param name="id">ThemeID</param>
        /// <returns></returns>
        public List<Faq> GetFaqByThemeID(int id)
        {

            return context.Faq.Where(f => f.ThemeID == id).Include(f => f.Theme).Include(f => f.Subtheme).ToList<Faq>();

        }
        /// <summary>
        /// Gets all FAQs of a subtheme
        /// </summary>
        /// <param name="id">SubthemeID</param>
        /// <returns></returns>
        public List<Faq> GetFaqBySubthemeID(int id)
        {

            return context.Faq.Where(f => f.SubthemeID == id).Include(f => f.Theme).Include(f => f.Subtheme).ToList<Faq>();

        }
        /// <summary>
        /// Gets a FAQ by ID
        /// </summary>
        /// <param name="id">FaqID</param>
        /// <returns></returns>
        public override Faq GetByID(object id)
        {

            int ID = Convert.ToInt32(id.ToString());
            return context.Faq.Where(f => f.FaqID == ID).Include(f => f.Theme).Include(f => f.Subtheme).SingleOrDefault<Faq>();

        }
    }
}
