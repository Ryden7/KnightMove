using Microsoft.Extensions.DependencyInjection;

namespace KnightMove
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<Function>();
        }
    }
}