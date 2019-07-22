using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DOrderChange
    {
        #region Properties

        /// <summary>
        /// Поле типа Guid. Но для совместимости с Навижен типа string
        /// </summary>
        public string Id { get; set; }

        public int DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public int LineNo { get; set; }
        public string ItemNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public int Operation { get; set; }

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
        /// Время отправки уведомления
        /// </summary>
        public DateTime Notice { get; set; }

        #endregion

        #region Constructors

        public DOrderChange() { }

        internal DOrderChange(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methods

        internal void Translate(SqlDataReader reader)
        {
            Id = reader["Id"].ToString();
            DocumentType = (int)reader["Document Type"];
            DocumentNo = reader["Document No_"].ToString();
            LineNo = (int)reader["Line No_"];
            ItemNo = reader["ItemNo"].ToString();
            Quantity = (decimal)reader["Quantity"];
            Price = (decimal)reader["Price"];
            CreateTime = Convert.ToDateTime(reader["CreateTime"]);
            Processed = Convert.ToByte(reader["Processed"]) != 0;
            Processing = Convert.ToInt32(reader["Processing"]);
            ProcessingTime = Convert.ToDateTime(reader["Processing Time"]);
            Notice = Convert.ToDateTime(reader["Notice"]);
        }

        public void Insert()
        {
            /*
INSERT INTO [dbo].[B2B_OrderChange]
           (Id, [Document Type],[Document No_],[Line No_]
           ,[ItemNo],[Quantity],[Price]
          ,[CreateTime],[Processed],[Processing],[Processing Time])
     VALUES
           (<BasketNo, int,>
           ,<LineNo, int,>
           ,<ItemNo, varchar(20),>
           ,<Quantity, decimal(38,20),>
           ,<Price, decimal(38,20),>)
             */
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
INSERT INTO [dbo].[B2B_OrderChange]
           (Id,[Document Type],[Document No_],[Line No_]
           ,[ItemNo],[Quantity],[Price],[Operation]
          ,[CreateTime],[Processed],[Processing],[Processing Time])
     VALUES
           (@Id, @DocumentType, @DocumentNo, @LineNo
            , @ItemNo,@Quantity,@Price,@Operation
            ,@CreateTime,@Processed,@Processing,@ProcessingTime)
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Id", SqlDbType.VarChar).Value = Id;
                command.Parameters.Add("DocumentType", SqlDbType.Int).Value = DocumentType;
                command.Parameters.Add("DocumentNo", SqlDbType.VarChar).Value = DocumentNo;
                command.Parameters.Add("LineNo", SqlDbType.Int).Value = LineNo;
                command.Parameters.Add("ItemNo", SqlDbType.VarChar).Value = String.IsNullOrEmpty(ItemNo) ? "" : ItemNo;
                command.Parameters.Add("Quantity", SqlDbType.Decimal).Value = Quantity;
                command.Parameters.Add("Price", SqlDbType.Decimal).Value = Price;
                command.Parameters.Add("Operation", SqlDbType.Int).Value = Operation;
                command.Parameters.Add("CreateTime", SqlDbType.DateTime).Value = CreateTime;
                command.Parameters.Add("Processed", SqlDbType.TinyInt).Value = Processed;
                command.Parameters.Add("Processing", SqlDbType.Int).Value = Processing;
                command.Parameters.Add("ProcessingTime", SqlDbType.DateTime).Value = ProcessingTime;

                connect.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SaveNotice()
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
UPDATE [dbo].[B2B_OrderChange]
SET Notice = @Notice
WHERE [Id] = @Id
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Id", SqlDbType.VarChar).Value = Id;
                command.Parameters.Add("Notice", SqlDbType.DateTime).Value = Notice;

                connect.Open();
                command.ExecuteNonQuery();
            }
        }

        public static DOrderChange GetById(Guid id)
        {
            return GetById(id.ToString());
        }

        public static DOrderChange GetById(string id)
        {
            DOrderChange res = null;
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
  FROM [dbo].[B2B_OrderChange]
  WHERE Id = @Id
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Id", SqlDbType.VarChar).Value = id;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        res = new DOrderChange(data);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Вернуть заявки на создание заказа, по которым нет уведомлений.
        /// </summary>
        /// <returns></returns>
        public static List<DOrderChange> WithoutNotice()
        {
            List<DOrderChange> res = new List<DOrderChange>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM B2B_OrderChange
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
                        res.Add(new DOrderChange(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
