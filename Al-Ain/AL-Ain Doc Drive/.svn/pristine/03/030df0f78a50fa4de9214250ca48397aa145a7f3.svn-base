using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Search_n_View.Models;

namespace Search_n_View.Helpers
{
    public class Logs
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static void ErrorLog(string className, string functionName, string ErrorMessage, string ErrorSource, string Errorstacktrace)
        {
            try
            {
                ErrorLog er = new ErrorLog();
                er.ClassName = className;
                er.FunctionName = functionName;
                er.Message = ErrorMessage;
                er.Source = ErrorSource;
                er.Stacktrace = Errorstacktrace;
                er.AddedBy = null;
                er.ModifiedBy = null;
                er.IsDeleted = false;
                er.AddedTime = DateTime.Now;
                er.ModifiedTime = DateTime.Now;
                db.ErrorLog.Add(er);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorLog("Logs", "ErrorLog", ex.Message, ex.Source, ex.StackTrace);
            }
        }
    }
}