using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    /// <summary>
    /// ViewModel to create, edit and view the FAQS
    /// </summary>
    public class FaqSubtheme
    {
        /// <summary>
        /// List of all FAQs
        /// </summary>
        public List<Faq> Faq { get; set; }
        /// <summary>
        /// List of all subthemes
        /// </summary>
        public List<Subtheme> Subtheme { get; set; }
        /// <summary>
        /// List of all themes
        /// </summary>
        public List<Theme> Theme { get; set; }
        /// <summary>
        /// FAQ Property
        /// </summary>
        public Faq faqEen { get; set; }
    }
}
