using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    /// <summary>
    /// FAQ class
    /// </summary>
    public class Faq
    {
        /// <summary>
        /// ID Property
        /// </summary>
        public int FaqID { get; set; }

        /// <summary>
        /// ThemeID Property
        /// </summary>
        public int ThemeID { get; set; }
        /// <summary>
        /// Theme Navigation Property
        /// </summary>
        public Theme Theme { get; set; }

        /// <summary>
        /// SubthemeID Property
        /// </summary>
        public int SubthemeID { get; set; }
        /// <summary>
        /// Subtheme Navigation Property
        /// </summary>
        public Subtheme Subtheme { get; set; }
        /// <summary>
        /// Question Property
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// Answer Property
        /// </summary>
        public string Answer { get; set; }
    }
}
