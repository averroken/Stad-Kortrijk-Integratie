using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP_WEB.Models
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public bool IsAdmin { get; set; }
    }
}