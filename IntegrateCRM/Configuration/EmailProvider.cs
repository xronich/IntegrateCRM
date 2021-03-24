namespace IntegrateCRM.Configuration
{
    public class EmailProvider
    {
        public string SenderEmail { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Name { get; set; }
        public string ReceiverEmails { get; set; }
        public string ConfigPassword { get; set; }
    }
}
