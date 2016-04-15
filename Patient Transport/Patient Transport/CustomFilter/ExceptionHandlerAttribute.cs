using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Patient_Transport.Models.DAL;

namespace Patient_Transport.CustomFilter {
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter {
        public void OnException(ExceptionContext filterContext) {
            if (!filterContext.ExceptionHandled) {


                ExceptionLogger logger = new ExceptionLogger() {
                    ExceptionMessage = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                    LogTime = DateTime.Now
                };

                ApplicationModel ctx = new ApplicationModel();
                ctx.ExceptionLoggers.Add(logger);
                ctx.SaveChanges();

                filterContext.ExceptionHandled = true;
            }
        }
    }
}