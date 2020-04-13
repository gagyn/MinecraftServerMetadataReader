namespace HypixelCounter.Services
{
    public class AppConfiguration
    {
        public string MongoConnectionString { get; set; }
        public string DefaultMessageSubject { get; set; }
        public string DefaultMessageText { get; set; }
        public string FromMailAddress { get; set; }
        public string ToMailAddress { get; set; }
        public string SmtpClientAddress { get; set; }
        public string SmtpClientLogin { get; set; }
        public string SmtpClientPassword { get; set; }
        public int SmtpClientPort { get; set; }
    }
}