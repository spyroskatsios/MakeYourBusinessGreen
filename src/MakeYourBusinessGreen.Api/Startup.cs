using MakeYourBusinessGreen.Api.Common;
using MakeYourBusinessGreen.Api.HealthChecks;
using MakeYourBusinessGreen.Api.Services;
using MakeYourBusinessGreen.Application;
using MakeYourBusinessGreen.Application.Interfaces;
using MakeYourBusinessGreen.Infrastructure;
using MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
using Microsoft.OpenApi.Models;

namespace MakeYourBusinessGreen.Api;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfastructure(Configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("*"));
        });

        services.AddControllers();

        services.AddAuthentication();
        services.AddAuthorization();

        services.AddHttpContextAccessor();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHealthChecks()
            .AddDbContextCheck<WriteDbContext>()
            .AddDbContextCheck<ReadDbContext>()
            .AddDbContextCheck<IdentityDbContext>();

        services.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                         }
                     },
                     new List<string>()
                    }
                 });
        });
        services.AddRazorPages();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see .
            app.UseHsts();
        }

        app.UseCustomExceptionMiddleware();

        app.InstallHealthChecks();

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });

    }
}


