using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    public class Faq
    {
        public int FaqID { get; set; }

        public int ThemeID { get; set; }
        public Theme Theme { get; set; }

        public int SubthemeID { get; set; }
        public Subtheme Subtheme { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
