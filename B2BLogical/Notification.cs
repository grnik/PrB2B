using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;
using NLog;

namespace B2BLogical
{
    public static class Notification
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Рассылка оповещений по событиям
        /// </summary>
        static public void Run()
        {
            logger.Debug("Start send Notification for B2B");
            try
            {
                RunBasket();
                RunOrderStatus();
            }
            catch (Exception e)
            {
                logger.Error(e, "Ошибка при вызове Notivication for B2B");
            }
        }

        #region OrderStatus

        /// <summary>
        /// Сообщения по событиям с корзиной
        /// </summary>
        static private void RunOrderStatus()
        {
            List<LOrderStatus> orderStatuses = LOrderStatus.WithoutNotice();
            foreach (LOrderStatus orderStatus in orderStatuses)
            {
                DCustomer customer = DCustomer.GetById(orderStatus.Order.CustomerNo);
                List<RecipientEmail> salespersons = new List<RecipientEmail>();
                LSalesperson nb = LSalesperson.GetByCode(customer.SalespersonCodeNB);
                salespersons.Add(new RecipientEmail(nb.Name, nb.Email));
                if (nb.Code != customer.SalespersonCodeRegional)
                {
                    LSalesperson region = LSalesperson.GetByCode(customer.SalespersonCodeRegional);
                    salespersons.Add(new RecipientEmail(name: region.Name, email: region.Email));
                }

                SendOrderStatus(orderStatus, salespersons);
            }
        }

        static private void SendOrderStatus(LOrderStatus orderStatus, List<RecipientEmail> emails)
        {
            const string subject = "Изменение статуса заказа в Б2Б";
            if (!orderStatus.Processed)
            {
                string body = string.Format("При измении статуса заказа {0} клиента {1} на статус {2} произошла ошибка.", orderStatus.DocumentNo, orderStatus.Order.CustomerNo, orderStatus.NewStatus);
                SendMessage.SendGmailCom(emails, subject, body, string.Empty);
            }
            else
            {
                string body = string.Format("Статус заказ {0} клиента {1} был изменен на {2}.", orderStatus.DocumentNo, orderStatus.Order.CustomerNo, orderStatus.NewStatus);
                SendMessage.SendGmailCom(emails, subject, body, string.Empty);
            }

            orderStatus.FixNotice();
        }

        #endregion

        #region Basket

        /// <summary>
        /// Сообщения по событиям с корзиной
        /// </summary>
        static private void RunBasket()
        {
            List<LBasket> baskets = LBasket.WithoutNotice();
            foreach (LBasket basket in baskets)
            {
                DCustomer customer = DCustomer.GetById(basket.CustomerId);
                List<RecipientEmail> salespersons = new List<RecipientEmail>();
                LSalesperson nb = LSalesperson.GetByCode(customer.SalespersonCodeNB);
                salespersons.Add(new RecipientEmail(nb.Name, nb.Email));
                if (nb.Code != customer.SalespersonCodeRegional)
                {
                    LSalesperson region = LSalesperson.GetByCode(customer.SalespersonCodeRegional);
                    salespersons.Add(new RecipientEmail(name: region.Name, email: region.Email));
                }

                SendBasket(basket, salespersons);
            }
        }

        static private void SendBasket(LBasket basket, List<RecipientEmail> emails)
        {
            const string subject = "Новый заказ в Б2Б";
            if (string.IsNullOrEmpty(basket.OrderId))
            {
                string body = string.Format("При создании заказа клиента {0} по заявке {1} произошла ошибка.", basket.CustomerId, basket.EntryNo);
                body += " " + BasketLineToHTMLTable(basket);
                //Ошибка, т.к. выбираем заказы по времени
                SendMessage.SendGmailCom(emails, subject, body, string.Empty);
            }
            else
            {
                string body = string.Format("Создан заказ {0} клиента {1} по заявке {2}.", basket.OrderId, basket.CustomerId, basket.EntryNo);
                body += " " + BasketLineToHTMLTable(basket);
                SendMessage.SendGmailCom(emails, subject, body, string.Empty);
            }

            basket.FixNotice();
        }

        static private string BasketLineToHTMLTable(LBasket basket)
        {
            StringBuilder table = new StringBuilder("<table>");
            table.Append("<tr><th>Товар Но.</th><th>Комментарий</th><th>Кол-во</th><th>Цена</th></tr>");
            foreach (LBasketLine line in basket.Line)
            {
                table.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", line.ItemNo, line.Comment, line.Quantity.ToString("N"), line.Price.ToString("N")));
            }
            table.Append("</table>");

            return table.ToString();
        }

        #endregion
    }
}
