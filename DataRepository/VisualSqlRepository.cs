using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperBase;
using System.Data.SqlClient;

namespace DataRepository
{
    public class VisualSqlRepository:DapperBaseConnection<SqlConnection>
    {

        public VisualSqlRepository(string ConnectionString):base(ConnectionString)
        {
            
        }

        // объекты
        #region Table db_object_marker
        private string sql_objects_select = @"select ObjectId, address, [identity] from db_objects ";

        public IEnumerable<Db_objects> GetAllObjects()
        {
            IEnumerable<Db_objects> markers = Enumerable.Empty<Db_objects>();

            return this.GetListData<Db_objects>(sql_objects_select,null);
            
        }


        public Db_objects GetObjectById(int id)
        {
            Db_objects marker = null;

            marker = this.GetItemData<Db_objects>(sql_objects_select + " where objectId=@id_", new { id_ = id });
           
            return marker;
        }

        public Db_objects GetObjectByIdentity(string identity)
        {
            Db_objects marker = null;
            marker = this.GetItemData<Db_objects>(sql_objects_select + " where [identity]=@identity_", new { identity_ = identity });
            return marker;
        }


        public int InsertObject(Db_objects marker)
        {
            int res = this.GetItemData<int>("insert into db_objects (address, [identity]) values(@address,@identity); SELECT SCOPE_IDENTITY();", marker);
            
            return res;

        }

        public int UpdateObject(Db_objects marker)
        {
            int res=0;
            res=this.ExecuteSQL("update db_objects set address=@address, [identity]=@identity where objectId=@objectId", marker);
            return res;

        }

        public int DeleteObjectById(int id)
        {
            int res = 0;
            res= this.ExecuteSQL("delete from db_objects where objectId=@id", new { id = id });
            return res;
        }

       

        #endregion


        #region Table ElectricAndWaterParams
        private string selectEVSql = @"select Id, [Identity], TotalEnergy, Amperage1, Amperage2, Amperage3, Voltage1, Voltage2, Voltage3, CurrentElectricPower, TotalWaterRate, RecvDate, Errors, Alarm, Presure from PumpParameters ";

        // параметры по Id - 1 запись или null
        public PumpParameters GetPumpParamsById(int id)
        {
            PumpParameters parameter = null;           
            parameter = this.GetItemData<PumpParameters>(selectEVSql + " where Id=@id_", new { id_ = id });
            return parameter;
        }

        public IEnumerable<PumpParameters> GetPumpParamsByIdentityAndDate(string identity, DateTime from, DateTime to)
        {
            IEnumerable<PumpParameters> parameters = Enumerable.Empty<PumpParameters>();
            parameters = this.GetListData<PumpParameters>(selectEVSql + " where [Identity]=@identity_ and recvDate between @start and @end;", new { identity_ = identity, start = from, end = to });
            
            return parameters;
        }

        #endregion

        

        #region Statistic

        public IEnumerable<ByHourStat> GetStatByHour(DateTime day, string identity)
        {
            IEnumerable<ByHourStat> parameters = Enumerable.Empty<ByHourStat>();
            string sqlstring = @"WITH dates AS (
  select DATEADD(dd, DATEDIFF(dd,0,@day_), 0) as HourTime
  UNION ALL
  SELECT dateadd(hour,1,dates.HourTime)
  FROM dates
  WHERE dates.HourTime < dateadd(day,1,DATEADD(dd, DATEDIFF(dd,0,@day_), 0))
) select dates.HourTime, params.recvDate, params.TotalEnergy, params.TotalWaterRate  from dates
left join 
(select par.recvDate, par.TotalEnergy, par.TotalWaterRate from PumpParameters par where par.[identity]=@identity_) params on params.recvDate=(select min(e.recvDate) from PumpParameters e where e.[identity]=@identity_ and e.recvDate between dates.HourTime and dateadd(hour,1,dates.HourTime) and (e.TotalEnergy is not null or e.TotalWaterRate is not null)) order by dates.HourTime";
            parameters = this.GetListData<ByHourStat>(sqlstring, new { identity_ = identity, day_ = day });
            
            return parameters;
        }

               

        public IEnumerable<ByHourStat> GetStatByDays(DateTime month, string identity)
        {
            IEnumerable<ByHourStat> parameters = Enumerable.Empty<ByHourStat>();
            DateTime startMonth = new DateTime(month.Year, month.Month, 1);
            string sqlstring = @"WITH dates AS (
  select DATEADD(dd, DATEDIFF(dd,0,@day_), 0) as HourTime
  UNION ALL
  SELECT dateadd(day,1,dates.HourTime)
  FROM dates
  WHERE dates.HourTime < dateadd(month,1,DATEADD(dd, DATEDIFF(dd,0,@day_), 0))
) select dates.HourTime, params.recvDate, params.TotalEnergy, params.TotalWaterRate  from dates
left join 
(select par.recvDate, par.TotalEnergy, par.TotalWaterRate from PumpParameters par where par.[identity]=@identity_) params on params.recvDate=(select min(e.recvDate) from PumpParameters e where e.[identity]=@identity_ and e.recvDate between dates.HourTime and dateadd(day,1,dates.HourTime) and (e.TotalEnergy is not null or e.TotalWaterRate is not null)) order by dates.HourTime";
            parameters = this.GetListData<ByHourStat>(sqlstring, new { identity_ = identity, day_ = startMonth });
            return parameters;
        }

        #endregion
        
        

    }
}
