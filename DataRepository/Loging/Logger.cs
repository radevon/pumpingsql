using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Policy;
using System.Text;
using System.Web;


namespace DataRepository
{
    public class Logger
    {
        // запись лога в файл
        private string FilePath { get; set; }
        //private IDbConnection Connection { get; set; }

        public Logger(string FilePath)
        {
            this.FilePath = FilePath;
            //this.Connection = connection;
        }

        public bool LogToFile(LogMessage message)
        {
            
            try
            {
                    using (StreamWriter sw = (File.Exists(FilePath)) ? File.AppendText(FilePath) : File.CreateText(FilePath))
                    {
                        sw.WriteLine(message.ToString());
                    }
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*
        public bool LogToDatabase(LogMessage message, IDbConnection connection)
        {
            int result;
            try
            {

                connection.Open();
                using(IDbCommand cmd=connection.CreateCommand()){
                    cmd.CommandText="insert into ";
                    cmd.CommandType=CommandType.Text;
                    cmd.Parameters.Add();
                    result=cmd.

                }
                connection.Close();
                
            return result>0;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
         */

    }
}