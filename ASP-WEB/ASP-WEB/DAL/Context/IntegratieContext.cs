using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.DAL.Context
{
    /// <summary>
    /// Context used in this project
    /// </summary>
    public class IntegratieContext : DbContext
    {
        /// <summary>
        /// Theme DbSet
        /// </summary>
        public DbSet<Theme> Theme { get; set; }
        /// <summary>
        /// Subtheme DbSet
        /// </summary>
        public DbSet<Subtheme> Subtheme { get; set; }
        /// <summary>
        /// Office DbSet
        /// </summary>
        public DbSet<Office> Office { get; set; }
        /// <summary>
        /// Faq DbSet
        /// </summary>
        public DbSet<Faq> Faq { get; set; }
        

    }
}
