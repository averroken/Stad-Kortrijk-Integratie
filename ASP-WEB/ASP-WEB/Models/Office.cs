using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_WEB.Models
{
    /// <summary>
    /// Office Class
    /// </summary>
    public class Office
    {
        /// <summary>
        /// ID Property
        /// </summary>
        public int OfficeID { get; set; }
        /// <summary>
        /// Name Property
        /// </summary>
        public string Name { get; set; }
        //[Url]
        /// <summary>
        /// URL Property
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Street Property
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// HouseNumber Property
        /// </summary>
        public string HouseNumber { get; set; }
        /// <summary>
        /// ZipCode Property
        /// </summary>
        public int ZipCode { get; set; }
        /// <summary>
        /// City Property
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// PhoneNumber Property
        /// </summary>
        public string PhoneNumber { get; set; }
        //public string OpeningHours { get; set; }
        //[EmailAddress]
        /// <summary>
        /// EmailAddress Property
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Subtheme Navigation Property
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Subtheme> Subtheme { get; set; }
        /// <summary>
        /// Monday Opening Hours Property
        /// </summary>
        public string Monday { get; set; }
        /// <summary>
        /// Tuesday Opening Hours Property
        /// </summary>
        public string Tuesday { get; set; }
        /// <summary>
        /// Wednseday Opening Hours Property
        /// </summary>
        public string Wednesday { get; set; }
        /// <summary>
        /// Thursday Opening Hours Property
        /// </summary>
        public string Thursday { get; set; }
        /// <summary>
        /// Friday Opening Hours Property
        /// </summary>
        public string Friday { get; set; }
        /// <summary>
        /// Saturday Opening Hours Property
        /// </summary>
        public string Saturday { get; set; }
        /// <summary>
        /// Sunday Opening Hours Property
        /// </summary>
        public string Sunday { get; set; }

    }
}
