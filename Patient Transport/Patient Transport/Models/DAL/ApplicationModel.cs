using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Patient_Transport.Models.DAL {
    public class ApplicationModel : DbContext {

        public ApplicationModel() : base("SQLConnectionString") {

        }

        public DbSet<ExceptionLogger> ExceptionLoggers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}