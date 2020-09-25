using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Common;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace MheOperator
{
    public static class ServiceExtensions
    {
        public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConfiguration = configuration.GetSection("databaseconnection");
            if ((bool) dbConfiguration.GetValue(typeof(bool), "useFileDB"))
            {
                var connectionString = (string) dbConfiguration.GetValue(typeof(string), "filePath");
                services.AddDbContext<Data.StoreDbContext>(o => o.UseSqlite("Filename=" + connectionString, options =>
                {
                    options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "mhe");
                    options.MigrationsAssembly("Data");
                }));
            }
            else
            {
                Console.WriteLine("Connecting to database: {0}", dbConfiguration.GetValue(typeof(string), "connectionString"));
                services.AddDbContext<Data.StoreDbContext>(o =>
                    o.UseNpgsql((string) dbConfiguration.GetValue(typeof(string), "connectionString"), options =>
                    {
                        options.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "mhe");
                        options.MigrationsAssembly("Data");
                    }).EnableSensitiveDataLogging());
            }
        }

        public static void ConfigureStoreManagementClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            var smConfig = configuration.GetSection("StoreManagementClientSettings");
            services.AddHttpClient<IStoreManagementClient, StoreManagementClient.StoreManagementClient>(client =>
            {
                client.BaseAddress = new Uri((string) (smConfig.GetValue(typeof(string), "Uri")));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", 
                    smConfig.GetValue(typeof(string), "Token").ToString());
                client.Timeout = TimeSpan.FromSeconds(60);
            }).AddPolicyHandler((serviceProvider, request) =>
                GetRetryPolicy<StoreManagementClient.StoreManagementClient>(serviceProvider));
        }

        public static void ConfigureMujinClient(this IServiceCollection services, IConfiguration configuration)
        {
            var mujinConfiguration = configuration.GetSection("MujinConnectionSettings");
            var login = (string) mujinConfiguration.GetValue(typeof(string), "Login");
            var pass = (string) mujinConfiguration.GetValue(typeof(string), "Password");
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.Default.GetBytes(login + ":" + pass)));
            services.AddHttpClient<IMujinClient, MujinClient.MujinClient>(client =>
            {
                client.BaseAddress = new Uri((string) mujinConfiguration.GetValue(typeof(string), "Uri"));
                client.DefaultRequestHeaders.Authorization = authHeader;
                client.Timeout = TimeSpan.FromSeconds(60);
            }).AddPolicyHandler((serviceProvider, request) => GetRetryPolicy<MujinClient.MujinClient>(serviceProvider));
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy<T>(IServiceProvider serviceProvider)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromMilliseconds(500),
                        TimeSpan.FromMilliseconds(1000),
                        TimeSpan.FromMilliseconds(2000),
                        TimeSpan.FromMilliseconds(3500),
                        TimeSpan.FromMilliseconds(5500),
                        TimeSpan.FromMilliseconds(8000),
                        TimeSpan.FromMilliseconds(11000),
                        TimeSpan.FromMilliseconds(14500),
                        TimeSpan.FromMilliseconds(18500),
                    },
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        serviceProvider.GetService<ILogger<T>>()?
                            .LogWarning("Delaying for {delay}ms, then making retry {retry}.",
                                timespan.TotalMilliseconds, retryAttempt);
                    });
        }
    }
}