using FluentValidation;
using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Validators;

public class SysResUsageValidator : AbstractValidator<SysResUsageValues>
{
    public SysResUsageValidator()
    {
        RuleFor(x => x.CpuUsageInPercentage)
            .InclusiveBetween(0, 100)
            .WithMessage("CPU usage must be between 0 and 100%");
        RuleFor(x => x.RamUsageInPercentage)
            .InclusiveBetween(0, 100)
            .WithMessage("RAM usage must be between 0 and 100%");
    }
}