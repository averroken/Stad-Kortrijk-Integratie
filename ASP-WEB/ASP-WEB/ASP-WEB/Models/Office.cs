using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    public class Office
    {
        public int OfficeID { get; set; }
        public string Name { get; set; }
      //  [Url]
        public string URL { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        //[Phone]
        public string PhoneNumber { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
       // [EmailAddress]
        public string EmailAddress { get; set; }
        [JsonIgnore]
        public virtual ICollection<Subtheme> Subtheme { get; set; }
    }
}
