using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    /// <summary>
    /// Subtheme Class
    /// </summary>
    public class Subtheme
    {
        /// <summary>
        /// ID Property
        /// </summary>
        public int SubthemeID { get; set; }
        /// <summary>
        /// Name Property
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description Property
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Office Collection Navigation Property
        /// </summary>
        public virtual ICollection<Office> Office { get; set; }
        /// <summary>
        /// OfficeID Collection Property
        /// </summary>
        public ICollection<int> OfficeID { get; set; }

        /// <summary>
        /// Theme Navigation Property
        /// </summary>
        public virtual Theme Theme { get; set; }
        /// <summary>
        /// ThemeID property
        /// </summary>
        public int ThemeID { get; set; }

        /// <summary>
        /// Name of the photo Property
        /// </summary>
        public string FotoURL { get; set; }
    }
}
