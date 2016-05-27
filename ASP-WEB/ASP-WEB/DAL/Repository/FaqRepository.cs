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
    public class FaqRepository : GenericRepository<Faq>, IGenericRepository<Faq>
    {
        //Remember eager loading
        public override IEnumerable<Faq> All()
        {
            return context.Faq.Include(f => f.Theme).Include(f => f.Subtheme).ToList<Faq>();
        }
        public List<Faq> Search(string searchString)
        {
            using (IntegratieContext context = new IntegratieContext())
            {
                return context.Faq.Where(f => f.Answer.Contains(searchString) || f.Question.Contains(searchString)).Include(f => f.Theme).Include(f => f.Subtheme).ToList<Faq>();
            }
        }
    }
}
