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

            //ExternalRepository ins = new ExternalRepository(ConfigurationManager.ConnectionStrings["Data"].ConnectionString);

            //MethodResult res = ins.InsertNewRow(DateTime.Now, "123", new Random(DateTime.Now.Second).NextDouble() * 100, new Random(DateTime.Now.Second).NextDouble() * 10, new Random(DateTime.Now.Second).NextDouble() * 10, new Random(DateTime.Now.Second).NextDouble() * 10, new Random(DateTime.Now.Second).NextDouble() * 200, new Random(DateTime.Now.Second).NextDouble() * 200, new Random(DateTime.Now.Second).NextDouble() * 200, new Random(DateTime.Now.Second).NextDouble() * 100, new Random(DateTime.Now.Second).NextDouble() * 1100,"",null,0);
            
            return View(obj);
        }

        

        public JsonResult GetDataBySmallPeriod(string identity)
        {
            DateTime start=DateTime.Now;
            DateTime end = DateTime.Now;

            double interval; 
            

            EWdata jsonData = new EWdata();
            
            jsonData.EndDate = end;
                       
            jsonData.StartDate = start;
            try
            {
                if (!Double.TryParse(ConfigurationManager.AppSettings["dataInterval"], NumberStyles.Any, CultureInfo.GetCultureInfo("en-US").NumberFormat, out interval))
                {
                    interval = 4; // по умолчанию 4 часов
                }                
                start = end.AddHours(-interval);
                jsonData.StartDate = start;
            
            IEnumerable<PumpParameters> data = repo_.GetPumpParamsByIdentityAndDate(identity, start, end);
            
            IEnumerable<PumpParameters> temp=data.OrderByDescending(x => x.RecvDate);
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
