using System.Diagnostics;

namespace UsageCollectorWorkerService.Services;

public class GetSystemResourcesLinux : IGetSystemResources
{
    public async Task<byte> GetCpuUsageInPercent()
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
        
        return (byte)(100 - Convert.ToByte(memory[11].Split(",", StringSplitOptions.RemoveEmptyEntries)[0]));
    }
    
    public async Task<byte> GetRamUsageInPercent()
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
        
        int Total = int.Parse(memory[1]);
        int Used = int.Parse(memory[2]);
        int Shared = int.Parse(memory[4]);
        
        return Convert.ToByte((Used + Shared) / (Total / 100));
    }
}