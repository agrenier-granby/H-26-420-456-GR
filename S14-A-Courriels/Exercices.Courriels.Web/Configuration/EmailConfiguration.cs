namespace Exercices.Courriels.Web.Configuration
{
    public class EmailConfiguration
    {
        public string From { get; set; } = default!;
        public string? SmtpServer { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ApiKey { get; set; }
    }
}
