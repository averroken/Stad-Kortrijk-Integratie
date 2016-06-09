using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASP_WEB.Controllers
{
    public class OfficesController : ApiController
    {
        GenericRepository<Subtheme> repoSubtheme = new GenericRepository<Subtheme>();
        GenericRepository<Office> repoOffice = new GenericRepository<Office>();

        public IHttpActionResult GetOfficesBySubthemes(int? id)
        {
            if (!id.HasValue)
            {
                return Ok(repoOffice.All());
            }
            var offices = repoSubtheme.GetByID(id).Office;

            return Ok(offices);
        }
    }
}
