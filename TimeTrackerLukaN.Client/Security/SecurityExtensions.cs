using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTrackerLukaN.Client.Security
{
    public static class SecurityExtensions
    {
        public static void AddTokenAuthenticationStateProvider(this IServiceCollection services)
        {
            services.AddScoped<TokenAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider =>
                    provider.GetRequiredService<TokenAuthenticationStateProvider>());
        }
    }
}
