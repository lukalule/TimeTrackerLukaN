using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using TimeTrackerLukaN.Client.Security;
using TimeTrackerLukaN.Client.Services;

namespace TimeTrackerLukaN.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddTokenAuthenticationStateProvider();
            services.AddTransient<ApiService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
