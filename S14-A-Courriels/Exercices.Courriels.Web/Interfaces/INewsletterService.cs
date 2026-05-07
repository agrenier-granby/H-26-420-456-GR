using Exercices.Courriels.Web.Models;

namespace Exercices.Courriels.Web.Interfaces
{
    public interface INewsletterService
    {
        List<Subscriber> GetSubscribers();
        int Subscribe(Subscriber subscriber);
        int Unsubscribe(string email);
    }
}
