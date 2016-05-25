using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    public class Subtheme
    {
        public int SubthemeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Office> Office { get; set; }
        public ICollection<int> OfficeID { get; set; }

        public virtual Theme Theme { get; set; }
        public int ThemeID { get; set; }

        public string FotoURL { get; set; }
    }
}
