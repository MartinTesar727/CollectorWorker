using FluentValidation;
using UsageCollectorWorkerService.BackgroundWorkers;
using UsageCollectorWorkerService.Models;
using UsageCollectorWorkerService.Services.DataHolder;
using UsageCollectorWorkerService.Services.DataSender;
using UsageCollectorWorkerService.Services.LowLevelCollecting;
using UsageCollectorWorkerService.Services.SystemResourcesCollector;
using UsageCollectorWorkerService.Validators;

namespace UsageCollectorWorkerService
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            //Hi
            StartupArgumentsValidator.ValidateStartupArguments(args);
            
            string networkServerPath = args[0];
            int durationInSeconds = Convert.ToInt32(args[1]);
            int intervalInSeconds = Convert.ToInt32(args[2]);
            
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            
            builder.Services.AddSingleton<HttpClient>(_ => new HttpClient()
            {
                BaseAddress = new Uri(networkServerPath),
                Timeout = TimeSpan.FromSeconds(10)
            });
            
            builder.Services.AddSingleton<IDataHolderService, DataHolderService>();
            builder.Services.AddSingleton<ISenderService, SenderService>();
            builder.Services.AddSingleton<ISysResCollectingService, SysResCollectingService>();
            builder.Services.AddSingleton<ILowLevelCollectingSevice, LowLevelCollectingService>();
            
            builder.Services.AddHostedService<CollectorWorker>(provider => new CollectorWorker(
                provider.GetRequiredService<IDataHolderService>(),
                provider.GetRequiredService<ISenderService>(),
                provider.GetRequiredService<ISysResCollectingService>(), durationInSeconds, intervalInSeconds));

            builder.Services.AddSingleton<IValidator<RootSysResUsageValues>, RootSysResUsageValidator>();
            builder.Services.AddSingleton<IValidator<SysResUsageValues>, SysResUsageValidator>();
            
            IHost host = builder.Build();
            host.Run();
        }
    }
}


