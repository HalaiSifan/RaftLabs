using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using RaftLabsAssignment2025.Clients;
using RaftLabsAssignment2025.Configuration;
using RaftLabsAssignment2025.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<ReqResApiOptions>(context.Configuration.GetSection("ReqResApi"));

        services.AddMemoryCache();
        services.AddHttpClient<IReqResClient, ReqResClient>();
        services.AddTransient<ExternalUserService>();
    })
    .Build();

var service = host.Services.GetRequiredService<ExternalUserService>();

// Demo: Fetch a single user
var user = await service.GetUserByIdAsync(2);
Console.WriteLine($"User: {user.first_name} {user.last_name} - {user.email}");

// Demo: Fetch all users
var users = await service.GetAllUsersAsync();
Console.WriteLine($"Total users fetched: {users.Count()}");

Console.ReadLine();