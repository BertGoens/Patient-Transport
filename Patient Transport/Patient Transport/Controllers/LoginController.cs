using System;
using System.DirectoryServices.AccountManagement;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Patient_Transport.Models.Application;
using Patient_Transport.Models.ViewModel;

namespace Patient_Transport.Controllers {
    public class AccountController : BaseController {
        private PT _pt = PT.Instance;

        //
        // GET: /Login/
        [AllowAnonymous]
        public ActionResult Index() {
            return View("LDAP");
        }

        //
        // POST: /Login/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel viewModel) {
            if (Membership.ValidateUser(viewModel.UserName, viewModel.Password)) {
                //LDAP USER
                PatientTransportPrincipal thisUser = _pt.Account.BuildAccount(viewModel);

                if (thisUser.Roles.Length < 1) {
                    //Incorrect user, no rights to use our app
                    FormsAuthentication.SignOut();
                    ViewBag.ErrorMessage = "Geen rechtten om de applicatie te gebruiken";
                    return View("LDAP");

                } else {
                    //Make a cookie
                    var serializeModel = new PatientTransportPrincipalSerializeModel();
                    serializeModel.Guid = (Guid)thisUser.Guid;
                    serializeModel.EmployeeId = thisUser.EmployeeId;
                    serializeModel.FirstName = thisUser.FirstName;
                    serializeModel.LastName = thisUser.LastName;

                    var serializer = new JavaScriptSerializer();
                    string userData = serializer.Serialize(serializeModel);
                    var authTicket = new FormsAuthenticationTicket(
                             1,
                             viewModel.UserName,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(15),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                    //Send to user home link
                    return RedirectToAction("Index", "Home");
                }
            } //User not authenticated

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: Login/Logout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
