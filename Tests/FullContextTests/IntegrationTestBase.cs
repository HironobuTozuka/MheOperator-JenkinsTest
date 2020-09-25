using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using MheOperator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Serilog;

namespace Tests
{
    public class IntegrationTestBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        private ICompositeService _dockerServices;
        private IHost _host;

        [SetUp]
        public void Setup()
        {
            _host = CreateHostBuilder().Build();
            var configuration = _host.Services.GetService<IConfiguration>();
            var useDockerCompose = configuration["Tests:DockerCompose:Required"];
            if ("true".Equals(useDockerCompose, StringComparison.OrdinalIgnoreCase))
            {
                var dockerFile = Path.Combine(Directory.GetCurrentDirectory(),
                    "IntegrationTestBase/docker-compose.yml");

                Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

                _dockerServices =
                    new Builder().UseContainer()
                        .UseCompose()
                        .FromFile(dockerFile)
                        .RemoveOrphans()
                        .Build()
                        .Start();

                Log.Logger.Information("Docker name: {0}", _dockerServices.Name);
                Log.Logger.Information("Docker State: {0}", _dockerServices.State.ToString());
                Log.Logger.Information("Docker Containers: {0}", MakeString(_dockerServices.Containers));
                Log.Logger.Information("Docker Services: {0}", MakeString(_dockerServices.Services));
                Log.Logger.Information("Docker Hosts: {0}", MakeString(_dockerServices.Hosts));
                Log.Logger.Information("Docker Images: {0}", MakeString(_dockerServices.Images));
            }

            Thread.Sleep(500);
            
            _host.Start();
            ServiceProvider = _host.Services;
            
            bool dbNotCreated = true;
            StoreDbContext dbContext = null;
            while (dbNotCreated)
            {
                try
                {
                    if (dbContext == null)
                        dbContext = _host.Services.CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
                    if (dbContext.routes.Any()) dbNotCreated = false;
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Error!");
                }

                Thread.Sleep(20);
            }
        }

        private string MakeString(IReadOnlyCollection<IService> dockerServicesContainers)
        {
            return string.Join(",",dockerServicesContainers.Select(it => it.ToString()));
        }

        [TearDown]
        public async Task TearDown()
        {
            await _host.StopAsync();
            _host.WaitForShutdown();
            _host.Dispose();
        }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json"))
                .ConfigureAppConfiguration(config => config.AddEnvironmentVariables())
                .ConfigureLogging(logging => { logging.AddConsole(); })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}