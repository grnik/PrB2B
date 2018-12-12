using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public class LCurrencyExchangeRate
    {
        #region Properties

        public string CurrencyCode { get; set; }
        public DateTime StartingDate { get; set; }
        public decimal ExchangeRate { get; set; }

        #endregion

        #region Constructores

        internal LCurrencyExchangeRate(B2BData.DCurrencyExchangeRate currencyExchangeRate)
        {
            Translate(currencyExchangeRate);
        }

        #endregion

        #region Methords

        internal void Translate(B2BData.DCurrencyExchangeRate currencyExchangeRate)
        {
            CurrencyCode = currencyExchangeRate.CurrencyCode;
            StartingDate = currencyExchangeRate.StartingDate;
            ExchangeRate = currencyExchangeRate.ExchangeRate;
        }

        internal DCurrencyExchangeRate GetData()
        {
            DCurrencyExchangeRate dCurrencyExchangeRate = new DCurrencyExchangeRate();

            dCurrencyExchangeRate.CurrencyCode = CurrencyCode;
            dCurrencyExchangeRate.StartingDate = StartingDate;
            dCurrencyExchangeRate.ExchangeRate = ExchangeRate;

            return dCurrencyExchangeRate;
        }

        public static LCurrencyExchangeRate GetByLastCurrencyRate(string currency)
        {
            DCurrencyExchangeRate dCurrencyExchangeRate = DCurrencyExchangeRate.GetByLastCurrencyRate(currency);

            return dCurrencyExchangeRate != null ? new LCurrencyExchangeRate(dCurrencyExchangeRate) : null;
        }

        #endregion
    }
}
