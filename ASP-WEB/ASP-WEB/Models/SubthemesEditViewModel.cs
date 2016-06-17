using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    /// <summary>
    /// ViewModel to edit the subthemes
    /// </summary>
    public class SubthemesEditViewModel
    {
        /// <summary>
        /// List of al themes
        /// </summary>
        public List<Theme> themes { get; set; }
        /// <summary>
        /// Subtheme to edit
        /// </summary>
        public Subtheme subtheme { get; set; }
        /// <summary>
        /// List of all offices
        /// </summary>
        public List<Office> offices { get; set; }
    }
}
