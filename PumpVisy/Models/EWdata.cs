using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataRepository;

namespace PumpVisy
{
    public class EWdata
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<PumpParameters> DataTable { get; set; }

       
        public int TimeDiff
        {
             get
            {
                return (int)Math.Floor((EndDate - StartDate).TotalHours);
            }
        }

        public EWdata()
        {
            DataTable = new List<PumpParameters>();
        }
    }

}