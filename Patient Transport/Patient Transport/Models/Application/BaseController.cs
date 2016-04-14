using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Patient_Transport.Models.Application {
    public class BaseController : Controller {
        protected virtual new PatientTransportPrincipal User {
            get { return HttpContext.User as PatientTransportPrincipal; }
        }
    }
}