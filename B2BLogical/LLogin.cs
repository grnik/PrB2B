﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B2BData;

namespace B2BLogical
{
    public class LLogin
    {
        #region Properties

        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }

        public string Token { get; private set; }
        public DateTime LastVisitTime { get; private set; }

        public string CustomerNo { get; set; }
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
