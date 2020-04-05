using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PumpingModule
{
    /// <summary>
    ///  Класс Денису для вставки данных по приборам
    ///  
    /// </summary>
    /// 
    public class ExternalRepository
    {
        private string ConnectionString;

        // конструктор со строкой подключения
        public ExternalRepository(string conStr)
        {
            this.ConnectionString = conStr;
        }




        /// <summary>
        /// Вставка новой строки в таблицу
        /// </summary>
        /// <param name="recvDate">Дата снятия показания</param>
        /// <param name="identity">Идентификатор станции, с которой приходят показания - к нему в программе визуализации привязывается адресс</param>
        /// <param name="totalEnergy">Значение параметра энергопотребления</param>
        /// <param name="amperage1">Значение тока 1</param>
        /// <param name="amperage2">Значение тока 2</param>
        /// <param name="amperage3">Значение тока</param>
        /// <param name="voltage1">Значение напряжения 1</param>
        /// <param name="voltage2">Значение напряжения 2</param>
        /// <param name="voltage3">Значение напряжения 3</param>
        /// <param name="currentElectricPower">Текущая мощность</param>
        /// <param name="totalWaterRate">Общий расход воды</param>
        /// <param name="errors">Список ошибок</param>
        /// <param name="presure">Давление</param>
        /// <param name="alarmCode">Какой то там важный алярм</param>
        /// <returns>объект MethodResult - характеризующий успешность операции</returns>
        public MethodResult InsertNewRow(DateTime recvDate, string identity, double? totalEnergy, double? amperage1, double? amperage2, double? amperage3, double? voltage1, double? voltage2, double? voltage3, double? currentElectricPower, double? totalWaterRate, string errors, double? presure, int alarmCode = 0)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("insert into PumpParameters (recvDate,[Identity],TotalEnergy,Amperage1,Amperage2,Amperage3,Voltage1,Voltage2,Voltage3,CurrentElectricPower,TotalWaterRate, Errors, Alarm, Presure) values(@recvDate,@Identity,@TotalEnergy,@Amperage1,@Amperage2,@Amperage3,@Voltage1,@Voltage2,@Voltage3,@CurrentElectricPower,@TotalWaterRate,@Errors,@alarmCode,@presure)", connection))
                    {
                        command.Parameters.AddWithValue("@recvDate", recvDate);
                        command.Parameters.AddWithValue("@Identity", identity);
                        command.Parameters.AddWithValue("@TotalEnergy", totalEnergy.HasValue ? (object)totalEnergy : DBNull.Value);
                        command.Parameters.AddWithValue("@Amperage1", amperage1.HasValue ? (object)amperage1 : DBNull.Value);
                        command.Parameters.AddWithValue("@Amperage2", amperage2.HasValue ? (object)amperage2 : DBNull.Value);
                        command.Parameters.AddWithValue("@Amperage3", amperage3.HasValue ? (object)amperage3 : DBNull.Value);
                        command.Parameters.AddWithValue("@Voltage1", voltage1.HasValue ? (object)voltage1 : DBNull.Value);
                        command.Parameters.AddWithValue("@Voltage2", voltage2.HasValue ? (object)voltage2 : DBNull.Value);
                        command.Parameters.AddWithValue("@Voltage3", voltage3.HasValue ? (object)voltage3 : DBNull.Value);
                        command.Parameters.AddWithValue("@CurrentElectricPower", currentElectricPower.HasValue ? (object)currentElectricPower : DBNull.Value);
                        command.Parameters.AddWithValue("@TotalWaterRate", totalWaterRate.HasValue ? (object)totalWaterRate : DBNull.Value);
                        command.Parameters.AddWithValue("@Errors", errors);
                        command.Parameters.AddWithValue("@alarmCode", alarmCode);
                        command.Parameters.AddWithValue("@presure", presure.HasValue ? (object)presure.Value : DBNull.Value);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();

                    }
                }
                return new MethodResult(true);
            }
            catch (Exception ex)
            {
                return new MethodResult(false, ex.Message + "\n" + ex.StackTrace);
            }
        }





    }

    /// <summary>
    /// Структура, возвращаемая из методов работы с БД - информация о успешности операции
    /// </summary>
    public struct MethodResult
    {

        public MethodResult(bool success)
        {
            this.isSuccess = success;
            Message = String.Empty;
        }

        public MethodResult(bool success, string message)
        {
            this.isSuccess = success;
            this.Message = message;
        }

        // как отработал метод (с ошибками или без)
        public bool isSuccess;
        // сообщение об ошибке если isSuccess=false
        public string Message;
    }

}
