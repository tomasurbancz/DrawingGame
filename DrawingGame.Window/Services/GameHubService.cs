using System.Threading.Tasks;
using DrawingGame.Data;
using Microsoft.AspNetCore.SignalR.Client;

namespace DrawingGame.Window.Services;

public class GameHubService
{
    public HubConnection Connection { get; private set; }

    public GameHubService()
    {
        Connection = new HubConnectionBuilder()
            .WithUrl("http://127.0.0.1/gamehubHub")
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task StartAsync()
    {
        if (Connection.State == HubConnectionState.Disconnected)
            await Connection.StartAsync();
    }

    public void Join(string name)
    {
        Connection.InvokeAsync("Join", name);
    }

    public void SendStroke(Stroke stroke)
    {

        Connection.InvokeAsync("SendStroke", stroke);
    }

    public void Leave()
    {
        Connection.InvokeAsync("Leave");
    }
}