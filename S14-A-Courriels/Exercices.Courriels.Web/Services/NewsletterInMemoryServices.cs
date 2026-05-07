using Exercices.Courriels.Web.Interfaces;
using Exercices.Courriels.Web.Models;

namespace Exercices.Courriels.Web.Services
{
    public class NewsletterInMemoryService() : INewsletterService
    {
        private readonly HashSet<Subscriber> _context = [];
        private const int NO_CHANGE_VALUE = 0;
        private const int CHANGE_VALUE = 1;

        public List<Subscriber> GetSubscribers() => new();

        public int Subscribe(Subscriber subscriber)
        {
            return _context.Add(subscriber) ? CHANGE_VALUE : NO_CHANGE_VALUE;
        }

        public int Unsubscribe(string email)
        {
            var subscriber = _context.FirstOrDefault(s => s.Email == email);
            if (subscriber != null)
            {
                return _context.Remove(subscriber) ? CHANGE_VALUE : NO_CHANGE_VALUE;
            }
            return NO_CHANGE_VALUE;
        }
    }
}
