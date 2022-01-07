using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Core;
using SocialNetwork.Core.Common;
using SocialNetwork.Core.Users;
using SocialNetwork.Infrastructure.Database;
using SocialNetwork.Infrastructure.Users;

namespace SocialNetwork.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(UserStore), typeof(User))
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Gateway")))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );
            services.AddScoped<IDatabaseUnitOfWork, MySqlUnitOfWork>();
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<IDatabaseUnitOfWork>());
            return services;
        }

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