namespace UsageCollectorWorkerService.Orchestrators;

public interface IClientOrchestrator
{ 
    Task StartCollectingAsync();
}