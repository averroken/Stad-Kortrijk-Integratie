using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    public class SubthemesEditViewModel
    {
        public List<Theme> themes { get; set; }
        public Subtheme subtheme { get; set; }
        public List<Office> offices { get; set; }
    }
}
