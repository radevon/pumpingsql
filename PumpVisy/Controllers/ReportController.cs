using DataRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PumpVisy.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {

        private VisualSqlRepository repo_;
        private Logger loger;

        public ReportController()
        {
            repo_ = new VisualSqlRepository(ConfigurationManager.ConnectionStrings["Data"].ConnectionString);
            loger = new Logger(ConfigurationManager.AppSettings["logFilePath"]);
            CultureInfo cult=CultureInfo.CreateSpecificCulture("ru-RU");
            cult.NumberFormat.NumberDecimalSeparator=".";
            Thread.CurrentThread.CurrentCulture = cult;
        }
        //
        // GET: /Report/

        

        public ActionResult Index()
        {
            List<Db_objects> objects = repo_.GetAllObjects().OrderBy(x => x.Address).ToList();
            ReportSource rc = new ReportSource();
            rc.Adresses = objects.Select(x => new SelectListItem() { Text=x.Address,Value=x.Identity}).ToList();

            return View("ReportIndex",rc);
        }

        
        [HttpPost]
        public ActionResult GenerateReport(ReportSource rc, String ReportType )
        {
            if (!ModelState.IsValid)
            {
                return View("ReportIndex", rc);
            }
            List<ByHourStat> values = new List<ByHourStat>();

            Db_objects m = repo_.GetObjectByIdentity(rc.Identity);
            if (m != null)
            {
                ViewBag.ObjectName = m.Address;
            }
            else
            {
                ViewBag.ObjectName = "-";
            }

            if (ReportType.IndexOf("Месячный", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                values = repo_.GetStatByDays(rc.DateParam, rc.Identity).ToList();
                return View("StatByMonth", values);
            }else
            {
                values = repo_.GetStatByHour(rc.DateParam, rc.Identity).ToList();
                return View("StatByDay", values);
            }

            
        }
         

    }
}
