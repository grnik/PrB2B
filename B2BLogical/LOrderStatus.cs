using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public class LOrderStatus
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

        public OrderStatus NewStatus { get; set; }

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
        public DateTime? Notice { get; set; }

        public LOrder Order
        {
            get
            {
                return LOrder.GetByNo(DocumentType, DocumentNo);
            }
        }
        #endregion

        #region Constructors

        public LOrderStatus()
        { }

        internal LOrderStatus(B2BData.DOrderStatus item)
        {
            Translate(item);
        }

        #endregion

        #region Methods

        internal void Translate(DOrderStatus OrderStatus)
        {
            Id = new Guid(OrderStatus.Id);
            DocumentType = OrderStatus.DocumentType;
            DocumentNo = OrderStatus.DocumentNo;
            NewStatus = (OrderStatus)(OrderStatus.NewStatus);
            CreateTime = OrderStatus.CreateTime;
            Processed = OrderStatus.Processed;
            Processing = OrderStatus.Processing;
            ProcessingTime = OrderStatus.ProcessingTime;
            Notice = OrderStatus.Notice;
        }

        internal static List<LOrderStatus> Translate(List<DOrderStatus> dOrderStatuses)
        {
            List<LOrderStatus> res = new List<LOrderStatus>();
            foreach (DOrderStatus dOrderStatus in dOrderStatuses)
            {
                res.Add(new LOrderStatus(dOrderStatus));
            }

            return res;
        }

        public static explicit operator DOrderStatus(LOrderStatus lOrderStatus)
        {
            DOrderStatus dOrderStatus = new DOrderStatus()
            {
                Id = lOrderStatus.Id.ToString(),
                DocumentType = lOrderStatus.DocumentType,
                DocumentNo = lOrderStatus.DocumentNo,
                NewStatus = (int)(lOrderStatus.NewStatus),
                CreateTime = lOrderStatus.CreateTime,
                Processed = lOrderStatus.Processed,
                Processing = lOrderStatus.Processing,
                ProcessingTime = lOrderStatus.ProcessingTime,
                Notice = lOrderStatus.Notice ?? LConst.MinDateNavision
            };

            return dOrderStatus;
        }

        public Guid Insert(string token)
        {
            LOrder order = LOrder.GetByNo(token, DocumentType, DocumentNo);
            if (order == null)
                throw new Exception("Не найден заказ");

            this.Id = Guid.NewGuid();

            CreateTime = DateTime.Now;
            ProcessingTime = CreateTime;//Т.к. в навижене не 0
            Processed = false;
            Processing = 0;

            DOrderStatus orderStatus = (DOrderStatus)this;
            orderStatus.Insert();

            return Id;
        }

        /// <summary>
        /// Фиксируем факт отправки сообщения по данной корзине.
        /// </summary>
        public void FixNotice()
        {
            Notice = DateTime.Now;
            DOrderStatus dOrderStatus = (DOrderStatus)this;

            dOrderStatus.SaveNotice();
        }

        public static LOrderStatus GetById(string token, Guid id)
        {
            LLogin login = LLogin.CheckToken(token);
            //DCustomer customer = DCustomer.GetById(login.CustomerNo);

            DOrderStatus OrderStatus = DOrderStatus.GetById(id);
            if (OrderStatus == null)
                return null;

            return new LOrderStatus(OrderStatus);
        }

        /// <summary>
        /// Вернуть заявки на создание заказа, по которым нет уведомлений.
        /// </summary>
        /// <returns></returns>
        internal static List<LOrderStatus> WithoutNotice()
        {
            return Translate(DOrderStatus.WithoutNotice());
        }

        #endregion
    }
}
