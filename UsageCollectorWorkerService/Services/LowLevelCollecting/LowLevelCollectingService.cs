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
        
        string[] lines = output.Split("\n");
        string[] memory = lines[3].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        
        return 100 - Convert.ToInt32(memory[11].Split(",", StringSplitOptions.RemoveEmptyEntries)[0]);
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
 
        string[] lines = output.Split("\n");
        string[] memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        
        int total = int.Parse(memory[1]);
        int used = int.Parse(memory[2]);
        int shared = int.Parse(memory[4]);
        
        return (used + shared) / (total / 100);
    }
}