using System.Text.RegularExpressions;

namespace UsageCollectorWorkerService.Validators;

public static class StartupArgumentsValidator
{
    public static void ValidateStartupArguments(string[] args)
    {
        if (args == null || args.Length != 3)
        {
            throw new ArgumentException("3 startup arguments are required");
        }
        
        if (!Regex.IsMatch(args[0], @"(http|https)://\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{4,5}/.*?"))
        {
            throw new ArgumentException("server path argument is invalid");
        }

        if (Regex.IsMatch(args[1], @"\D") || Regex.IsMatch(args[2], @"\D"))
        {
            throw new ArgumentException("duration and interval arguments must be positive integer");
        }

        if (Convert.ToInt32(args[1]) < 10 || Convert.ToInt32(args[1]) > 600 )
        {
            throw new ArgumentException("collecting time must be between 10 and 600 seconds");
        }

        if (Convert.ToInt32(args[2]) < 1 || Convert.ToInt32(args[2]) > 60 )
        {
            throw new ArgumentException("interval between collecting must be between 1 and 60 seconds");
        }

        if (Convert.ToInt32(args[2]) >= Convert.ToInt32(args[1]))
        {
            throw new ArgumentException("interval between collecting cannot be same or bigger than collecting time in total");
        }
    }
}