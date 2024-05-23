using MassTransit;

namespace Leeds.Cosumer;

public class MassTransitConsoleHostedService(IBusControl busControl) : IHostedService
{
    private readonly IBusControl busControl = busControl;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return busControl.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return busControl.StopAsync(cancellationToken);
    }
}