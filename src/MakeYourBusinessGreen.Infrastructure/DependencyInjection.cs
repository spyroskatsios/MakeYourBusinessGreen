using MakeYourBusinessGreen.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MakeYourBusinessGreen.Infrastructure.Authentication;
using MakeYourBusinessGreen.Infrastructure.Persistence;
using MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
using MakeYourBusinessGreen.Infrastructure.Persistence.Interfaces;
using MakeYourBusinessGreen.Infrastructure.Persistence.Repositories;
using MakeYourBusinessGreen.Infrastructure.Services;
using MakeYourBusinessGreen.Infrastructure.Settings;

namespace MakeYourBusinessGreen.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSqlServer(configuration);
        services.ConfigureIdentity();
        services.ConfigureJWT(configuration);

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IMailService, MailService>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddTransient<IUserService, UserService>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        services.AddScoped<IIdentityDbContext, IdentityDbContext>();
        services.AddScoped<SeedData>();

        return services;
    }

    private static IServiceCollection ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WriteDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlDb"));
        });

        services.AddDbContext<ReadDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlDb"));
        });

        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlDb"));
        });

        return services;
    }

    private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<User>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequiredLength = 6;
            o.User.RequireUniqueEmail = true;
        });

        builder.AddRoles<IdentityRole>();

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);

        builder.AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

        services.AddSingleton(jwtSettings);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.ValidIssuer,
                ValidAudience = jwtSettings.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
