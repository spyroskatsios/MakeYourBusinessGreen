using MakeYourBusinessGreen.Api;
using MakeYourBusinessGreen.Infrastructure.Authentication;
using MakeYourBusinessGreen.Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Respawn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MakeYourBusinessGreen.Tests.Integration;
public class TestBase
{
    private IConfiguration _configuration = null!;
    private IServiceScopeFactory _scopeFactory = null!;
    private static Checkpoint _checkpoint = null!;
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();

    public TestBase()
    {

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        var webHostEnviroment = Substitute.For<IWebHostEnvironment>();
        webHostEnviroment.EnvironmentName.Returns("Development");
        webHostEnviroment.ApplicationName.Returns("MakeYourBusinessGreen.Api");

        services.AddSingleton(webHostEnviroment);

        startup.ConfigureServices(services);

        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
           d.ServiceType == typeof(ICurrentUserService));

        if (currentUserServiceDescriptor is not null)
        {
            services.Remove(currentUserServiceDescriptor);
        }


        services.AddTransient(x => _currentUserService);

        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new Respawn.Graph.Table[] { new Respawn.Graph.Table("__EFMigrationsHistory") }
        };

        EnsureDatabase();
    }
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async Task ResetStateAsync()
    => await _checkpoint.Reset(_configuration.GetConnectionString("SqlDb"));

    private void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var readDbContext = scope.ServiceProvider.GetRequiredService<ReadDbContext>();
        var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        readDbContext.Database.Migrate();
        identityDbContext.Database.Migrate();
    }

    public async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("test@local", "Testing1234!", Array.Empty<string>());
    }

    public async Task<string> RunAsAdmininAsync()
    {
        return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "Admin", "Moderator" });
    }

    public async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var user = new User { UserName = userName, Email = userName, FirstName = "FirstName", LastName = "LastName" };

        var result = await userManager.CreateAsync(user, password);

        if (roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        if (result.Succeeded)
        {
            _currentUserService.Id.Returns(user.Id);

            return user.Id;
        }

        var errors = string.Join(Environment.NewLine, result.Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    }

    public async Task<User> GetUserBeEmailASync(string email)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        return await userManager.FindByEmailAsync(email);
    }

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        return await context.FindAsync<TEntity>(keyValues);

    }

    public async Task<Suggestion?> FindSuggestionAsync(SuggestionId id)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        return await context.Suggestions.Include(x => x.Office).Include("_statusChangedEvents").FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TEntity>> FindAllAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public async Task<List<Office>> CreateOfficesAsync()
    {
        var offices = new List<Office>()
        {
            new Office(Guid.NewGuid(),"1"),
            new Office(Guid.NewGuid(),"2"),
            new Office(Guid.NewGuid(),"3"),
            new Office(Guid.NewGuid(),"4"),
            new Office(Guid.NewGuid(),"5"),
        };

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        context.Offices.AddRange(offices);
        await context.SaveChangesAsync();

        return offices;

    }

    public async Task<List<Suggestion>> CreateSuggestionsAsync(string userId = null)
    {
        if (userId is null)
        {
            userId = Guid.NewGuid().ToString();
        }

        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        var office = new Office(Guid.NewGuid(), "111");
        await context.AddAsync(office);
        await context.SaveChangesAsync();

        var suggestions = new List<Suggestion>
        {
            new Suggestion(Guid.NewGuid(), "Title","Body", new Office(Guid.NewGuid(), Guid.NewGuid().ToString()), Status.Pending, Guid.NewGuid().ToString()),
            new Suggestion(Guid.NewGuid(), "Title","Body", new Office(Guid.NewGuid(), Guid.NewGuid().ToString()), Status.Pending, Guid.NewGuid().ToString()),
            new Suggestion(Guid.NewGuid(), "Title","Body", new Office(Guid.NewGuid(), Guid.NewGuid().ToString()), Status.Pending, Guid.NewGuid().ToString()),
            new Suggestion(Guid.NewGuid(), "Title","Body", office, Status.Pending, userId),
            new Suggestion(Guid.NewGuid(), "Title","Body", office, Status.Pending, userId),
        };

        context.Suggestions.AddRange(suggestions);

        await context.SaveChangesAsync();

        return suggestions;
    }

}
