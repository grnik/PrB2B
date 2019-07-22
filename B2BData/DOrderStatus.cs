using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DOrderStatus
    {
        #region Properties

        /// <summary>
        /// Поле типа Guid. Но для совместимости с Навижен типа string
        /// </summary>
        public string Id { get; set; }

        public int DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public int NewStatus { get; set; }

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

        public DOrderStatus() { }

        internal DOrderStatus(SqlDataReader reader)
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
            NewStatus = (int)reader["New Status"];
            CreateTime = Convert.ToDateTime(reader["CreateTime"]);
            Processed = Convert.ToByte(reader["Processed"]) != 0;
            Processing = Convert.ToInt32(reader["Processing"]);
            ProcessingTime = Convert.ToDateTime(reader["Processing Time"]);
            Notice = Convert.ToDateTime(reader["Notice"]);
        }

        public void Insert()
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
INSERT INTO [dbo].[B2B_OrderStatus]
           (Id,[Document Type],[Document No_]
          ,[New Status]
          ,[CreateTime],[Processed],[Processing],[Processing Time]
          ,Notice)
     VALUES
           (@Id, @DocumentType, @DocumentNo,@NewStatus
            ,@CreateTime,@Processed,@Processing,@ProcessingTime
            ,@Notice)
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Id", SqlDbType.VarChar).Value = Id;
                command.Parameters.Add("DocumentType", SqlDbType.Int).Value = DocumentType;
                command.Parameters.Add("DocumentNo", SqlDbType.VarChar).Value = DocumentNo;
                command.Parameters.Add("NewStatus", SqlDbType.Int).Value = NewStatus;
                command.Parameters.Add("CreateTime", SqlDbType.DateTime).Value = CreateTime;
                command.Parameters.Add("Processed", SqlDbType.TinyInt).Value = Processed;
                command.Parameters.Add("Processing", SqlDbType.Int).Value = Processing;
                command.Parameters.Add("ProcessingTime", SqlDbType.DateTime).Value = ProcessingTime;
                command.Parameters.Add("Notice", SqlDbType.DateTime).Value = Notice;

                connect.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SaveNotice()
        {
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
UPDATE [dbo].[B2B_OrderStatus]
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

        public static DOrderStatus GetById(Guid id)
        {
            return GetById(id.ToString());
        }

        public static DOrderStatus GetById(string id)
        {
            DOrderStatus res = null;
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
  FROM [dbo].[B2B_OrderStatus]
  WHERE Id = @Id
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("Id", SqlDbType.VarChar).Value = id;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    if (data.Read())
                    {
                        res = new DOrderStatus(data);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// Вернуть заявки на создание заказа, по которым нет уведомлений.
        /// </summary>
        /// <returns></returns>
        public static List<DOrderStatus> WithoutNotice()
        {
            List<DOrderStatus> res = new List<DOrderStatus>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
FROM B2B_OrderStatus
WHERE Notice < '20190101 0:00:00'
  AND CreateTime < @CreateTime
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                DateTime createTime = DateTime.Now - Properties.Settings.Default.NotificationPause;
                command.Parameters.Add("CreateTime", SqlDbType.DateTime2).Value = createTime;//Через две минуты после поступления
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DOrderStatus(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
