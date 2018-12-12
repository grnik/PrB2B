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
        public int EntryNo { get; private set; }
        public string CompanyName { get; set; }
        public string CustomerId { get; set; }
        public string Comment { get; set; }
        public string WantCheaper { get; set; }
        public decimal DeliveryPrice { get; set; }
        public int DeliveryDays { get; set; }
        public string DeliveryServiceCode { get; set; }
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
            CompanyName = reader["CompanyName"].ToString();
            CustomerId = reader["CustomerId"].ToString();
            Comment = reader["Comment"].ToString();
            WantCheaper = reader["WantCheaper"].ToString();
            DeliveryPrice = Convert.ToDecimal(reader["DeliveryPrice"]);
            DeliveryDays = Convert.ToInt32(reader["DeliveryDays"]);
            DeliveryServiceCode = reader["DeliveryServiceCode"].ToString();
            CreateTime = Convert.ToDateTime(reader["CreateTime"]);
            Processed = Convert.ToByte(reader["Processed"]) != 0;
            Processing = Convert.ToInt32(reader["Processing"]);
            ProcessingTime = Convert.ToDateTime(reader["ProcessingTime"]);
            OrderId = reader["OrderId"].ToString();
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
                int entryNo = Convert.ToInt32(command.ExecuteScalar());

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
           ,[DeliveryPrice],[DeliveryServiceCode],[DeliveryDays]
           ,[CreateTime],[Processed],[Processing],[Processing Time],[OrderId])
     VALUES
           (@EntryNo, @CompanyName, @CustomerId,@Comment,@WantCheaper
           ,@DeliveryPrice,@DeliveryServiceCode,@DeliveryDays
           ,@CreateTime,@Processed,@Processing,@ProcessingTime,@OrderId)
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
                command.Parameters.Add("CreateTime", SqlDbType.DateTime).Value = CreateTime;
                command.Parameters.Add("Processed", SqlDbType.TinyInt).Value = Processed;
                command.Parameters.Add("Processing", SqlDbType.Int).Value = Processing;
                command.Parameters.Add("ProcessingTime", SqlDbType.DateTime).Value = ProcessingTime;
                command.Parameters.Add("OrderId", SqlDbType.VarChar).Value = String.IsNullOrEmpty(OrderId) ? "" : OrderId;

                connect.Open();
                command.ExecuteNonQuery();
            }

            return EntryNo;
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

        #endregion
    }
}
