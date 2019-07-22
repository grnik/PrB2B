using System;
using System.Collections.Generic;

namespace B2BLogical
{
    public class SetupMail
    {
        #region Properties

        public string FromMail { get; set; }
        public string FromName { get; set; }
        public string FromPassword { get; set; }
        public string SMTPHost { get; set; }
        public int? SMTPPort { get; set; }
        public bool? SMTPEnableSSL { get; set; }
        public bool SMTPUseDefaultCredentials { get; set; }

        #endregion

        #region Constructor

        public SetupMail()
        {
            FromMail = Properties.Settings.Default.FromMail;
            FromName = Properties.Settings.Default.FromName;
            FromPassword = Properties.Settings.Default.FromPassword;
            SMTPHost = Properties.Settings.Default.SMTPHost;
            SMTPPort = Properties.Settings.Default.SMTPPort == 0 ? (int?) null : Properties.Settings.Default.SMTPPort;
            SMTPEnableSSL = Properties.Settings.Default.SMTPEnableSSL;
            SMTPUseDefaultCredentials = Properties.Settings.Default.SMTPUseDefaultCredentials;
        }

        #endregion
    }
}
