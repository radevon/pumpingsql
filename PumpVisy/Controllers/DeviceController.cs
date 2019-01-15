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

        private VisualSqlRepository repo = new VisualSqlRepository(ConfigurationManager.ConnectionStrings["Data"].ConnectionString);
        private Logger loger = new Logger(ConfigurationManager.AppSettings["logFilePath"]);

        public JsonResult AllMarkers()
        {
            List<Db_objects> devices = new List<Db_objects>();
            try
            {
                devices = repo.GetAllObjects().ToList();

                
            }
            catch (Exception ex)
            {
                LogMessage message = new LogMessage()
                {
                    MessageDate = DateTime.Now,
                    UserName = User.Identity.Name,
                    MessageType = "error",
                    MessageText = ex.Message + ex.StackTrace
                };
                loger.LogToFile(message);
                
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
                    //LogMessage message = new LogMessage()
                    //{
                    //    Id = 1,
                    //    UserName = User.Identity.Name,
                    //    MessageDate = DateTime.Now,
                    //    MessageType = "insert",
                    //    MessageText = new JavaScriptSerializer().Serialize(marker)
                    //};

                    //loger.LogToFile(message);
                    //loger.LogToDatabase(message);
                }
            }
            catch (Exception ex)
            {
                //LogMessage message = new LogMessage()
                //{
                //    Id = -1,
                //    UserName = User.Identity.Name,
                //    MessageDate = DateTime.Now,
                //    MessageType = "error",
                //    MessageText = ex.Message + ex.StackTrace
                //};

                //loger.LogToFile(message);
                //loger.LogToDatabase(message);
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
                //LogMessage message = new LogMessage()
                //{
                //    Id = -1,
                //    MessageDate = DateTime.Now,
                //    UserName = User.Identity.Name,
                //    MessageType = "error",
                //    MessageText = ex.Message + ex.StackTrace
                //};

                //loger.LogToFile(message);
                //loger.LogToDatabase(message);
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
                    //LogMessage message = new LogMessage()
                    //{
                    //    Id = 1,
                    //    UserName = User.Identity.Name,
                    //    MessageDate = DateTime.Now,
                    //    MessageType = "update",
                    //    MessageText = "old values: " + new JavaScriptSerializer().Serialize(old) + "; new values: " + new JavaScriptSerializer().Serialize(marker)
                    //};

                    //loger.LogToFile(message);
                    //loger.LogToDatabase(message);
                }
            }
            catch (Exception ex)
            {
                //LogMessage message = new LogMessage()
                //{
                //    Id = -1,
                //    MessageDate = DateTime.Now,
                //    UserName = User.Identity.Name,
                //    MessageType = "error",
                //    MessageText = ex.Message + ex.StackTrace
                //};

                //loger.LogToFile(message);
                //loger.LogToDatabase(message);
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
                    //LogMessage message = new LogMessage()
                    //{
                    //    Id = 1,
                    //    UserName = User.Identity.Name,
                    //    MessageDate = DateTime.Now,
                    //    MessageType = "delete",
                    //    MessageText = new JavaScriptSerializer().Serialize(deleted)
                    //};
                    //loger.LogToFile(message);
                    //loger.LogToDatabase(message);
                }
                
            }
            catch (Exception ex)
            {
                //LogMessage message = new LogMessage()
                //{
                //    Id = -1,
                //    MessageDate = DateTime.Now,
                //    UserName = User.Identity.Name,
                //    MessageType = "error",
                //    MessageText = ex.Message + ex.StackTrace
                //};

                //loger.LogToFile(message);
                //loger.LogToDatabase(message);
                deleteCount = -1;
            }
            return Json(deleteCount);
        }
    }
}
