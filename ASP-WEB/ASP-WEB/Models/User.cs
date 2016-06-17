using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    /// <summary>
    /// (View)Model used to manage the users
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID property
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// UserName Property
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Email Property
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// IsAdmin Property
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}
