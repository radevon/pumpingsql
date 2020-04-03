using System.Globalization;
using System.Reflection;
using DataRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PumpVisy.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {

        private VisualSqlRepository repo_;
        private Logger loger;
        //
        // GET: /History/

        public HistoryController()
        {
            repo_ = new VisualSqlRepository(ConfigurationManager.ConnectionStrings["Data"].ConnectionString);
            loger = new Logger(ConfigurationManager.AppSettings["logFilePath"]);
        }

        public ActionResult History(int id)
        {
            Db_objects obj = repo_.GetObjectById(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        public JsonResult GetByPeriod(string identity, DateTime start_)
        {
            DateTime start = new DateTime(start_.Year, start_.Month, start_.Day,0,0,0);
            DateTime end = new DateTime(start_.Year, start_.Month, start_.Day, 23, 59, 59);
                        
            EWdata jsonData = new EWdata();
            jsonData.StartDate = start;
            jsonData.EndDate = end;

            try
            {


                IEnumerable<PumpParameters> data = repo_.GetPumpParamsByIdentityAndDate(identity, start, end);
                IEnumerable<PumpParameters> temp = data.OrderBy(x => x.RecvDate);
                jsonData.DataTable = temp.ToList();
               
            }
            catch (Exception ex)
            {
                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex.Message + " " + ex.StackTrace));
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

       
    }
}
