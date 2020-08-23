using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Helper {
    public class SmtpCredentials {

        public string Host {
            get; set;
        }

        public string From {
            get; set;
        }

        public string Alias {
            get; set;
        }

        public string Port {
            get; set;
        }
    }
}
