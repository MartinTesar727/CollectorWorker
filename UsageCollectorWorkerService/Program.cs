using UsageCollectorWorkerService.BackgroundWorkers;
using UsageCollectorWorkerService.Services;

namespace UsageCollectorWorkerService
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            
            builder.Services.AddSingleton<HttpClient>(_ => new HttpClient()
            {
                /*BaseAddress = new Uri(args[0]),*/
                BaseAddress = new Uri($"http://10.1.103.190:5000/api/resourceusage/save"),
                Timeout = TimeSpan.FromSeconds(10)
            });
            builder.Services.AddSingleton<ICustomHttpClient, CustomHttpClient>();
            builder.Services.AddSingleton<IGetSystemResources, GetSystemResourcesLinux>();
            
            builder.Services.AddHostedService<Worker>();

            IHost host = builder.Build();
            host.Run();
        }
    }
}


