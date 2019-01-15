using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataRepository;
using System.Reflection;
using System.Globalization;

namespace PumpVisy.Controllers
{
    [Authorize]
    public class DetailsController : Controller
    {
        //
        // GET: /Details/

        private VisualSqlRepository repo_;
        private Logger loger;

        public DetailsController()
        {
            repo_ = new VisualSqlRepository(ConfigurationManager.ConnectionStrings["Data"].ConnectionString);
            loger = new Logger(ConfigurationManager.AppSettings["logFilePath"]);
        }

        
        public ActionResult ViewParameters(int id)
        {
            Db_objects obj = repo_.GetObjectById(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            
            return View(obj);
        }

        

        public JsonResult GetDataBySmallPeriod(string identity, string parameterGraph)
        {
            DateTime end = DateTime.Now;

            double interval = 1; // 1 час для построения графика
            int interval_table = 30;  // 30 мин для данных

            EWdata jsonData = new EWdata();
            
            jsonData.EndDate = end;

            DateTime start = end.AddHours(-interval);
            jsonData.StartDate = start;
            try
            {
                interval = Convert.ToDouble(ConfigurationManager.AppSettings["DataVisualInterval"], CultureInfo.GetCultureInfo("en-US").NumberFormat);
                interval_table = Convert.ToInt32(ConfigurationManager.AppSettings["DataTableInterval"], CultureInfo.GetCultureInfo("en-US").NumberFormat);

                start = end.AddHours(-interval);
                jsonData.StartDate = start;
            
            IEnumerable<PumpParameters> data = repo_.GetPumpParamsByIdentityAndDate(identity, start, end);
            
            IEnumerable<PumpParameters> temp=data.OrderByDescending(x => x.RecvDate);
            jsonData.DataTable = temp.Where(x=>x.RecvDate>end.AddMinutes(-interval_table)).ToList();
            PropertyInfo infoprop = (typeof(PumpParameters)).GetProperty(parameterGraph);

            jsonData.DataGraph = temp.Select(x => new DataForVisual() { RecvDate = x.RecvDate, Value = infoprop.GetValue(x) == null ? 0 : (double)infoprop.GetValue(x) }).ToList();

            }
            catch (Exception ex)
            {

                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex));
                               
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

    }
}
