using FluentValidation;
using UsageCollectorWorkerService.Models;

namespace UsageCollectorWorkerService.Validators;

public class RootSysResUsageValidator : AbstractValidator<RootSysResUsageValues>
{
    public RootSysResUsageValidator(IValidator<SysResUsageValues> sysResUsageValidator)
    {
        RuleFor(x => x).NotNull();

        RuleFor(x => x.UsageValues)
            .NotNull()
            .NotEmpty()
            .ForEach(p => p.SetValidator(sysResUsageValidator));
    }
}