using System.Diagnostics;

namespace UsageCollectorWorkerService.Services.LowLevelCollecting;

public class LowLevelCollectingService : ILowLevelCollectingSevice
{
    public async Task<int> GetCpuUsageInPercentAsync()
    {
        string output = "";
 
        ProcessStartInfo info = new ProcessStartInfo("mpstat 1 1")
        {
            FileName = "/bin/bash",
            Arguments = "-c \"mpstat 1 1\"",
            RedirectStandardOutput = true
        };
        
        using(Process process = Process.Start(info))
        {                
            output = await process.StandardOutput.ReadToEndAsync();
        }    
        
        // following lines parses linux process output which is reading CPU usage
        string[] lines = output.Split("\n");
        string[] cpuMetrics = lines[3].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        int usedCpuInPercentage = 100 - Convert.ToInt32(cpuMetrics[11].Split(",", StringSplitOptions.RemoveEmptyEntries)[0]); 
        
        return usedCpuInPercentage;
    }
    
    public async Task<int> GetRamUsageInPercentAsync()
    {
        string output = "";
 
        ProcessStartInfo info = new ProcessStartInfo("free -m")
        {
            FileName = "/bin/bash",
            Arguments = "-c \"free -m\"",
            RedirectStandardOutput = true
        };
        
        using(Process process = Process.Start(info))
        {                
            output = await process.StandardOutput.ReadToEndAsync();
        }
 
        // following lines parses linux process output which is reading RAM usage
        string[] lines = output.Split("\n");
        string[] memoryMetrics = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        
        int totalRamInstalled = int.Parse(memoryMetrics[1]);
        int totalRamUsed = int.Parse(memoryMetrics[2]) + int.Parse(memoryMetrics[4]); 
        int totalRamUsedInPercentage = totalRamUsed / (totalRamInstalled / 100);
        
        return totalRamUsedInPercentage;
    }
}