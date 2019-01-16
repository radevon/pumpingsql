﻿using System.Globalization;
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


        public JsonResult GetByPeriod(string identity, DateTime start_, string parameterGraph = "Amperage1")
        {
            DateTime end=start_.AddHours(1);
            
            EWdata jsonData = new EWdata();
            jsonData.StartDate = start_;
            jsonData.EndDate = end;

            try
            {


                IEnumerable<PumpParameters> data = repo_.GetPumpParamsByIdentityAndDate(identity, start_, end);
                IEnumerable<PumpParameters> temp = data.OrderBy(x => x.RecvDate);
                jsonData.DataTable = temp.ToList();
                PropertyInfo infoprop = (typeof(PumpParameters)).GetProperty(parameterGraph);
                jsonData.DataGraph = temp.Select(x => new DataForVisual() { RecvDate = x.RecvDate, Value = (double?)infoprop.GetValue(x) }).ToList();
            }
            catch (Exception ex)
            {
                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex.Message + " " + ex.StackTrace));
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

       
    }
}
