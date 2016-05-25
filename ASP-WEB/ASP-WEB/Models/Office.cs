using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    public class Office
    {
        public int OfficeID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string OpeningHours { get; set; }
        public string EmailAddress { get; set; }
        public virtual ICollection<Subtheme> Subtheme { get; set; }
    }
}
