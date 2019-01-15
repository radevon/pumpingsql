using DataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PumpVisy
{
    public static class Utilite
    {
        public static LogMessage CreateDefaultLogMessage(string UserName, string MessageType, Exception ex)
        {
            LogMessage message = new LogMessage()
            {
                MessageDate = DateTime.Now,
                UserName = UserName,
                MessageType = MessageType,
                MessageText = ex.Message+" "+ex.StackTrace
            };
            return message;
        }
    }
}