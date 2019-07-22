using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public enum OrderChangeOperation { Add = 0, Delete = 1, Change = 2 }
    public class LOrderChange
    {
        #region Properties
        /// <summary>
        /// Поле типа Guid. Уникальный ключ строки
        /// Возвращаетяс после вставки. При передачу в систему можно передавать значение  00000000-0000-0000-0000-000000000000
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Тип документа. Для заказа = 1
        /// </summary>
        public int DocumentType { get; set; }
        /// <summary>
        /// Номер документа (КЛЗК...)
        /// </summary>
        public string DocumentNo { get; set; }
        /// <summary>
        /// Строка в документе
        /// </summary>
        public int LineNo { get; set; }
        /// <summary>
        /// Код товара. При редактировании игнорируется.
        /// </summary>
        public string ItemNo { get; set; }
        /// <summary>
        /// Новое кол-во
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Новая цена
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Операция
        /// 0 - Вставка
        /// 1 - Удалить строку
        /// 2 - Отредактировать строку
        /// </summary>
        public OrderChangeOperation Operation { get; set; }

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
        #endregion

        #region Constructors

        public LOrderChange()
        { }

        internal LOrderChange(B2BData.DOrderChange item)
        {
            Translate(item);
        }

        #endregion

        #region Methods

        internal void Translate(DOrderChange orderChange)
        {
            Id = new Guid(orderChange.Id);
            DocumentType = orderChange.DocumentType;
            DocumentNo = orderChange.DocumentNo;
            LineNo = orderChange.LineNo;
            ItemNo = orderChange.ItemNo;
            Quantity = orderChange.Quantity;
            Price = orderChange.Price;
            Operation = (OrderChangeOperation)(orderChange.Operation);
            CreateTime = orderChange.CreateTime;
            Processed = orderChange.Processed;
            Processing = orderChange.Processing;
            ProcessingTime = orderChange.ProcessingTime;
        }

        public static explicit operator DOrderChange(LOrderChange lOrderChange)
        {
            DOrderChange dOrderChange = new DOrderChange()
            {
                Id = lOrderChange.Id.ToString(),
                DocumentType = lOrderChange.DocumentType,
                DocumentNo = lOrderChange.DocumentNo,
                LineNo = lOrderChange.LineNo,
                ItemNo = lOrderChange.ItemNo,
                Quantity = lOrderChange.Quantity,
                Price = lOrderChange.Price,
                Operation = (int)(lOrderChange.Operation),
                CreateTime = lOrderChange.CreateTime,
                Processed = lOrderChange.Processed,
                Processing = lOrderChange.Processing,
                ProcessingTime = lOrderChange.ProcessingTime
            };

            return dOrderChange;
        }

        public Guid Insert(string token)
        {
            LOrder order = LOrder.GetByNo(token, DocumentType, DocumentNo);
            if(order == null)
                throw new Exception("Не найден заказ");

            this.Id = Guid.NewGuid();

            CreateTime = DateTime.Now;
            ProcessingTime = CreateTime;//Т.к. в навижене не 0
            Processed = false;
            Processing = 0;

            DOrderChange line = (DOrderChange)this;
            line.Insert();

            return Id;
        }


        public static LOrderChange GetById(string token, Guid id)
        {
            LLogin login = LLogin.CheckToken(token);
            //DCustomer customer = DCustomer.GetById(login.CustomerNo);

            DOrderChange orderChange = DOrderChange.GetById(id);
            if (orderChange == null)
                return null;

            return new LOrderChange(orderChange);
        }

        #endregion
    }
}
