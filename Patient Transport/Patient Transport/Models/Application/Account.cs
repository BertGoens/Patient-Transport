using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Patient_Transport.Models.ViewModel;

namespace Patient_Transport.Models.Application {
    // Uses References 
    // System.DirectoryServices
    // System.DirectoryServices.AccountManagement
    // System.Configuration

    public sealed class Account {

        private string _domainName = WebConfigurationManager.AppSettings[Constants.WebConfig.AD_DOMAIN_NAME];
        private string _container = WebConfigurationManager.AppSettings[Constants.WebConfig.AD_CONTAINER];
        private string _qryUsername = WebConfigurationManager.AppSettings[Constants.WebConfig.QUERY_USERNAME];
        private string _qryPassword = WebConfigurationManager.AppSettings[Constants.WebConfig.QUERY_PASSWORD];

        private PT_Group[] Groups { get; set;}
        private PT_PermissionLinks[] UserLinks { get; set; }

        public Account() {
            loadGroups();
            populateGroups();
            loadLinks();
        }

        private void loadGroups() {
            //Load groups from Web.Config
            Groups = new PT_Group[] {
                new PT_Group(WebConfigurationManager.AppSettings[Constants.WebConfig.GRP_DOKTER], UserType.Dokter_Invoer),
                new PT_Group(WebConfigurationManager.AppSettings[Constants.WebConfig.GRP_VERDIEP], UserType.Verdiep),
                new PT_Group(WebConfigurationManager.AppSettings[Constants.WebConfig.GRP_DISPATCH], UserType.Dispatch),
                new PT_Group(WebConfigurationManager.AppSettings[Constants.WebConfig.GRP_VERVOER], UserType.Vervoer),
                new PT_Group(WebConfigurationManager.AppSettings[Constants.WebConfig.GRP_ADMIN], UserType.Administrator)
            };
        }

        private void populateGroups() {
            //Create GroupPrincipal objects for the groups if possible
            using (var context = new PrincipalContext(ContextType.Domain, _domainName, _container)) {
                foreach (var grp in Groups) {
                    try {
                        using (var qryGroup = GroupPrincipal.FindByIdentity(context, IdentityType.Name, grp.GroupName)) {
                            grp.GroupPrincipal = qryGroup;
                        }
                    } catch (PrincipalOperationException ex) {
                        //A group is not found
                        System.Diagnostics.Debug.WriteLine("A group is not found: " + grp.GroupName);
                        ex.ToString();
                        //System.Diagnostics.Debug.WriteLine(ex);
                    }                    
                }
            }
        }

        private void loadLinks() {
            
            List<PT_PermissionLinks> links = new List<PT_PermissionLinks>();

            links.Add(populateLink(UserType.Dokter_Invoer, WebConfigurationManager.AppSettings[Constants.WebConfig.PERM_DOKTER]));
            links.Add(populateLink(UserType.Verdiep, WebConfigurationManager.AppSettings[Constants.WebConfig.PERM_VERDIEP]));
            links.Add(populateLink(UserType.Dispatch, WebConfigurationManager.AppSettings[Constants.WebConfig.PERM_DISPATCH]));
            links.Add(populateLink(UserType.Vervoer, WebConfigurationManager.AppSettings[Constants.WebConfig.PERM_VERVOER]));
            links.Add(populateLink(UserType.Administrator, WebConfigurationManager.AppSettings[Constants.WebConfig.PERM_ADMIN]));

            UserLinks = links.ToArray();
            }

        private PT_PermissionLinks populateLink(UserType t, string webconfigKey) {
            string value = WebConfigurationManager.AppSettings[webconfigKey];
            string[] extractedLinks = null;
            if (!string.IsNullOrEmpty(value)) {
                extractedLinks = value.Split(Constants.WebConfig.PERM_DELIMITER);
            }

            return new PT_PermissionLinks(extractedLinks, t);
        }

        public PatientTransportPrincipal BuildAccount(LoginModel viewModel) {
            //User already exists, check if he has rights to our application

            // Get UserPrincipal Object from user

            //1. Check what groups the user is a member of
            UserPrincipal thisUser = null;

            using (var context = new PrincipalContext(ContextType.Domain)) {
                thisUser = UserPrincipal.FindByIdentity(context, User.Identity.Name);
                UserPrincipal.FindByIdentity(context, IdentityType.Guid, "GUID");
                //thisUser.EmailAddress;
                //User.Identity.
            }
            
            var userPermissions = new List<UserType>();
            for (int i = 0; i < Groups.Length; i++) {
                if (thisUser.IsMemberOf(Groups[i].GroupPrincipal)) {
                    userPermissions.Add(Groups[i].UserType);
                }
            }

            //2. Get links
            // !TOOD! Save as html?
            //3. Save links
            
        }

    }

    internal enum UserType {
        Dokter_Invoer,
        Verdiep,
        Dispatch,
        Vervoer,
        Administrator
    }

    internal class PT_Group {
        public string GroupName { get; private set; }
        public UserType UserType { get; private set; }
        public GroupPrincipal GroupPrincipal { get; set; }

        public PT_Group(string groupName, UserType userType) {
            this.GroupName = groupName;
            this.UserType = userType;
        }

    }

    internal class PT_PermissionLinks {
        public UserType UserType { get; private set; }
        public string[] Links { get; private set; }

        public PT_PermissionLinks(string[] links, UserType ut) {
            this.Links = links;
            this.UserType = ut;
        }
    }

    public interface IPatientTransportPrincipal : IPrincipal {
        Guid Guid { get; set; }
        string EmployeeId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        UserType[] Roles { get; set; }
    }

    public class PatientTransportPrincipal : IPatientTransportPrincipal {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public PatientTransportPrincipal(string name) {
            this.Identity = new GenericIdentity(name);
        }

        public Guid Guid { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType[] Roles { get; set; }

    }

    public class PatientTransportPrincipalSerializeModel {
        public Guid Guid { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType[] Roles { get; set; }

        public PatientTransportPrincipalSerializeModel() { }
    }

}