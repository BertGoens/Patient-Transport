using System.Web.Mvc;

namespace Patient_Transport.Models.Application {
    public abstract class BaseViewPage : WebViewPage {
        public virtual new PatientTransportPrincipal User {
            get { return base.User as PatientTransportPrincipal; }
        }
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel> {
        public virtual new PatientTransportPrincipal User {
            get { return base.User as PatientTransportPrincipal; }
        }
    }
}