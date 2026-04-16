namespace Microsoft.Extensions.DependencyInjection
{
    public static class MVCExtensions
    {
        public static IServiceCollection AddDefaultMVC(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            return services;
        }
    }
}
