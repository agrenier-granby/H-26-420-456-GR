using Humanizer;

namespace Exercices.Extensions.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFriendlyString(this DateTime dateTime)
        {
            return dateTime.Humanize();
        }
    }
}