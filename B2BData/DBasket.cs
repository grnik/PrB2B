using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DBasket
    {
        #region Properties
        public int EntryNo { get; set; }
        public string CompanyName { get; set; }
        public string CustomerId { get; set; }
        public string Comment { get; set; }
        public string WantCheaper { get; set; }
        public decimal DeliveryPrice { get; set; }
        public int DeliveryDays { get; set; }
        public string DeliveryServiceCode { get; set; }
        /// <summary>
        /// Дата отгрузки
        /// </summary>
        public DateTime ShippingDate { get; set; }
        /// <summary>
        /// Реквизиты клиента
        /// </summary>
        public string EssentialCode { get; set; }
        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Если заказ перенесен в систему, то True
        /// </summary>
        public bool Processed { get; set; }
        /// <summary>
        /// Начали перенос. Если начали и не завершили. Счетчик покажет число раз.
        /// </summary>
        public int Processing { get; set; }
        /// <summary>
        /// Когда начали перенос (для повторной попытки)
        /// </summary>
        public DateTime ProcessingTime { get; set; }
        /// <summary>
        /// Номер созданного заказа
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// Оплата в кредит.
        /// </summary>
        public decimal InCredit { get; set; }
        /// <summary>
        /// Счет на сумму. На какую сумму клиенту высталвен счет на данный заказ.
        /// </summary>
        public decimal AccountSum { get; set; }
        public string ShippingNote { get; set; }
        /// <summary>
        /// Время отправки уведомления
        /// </summary>
        public DateTime Notice { get; set; }

        #endregion

        #region Constructors

        public DBasket() { }

        public DBasket(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methods

        internal void Translate(SqlDataReader reader)
        {
            EntryNo = Convert.ToInt32(reader["EntryNo"]);
            CompanyName = reader["Company Name"].ToString();
            CustomerId = reader["CustomerId"].ToString();
            Comment = reader["Comment"].ToString();
            WantCheaper = reader["WantCheaper"].ToString();
            DeliveryPrice = Convert.ToDecimal(reader["DeliveryPrice"]);
            DeliveryDays = Convert.ToInt32(reader["DeliveryDays"]);
            DeliveryServiceCode = reader["DeliveryServiceCode"].ToString();
            EssentialCode = reader["EssentialCode"].ToString();
            CreateTime = Convert.ToDateTime(reader["CreateTime"]);
            Processed = Convert.ToByte(reader["Processed"]) != 0;
            Processing = Convert.ToInt32(reader["Processing"]);
            ProcessingTime = Convert.ToDateTime(reader["Processing Time"]);
            ShippingDate = Convert.ToDateTime(reader["ShippingDate"]);
            OrderId = reader["OrderId"].ToString();
            InCredit = Convert.ToDecimal(reader["In Credit"]);
            AccountSum = Convert.ToDecimal(reader["Account Sum"]);
            ShippingNote = reader["Shipping Note"].ToString();
            Notice = Convert.ToDateTime(reader["Notice"]);
        }

        public static int LastEntryNo()
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT MAX(EntryNo)
FROM [dbo].[B2B_Basket]
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                connect.Open();
                object objEntry = command.ExecuteScalar();
                int entryNo = objEntry == DBNull.Value ? 0 : Convert.ToInt32(objEntry);

                return entryNo;
            }
        }

        public int Insert()
        {
            EntryNo = LastEntryNo() + 1;

            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
INSERT INTO [dbo].[B2B_Basket]
           ([EntryNo],[Company Name],[CustomerId],[Comment],[WantCheaper]
           ,[DeliveryPrice],[DeliveryServiceCode],[DeliveryDays], ShippingDate
           ,[EssentialCode] 
           ,[CreateTime],[Processed],[Processing],[Processing Time],[OrderId]
           ,[In Credit],[Account Sum]
           ,[Shipping Note],Notice)
     VALUES
           (@EntryNo, @CompanyName, @CustomerId,@Comment,@WantCheaper
           ,@DeliveryPrice,@DeliveryServiceCode,@DeliveryDays, @ShippingDate
           ,@EssentialCode
           ,@CreateTime,@Processed,@Processing,@ProcessingTime,@OrderId
           ,@InCredit,@AccountSum
           ,@ShippingNote,@Notice)
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("EntryNo", SqlDbType.Int).Value = EntryNo;
                command.Parameters.Add("CompanyName", SqlDbType.VarChar).Value = String.IsNullOrEmpty(CompanyName) ? "" : CompanyName;
                command.Parameters.Add("CustomerId", SqlDbType.VarChar).Value = String.IsNullOrEmpty(CustomerId) ? "" : CustomerId;
                command.Parameters.Add("Comment", SqlDbType.VarChar).Value = String.IsNullOrEmpty(Comment) ? "" : Comment;
                command.Parameters.Add("WantCheaper", SqlDbType.VarChar).Value = String.IsNullOrEmpty(WantCheaper) ? "" : WantCheaper;
                command.Parameters.Add("DeliveryPrice", SqlDbType.Decimal).Value = DeliveryPrice;
                command.Parameters.Add("DeliveryServiceCode", SqlDbType.VarChar).Value = String.IsNullOrEmpty(DeliveryServiceCode) ? "" : DeliveryServiceCode;
                command.Parameters.Add("DeliveryDays", SqlDbType.Int).Value = DeliveryDays;
                command.Parameters.Add("EssentialCode", SqlDbType.VarChar).Value = String.IsNullOrEmpty(EssentialCode) ? "" : EssentialCode;
                command.Parameters.Add("CreateTime", SqlDbType.DateTime).Value = CreateTime;
                command.Parameters.Add("Processed", SqlDbType.TinyInt).Value = Processed;
                command.Parameters.Add("Processing", SqlDbType.Int).Value = Processing;
                command.Parameters.Add("ProcessingTime", SqlDbType.DateTime).Value = ProcessingTime;
                command.Parameters.Add("ShippingDate", SqlDbType.DateTime).Value = ShippingDate;
                command.Parameters.Add("OrderId", SqlDbType.VarChar).Value = String.IsNullOrEmpty(OrderId) ? "" : OrderId;
                command.Parameters.Add("InCredit", SqlDbType.Decimal).Value = InCredit;
                command.Parameters.Add("AccountSum", SqlDbType.Decimal).Value = AccountSum;
                command.Parameters.Add("ShippingNote", SqlDbType.VarChar).Value = String.IsNullOrEmpty(ShippingNote) ? "" : ShippingNote;
                command.Parameters.Add("Notice", SqlDbType.DateTime).Value = Notice;

                connect.Open();
                command.ExecuteNonQuery();
            }

            return EntryNo;
        }

        public void SaveNotice()
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
UPDATE [dbo].[B2B_Basket]
SET Notice = @Notice
WHERE [EntryNo] = @EntryNo
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("EntryNo", SqlDbType.Int).Value = EntryNo;
                command.Parameters.Add("Notice", SqlDbType.DateTime).Value = Notice;

                connect.Open();
                command.ExecuteNonQuery();
            }
        }

        public static DBasket GetByNo(int no)
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM [B2B_Basket]
WHERE [EntryNo] = @No;
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("No", SqlDbType.Int).Value = no;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        return new DBasket(data);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Вернуть заявки на создание заказа, по которым нет уведомлений.
        /// </summary>
        /// <returns></returns>
        public static List<DBasket> WithoutNotice()
        {
            List<DBasket> res = new List<DBasket>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM B2B_Basket
WHERE Notice < '20190101 0:00:00'
  AND CreateTime < @CreateTime
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("CreateTime", SqlDbType.DateTime2).Value = DateTime.Now - Properties.Settings.Default.NotificationPause;//Через две минуты после поступления
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DBasket(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
