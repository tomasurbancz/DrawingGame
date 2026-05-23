using DrawingGame.Backend.Hub;
using Microsoft.AspNetCore.SignalR;

namespace DrawingGame.Backend.Services;

public class GameTimerService : BackgroundService
{
    private readonly GameHub _hub;

    private int _timeLeft = 60;

    public GameTimerService(GameHub hub)
    {
        _hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);

            _timeLeft--;

            await _hub.UpdateTimer(_timeLeft);

            if (_timeLeft <= 0)
            {
                _timeLeft = 60;
            }
        }
    }
}