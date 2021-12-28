using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Core;

namespace SocialNetwork.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IdentityBuilder AddApplicationIdentity(this IServiceCollection services, Action<IdentityOptions>? configureOptions = null)
        {
            services.AddAuthentication(o =>
                {
                    o.DefaultScheme = IdentityConstants.ApplicationScheme;
                    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies(o => { });

            return services.AddIdentityCore<User>(o =>
            {
                o.Stores.MaxLengthForKeys = 128;
                configureOptions?.Invoke(o);
            }).AddSignInManager();
        }
    }
}