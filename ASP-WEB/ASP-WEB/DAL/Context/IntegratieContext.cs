using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.DAL.Context
{
    public class IntegratieContext : DbContext
    {
        public DbSet<Theme> Theme { get; set; }
        public DbSet<Subtheme> Subtheme { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Faq> Faq { get; set; }
        

    }
}
