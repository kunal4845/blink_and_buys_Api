using System.Collections.Generic;

namespace Core.Helper
{
    public class SmtpCredentials
    {
        public string Host { get; set; }
        public string Alias { get; set; }
        public int Port { get; set; }
        public string Subject { get; set; }
        public string ServerAddress { get; set; }
        public string Password { get; set; }
        public List<string> To { get; set; }
    }
}
