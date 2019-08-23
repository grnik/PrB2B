using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    [DataContract]
    public class LLogin
    {
        #region Properties

        [DataMember]
        public string Email { get; set; }
        //TODO: Убрать на новой версии
        //[IgnoreDataMember]
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Patronymic { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Token { get; private set; }
        [DataMember]
        public DateTime LastVisitTime { get; private set; }

        //TODO: Убрать на новой версии
        //[IgnoreDataMember]
        [DataMember]
        public string CustomerNo { get; set; }

        /// <summary>
        /// Логин может разрешать отгрузку из интернета
        /// </summary>
        [DataMember]
        public bool AllowShipmInt { get; set; }

        #endregion

        #region Constructores

        internal LLogin(B2BData.DLogin login)
        {
            Translate(login);
        }

        #endregion

        #region Methords

        internal void Translate(B2BData.DLogin login)
        {
            Email = login.Email;
            Password = login.Password;
            Name = login.Name;
            Patronymic = login.Patronymic;
            Surname = login.Surname;
            Phone = login.Phone;
            Token = login.Token;
            LastVisitTime = login.LastVisitTime;
            CustomerNo = login.CustomerNo;
            AllowShipmInt = login.AllowShipmInt;
        }

        internal DLogin GetData()
        {
            DLogin dLogin = new DLogin();

            dLogin.Email = Email;
            dLogin.Password = Password;
            dLogin.Name = Name;
            dLogin.Patronymic = Patronymic;
            dLogin.Surname = Surname;
            dLogin.Phone = Phone;
            dLogin.Token = Token;
            dLogin.LastVisitTime = LastVisitTime;

            return dLogin;
        }

        public static LLogin LogOn(string loginName, string password)
        {
            LLogin login = GetByLoginPass(loginName, password);
            if (login == null)
                throw new ExceptionLogin(ExceptionLoginType.LogOnError);

            login.Token = Guid.NewGuid().ToString();
            login.SaveTokenLastVisitTime();

            return login;
        }

        private static LLogin GetByLoginPass(string login, string password)
        {
            DLogin dLogin = DLogin.GetByLoginPass(login, password);
            return dLogin != null ? new LLogin(dLogin) : null;
        }

        /// <summary>
        /// Присваиваем и сохраняем токен
        /// </summary>
        /// <returns></returns>
        private void SaveTokenLastVisitTime()
        {
            LastVisitTime = DateTime.Now;
            GetData().SaveTokenLastVisitTime();
        }

        public static LLogin CheckToken(string token)
        {
            LLogin login = GetLoginByToken(token);
            if (login == null)
                throw new ExceptionLogin(ExceptionLoginType.TokenError);

            if ((DateTime.Now - login.LastVisitTime) > Properties.Settings.Default.LifeTimeToken)
                throw new ExceptionLogin(ExceptionLoginType.TokenError);

            login.SaveTokenLastVisitTime();

            return login;
        }

        private static LLogin GetLoginByToken(string token)
        {
            DLogin dLogin = DLogin.GetLoginByToken(token);

            return dLogin != null ? new LLogin(dLogin) : null;
        }

        #endregion
    }
}
