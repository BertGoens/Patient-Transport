
namespace Patient_Transport.Models {
    public abstract class Constants {

        /// <summary>
        /// Kijk naar de Web.config om hun waardes te zien.
        /// </summary>
        public abstract class WebConfig {
            public static string AD_DOMAIN_NAME = "AD_Domain_Name";
            public static string AD_CONTAINER = "AD_Container";

            public static string QUERY_USERNAME = "Query_Username";
            public static string QUERY_PASSWORD = "Query_Password";

            //Groups
            public static string GRP_DOKTER = "GRP_Dokter_Invoer";
            public static string GRP_VERDIEP = "GRP_Verdiep";
            public static string GRP_DISPATCH = "GRP_Dispatch";
            public static string GRP_VERVOER = "GRP_Vervoer";
            public static string GRP_ADMIN = "GRP_Administrator";

            //Permissions / Links
            public static char PERM_DELIMITER = ';';
            public static string PERM_DOKTER = "PERM_Dokter_Invoer";
            public static string PERM_VERDIEP = "PERM_Verdiep";
            public static string PERM_DISPATCH = "PERM_Dispatch";
            public static string PERM_VERVOER = "PERM_Vervoer";
            public static string PERM_ADMIN = "PERM_Administrator";
        }

        public static string SESSION_LINKS = "User_Links";

        public abstract class Database {
        }

        public abstract class Request {
            public static string DOKTER_CONSULT = "Dokter_Consult";
        }

        public abstract class Controllers {
            public abstract class Account {
                public static string GET_Index = "Index";
                public static string POST_Login = "Login";
                public static string POST_Logout = "Logout";
            }

            public abstract class Home {
                public static string GET_INDEX = "Index";
            }
        }
    }
}