using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DataRepository;
using System.Configuration;


namespace PumpVisy.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        
        // GET: /Device/

        private VisualSqlRepository repo;
        private Logger loger;

        public DeviceController()
        {
            repo = new VisualSqlRepository(ConfigurationManager.ConnectionStrings["Data"].ConnectionString);
            loger = new Logger(ConfigurationManager.AppSettings["logFilePath"]);
        }

        public JsonResult AllMarkers()
        {
            List<Db_objects> devices = new List<Db_objects>();
            try
            {
                devices = repo.GetAllObjects().ToList();
                
            }
            catch (Exception ex)
            {
                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex.Message + " " + ex.StackTrace));                
            }
            
            return Json(devices, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertNewMarker(Db_objects marker)
        {
            int insertedId;
            try
            {
                insertedId = repo.InsertObject(marker);
                // логирование
                if (insertedId > 0)
                {
                    loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "insert", new JavaScriptSerializer().Serialize(marker)));
                }
            }
            catch (Exception ex)
            {
                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex.Message + " " + ex.StackTrace));
                insertedId = -1;
            }
            
            return Json(insertedId);
        }


        public ActionResult GetMarker(int ObjectId)
        {
            try
            {
                Db_objects marker = repo.GetObjectById(ObjectId);
                if(marker!=null)
                    return Json(marker, JsonRequestBehavior.AllowGet);
                else
                    return HttpNotFound();
                
            }
            catch (Exception ex)
            {
                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex.Message + " " + ex.StackTrace));
                return HttpNotFound();
            }
            
        }

        [HttpPost]
        public JsonResult UpdateMarker(Db_objects marker)
        {
            int updateCount;
            try
            {
                Db_objects old = repo.GetObjectById(marker.ObjectId);
                updateCount = repo.UpdateObject(marker);
                if (updateCount > 0)
                {
                    loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "update", "old values: " + new JavaScriptSerializer().Serialize(old) + "; new values: " + new JavaScriptSerializer().Serialize(marker)));
                    
                }
            }
            catch (Exception ex)
            {
                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex.Message + " " + ex.StackTrace));
                updateCount = -1;
            }
            return Json(updateCount);
        }

        [HttpPost]
        public JsonResult DeleteMarker(int ObjectId)
        {
            int deleteCount;
            
            try
            {
                Db_objects deleted = repo.GetObjectById(ObjectId);
                deleteCount = repo.DeleteObjectById(ObjectId);
                if (deleteCount > 0)
                {
                    loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "delete", new JavaScriptSerializer().Serialize(deleted)));
                    
                }
                
            }
            catch (Exception ex)
            {
                loger.LogToFile(Utilite.CreateDefaultLogMessage(User.Identity.Name, "error", ex.Message + " " + ex.StackTrace));
                deleteCount = -1;
            }
            return Json(deleteCount);
        }
    }
}
