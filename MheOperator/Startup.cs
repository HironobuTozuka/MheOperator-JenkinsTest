using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PlcCommunicationService;
using PlcRequestQueueService;
using RcsLogic;
using Tests;
using RcsLogic.Models;
using RcsLogic.RcsController;
using RcsLogic.RcsController.ToteCommand;
using RcsLogic.RcsController.Recovery;
using RcsLogic.Robot;
using RcsLogic.Services;
using RcsLogic.Watchdog;

namespace MheOperator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {NamingStrategy = new SnakeCaseNamingStrategy()};
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("storeManagementApi", new OpenApiInfo() {Title = "StoreManagement API", Version = "v1"});
                var filePath = Path.Combine(AppContext.BaseDirectory, "MheOperator.xml");
                c.IncludeXmlComments(filePath);
                c.DescribeAllEnumsAsStrings();
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.ConfigureDatabaseContext(Configuration);
            var usePlcTranslatorMock =
                (bool) Configuration.GetSection("PlcTranslatorSettings").GetValue(typeof(bool), "UseMock");
            if (usePlcTranslatorMock)
            {
                services.AddSingleton<IPlcService, PlcServiceMock>();
            }
            else
            {
                services.AddSingleton<IPlcService, PlcService>();
            }

            services.AddSingleton<RcsLogic.RcsInitializer>();
            var useMujinMock =
                (bool) Configuration.GetSection("MujinConnectionSettings").GetValue(typeof(bool), "UseMock");
            if (useMujinMock)
            {
                services.AddSingleton<IMujinClient, MujinClientMock>();
            }
            else
            {
                services.ConfigureMujinClient(Configuration);
            }

            var useStoreManagementMock = (bool) Configuration.GetSection("StoreManagementClientSettings")
                .GetValue(typeof(bool), "UseMock");
            if (useStoreManagementMock)
            {
                services.AddSingleton<IStoreManagementClient, StoreManagementMock>();
            }
            else
            {
                services.ConfigureStoreManagementClient(Configuration);
            }

            services.AddSingleton<LocationRepository>();
            services.AddSingleton<DeviceStatusService>();
            services.AddSingleton<ToteRepository>();
            services.AddSingleton<LocationService>();
            services.AddSingleton<ToteService>();
            services.AddSingleton<TaskBundleService>();
            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<TotesReadyForPicking>();
            services.AddSingleton<ServicedLocationProvider>();
            services.AddSingleton<RoutingService>();
            services.AddSingleton<LocationStatus>();
            services.AddSingleton<PickRequestDoneListenerRegistry>();
            services.AddSingleton<TransferRequestDoneListenerRegistry>();
            services.AddSingleton<ScanNotificationListenerRegistry>();
            services.AddSingleton<MoveTaskCompletingScanNotificationListener>();
            services.AddSingleton<IKafkaConsumerGroup, KafkaConsumerGroup>();
            services.AddSingleton<PlcNotificationListenerRegistry>();
            services.AddSingleton<UnknownToteRouter>();
            services.AddSingleton<RecoveryHandler>();
            services.AddSingleton<DeviceRegistry>();
            services.AddSingleton<TaskBundleWatchdog>();
            services.AddSingleton<RcsController>();
            services.AddSingleton<IReturnToteHandler, RcsController>();
            services.AddSingleton<TransferRequestDoneWatcher>();
            services.AddSingleton<ToteLocationUpdatingScanNotificationListener>();
            services.AddSingleton<ToteLocationWatchdog>();
            services.AddSingleton<ToteLocationUnknownWatchdog>();
            services.AddSingleton<WatchdogExecutor>();
            services.AddSingleton<IDeviceInitializer, DeviceInitializer>();
            services.AddSingleton<StoreManagementNotifyingTransferDoneListener>();
            services.AddSingleton<ToteCommandDecisionTree>();
            services.AddSingleton<ToteBarcodeReadOnRequestForNoReadTransferDoneListener>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthorization();

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/storeManagementApi/swagger.json", "StoreManagement API V1");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.ApplicationServices.CreateScope().ServiceProvider.GetService<StoreDbContext>().Initialize();
            app.ApplicationServices.GetService<RcsInitializer>();
        }
    }
}