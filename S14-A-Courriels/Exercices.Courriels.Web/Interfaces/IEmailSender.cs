namespace Exercices.Courriels.Web.Interfaces
{
    public interface IEmailSender
    {
        Task Send(string to, string subject, string html, string? from = null);
    }
}
