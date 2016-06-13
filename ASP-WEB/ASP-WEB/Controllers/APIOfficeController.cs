using ASP_WEB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace ASP_WEB.Controllers
{
    public class OfficesController : ApiController
    {
        GenericRepository<Subtheme> repoSubtheme = new GenericRepository<Subtheme>();
        GenericRepository<Office> repoOffice = new GenericRepository<Office>();

        [HttpGet]
        public HttpResponseMessage Get(int? id)
        {
          
            if (!id.HasValue)
            {
                var allOffices = repoOffice.All().ToList();
                JsonConvert.SerializeObject(allOffices);
                HttpResponseMessage responseAll = Request.CreateResponse(HttpStatusCode.OK, allOffices);
                return responseAll;
                //return repoOffice.All().ToList();
            }
            var offices = repoSubtheme.GetByID(id).Office.ToList();
            JsonConvert.SerializeObject(offices);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, offices);
            return response;
        }
    }
}
