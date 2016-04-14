using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patient_Transport.Models.Application {
    /// <summary>
    /// Used to access and store server related information
    /// </summary>
    public sealed class PT {

        #region singleton

        private static PT instance = null;
        private static readonly object padlock = new object();

        PT() {
            Account = new Account();
            StartupDate = DateTime.Now;
        }

        public static PT Instance {
            get {
                lock (padlock) {
                    if (instance == null) {
                        instance = new PT();
                    }
                    return instance;
                }
            }
        }

        #endregion

        /// <summary>
        /// Get the local startup date of when the server went online
        /// </summary>
        public DateTime StartupDate { get; private set; }

        /// <summary>
        /// Everything interfering with the AD goes trough this
        /// </summary>
        public Account Account { get; private set; }
    }
}