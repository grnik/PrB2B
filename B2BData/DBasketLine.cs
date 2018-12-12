using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2BData
{
    public class DBasketLine
    {
        #region Properties
        public int BasketNo { get; set; }
        public int LineNo { get; set; }
        public string ItemNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        #endregion

        #region Constructors

        public DBasketLine() { }

        internal DBasketLine(SqlDataReader reader)
        {
            Translate(reader);
        }

        #endregion

        #region Methods

        internal void Translate(SqlDataReader reader)
        {
            BasketNo = (int)reader["BasketNo"];
            LineNo = (int)reader["LineNo"];
            ItemNo = reader["ItemNo"].ToString();
            Quantity = (decimal)reader["Quantity"];
            Price = (decimal)reader["Price"];
        }

        public void Insert()
        {
            /*
INSERT INTO [dbo].[B2B_BasketLine]
           ([BasketNo]
           ,[LineNo]
           ,[ItemNo]
           ,[Quantity]
           ,[Price])
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
INSERT INTO [dbo].[B2B_BasketLine]
           ([BasketNo]
           ,[LineNo]
           ,[ItemNo]
           ,[Quantity]
           ,[Price])
     VALUES
           (@BasketNo, @LineNo, @ItemNo,@Quantity,@Price)
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("BasketNo", SqlDbType.Int).Value = BasketNo;
                command.Parameters.Add("LineNo", SqlDbType.Int).Value = LineNo;
                command.Parameters.Add("ItemNo", SqlDbType.VarChar).Value = String.IsNullOrEmpty(ItemNo) ? "" : ItemNo;
                command.Parameters.Add("Quantity", SqlDbType.Decimal).Value = Quantity;
                command.Parameters.Add("Price", SqlDbType.Decimal).Value = Price;

                connect.Open();
                command.ExecuteNonQuery();
            }
        }

        public static List<DBasketLine> GetByBasket(int basketNo)
        {
            List<DBasketLine> res = new List<DBasketLine>();
            using (SqlConnection connect = new SqlConnection(Properties.Settings.Default.ConnectionB2B))
            {
                string commText = @"
SELECT *
  FROM [dbo].[B2B_BasketLine]
  WHERE BasketNo = @BasketNo
                ";
                SqlCommand command = new SqlCommand(commText, connect);
                command.Parameters.Add("BasketNo", SqlDbType.Int).Value = basketNo;
                connect.Open();
                using (SqlDataReader data = command.ExecuteReader())
                {
                    while (data.Read())
                    {
                        res.Add(new DBasketLine(data));
                    }
                }
            }

            return res;
        }

        #endregion
    }
}
