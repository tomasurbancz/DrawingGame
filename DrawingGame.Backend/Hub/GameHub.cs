using System.Collections.Concurrent;
using DrawingGame.Backend.Generator;
using DrawingGame.Data;
using Microsoft.AspNetCore.SignalR;

namespace DrawingGame.Backend.Hub;

public class GameHub : Microsoft.AspNetCore.SignalR.Hub
{
    private static ConcurrentDictionary<string, string> _connections = new ();
    private static ConcurrentBag<string> _answered = new();

    private static int _remainingGuesses = 0;

    private static string _answer = "";
    private static string _drawer = "";
    private static bool _started = false;
    private static HintGenerator.Hint? _lastHint = null;
    private static bool _first = true;
    private static int _timeLeft = 0;
    private static ConcurrentBag<Stroke> _strokes = new();
    private static int _round = 0;
    private static ConcurrentDictionary<string, Score> _scores = new();
    
    public async Task SendStroke(Stroke stroke)
    {
        await Clients.All.SendAsync("ReceiveStroke", stroke);
    }

    public async Task GenerateHint()
    {
        List<int> current = new List<int>();
        if (_lastHint != null) current = _lastHint.Current;
        HintGenerator.Hint hint = HintGenerator.GenerateHint(_answer, current, _first ? 0 : 1);
        _first = false;
        _lastHint = hint;
        await Clients.All.SendAsync("SetHint", hint.HintText);
    }
    
    private async Task DecreaseGuess()
    {
        _remainingGuesses--;
        if (_remainingGuesses <= 0)
        {
            await StartNewRound();
        }
    }
    
    public async Task CheckAnswer(string answer)
    {
        if (answer.Trim().ToLower().Equals(_answer.ToLower()))
        {
            _answered.Add(Context.ConnectionId);
            await Clients.Caller.SendAsync("ReceiveAnswerResult", true);
            await DecreaseGuess();
        }
        else await Clients.Caller.SendAsync("ReceiveAnswerResult", false);
    }

    private async Task StartNewRound()
    {
        _started = true;
        _remainingGuesses = _connections.Count;
        _answered.Clear();
        _strokes.Clear();
        _answer = AnswerGenerator.GenerateAnswer();
        _drawer = DrawerGenerator.GenerateDrawer(_connections);
        _first = true;
        _round++;
        _lastHint = null;
        await GenerateHint();
        await Clients.All.SendAsync("NewRound", _connections[_drawer]);
        await Clients.All.SendAsync("ClearStrokes");
        await SendChat($"Nyni kresli {_connections[_drawer]}");
    }
    
    private async Task SendSyncState()
    {
        await Clients.Caller.SendAsync("SyncState", null);
    }
    
    public async Task Join(string name)
    {
        _connections[Context.ConnectionId] = name;
        await SendChat($"Uzivatel {name} se pripojil");
        if (!_started)
        {
            if (_connections.Count >= 2) await StartNewRound();
        }

        GameState gameState = new GameState();
        gameState.CurrentDrawer = _drawer;
        gameState.CurrentRound = _round;
        gameState.Players = GetPlayersList();
        gameState.Scores = GetScores();
        gameState.Started = _started;
        gameState.Timer = _timeLeft + "s";
        gameState.Strokes = GetStrokes();
        
        await Clients.Caller.SendAsync("SyncState", gameState);
    }

    private List<Score> GetScores()
    {
        List<Score> scores = new();
        foreach (var score in _scores)
        {
            scores.Add(score.Value);
        }
        return scores;
    }
    
    private List<Stroke> GetStrokes()
    {
        List<Stroke> strokes = new();
        foreach (var stroke in _strokes)
        {
            strokes.Add(stroke);
        }
        return strokes;
    }
    
    private List<string> GetPlayersList()
    {
        List<string> players = new();
        foreach (var keyValuePair in _connections)
        {
            players.Add(keyValuePair.Value);
        }
        return players;
    }

    private async Task SendChat(string message)
    {
        await Clients.All.SendAsync("ReceiveChat", message);
    }

    private async Task SendScores()
    {
        await Clients.All.SendAsync("UpdateScores", null);
    }

    public async Task Leave()
    {
        string name = _connections[Context.ConnectionId];
        if (!_answered.Contains(Context.ConnectionId)) 
            await DecreaseGuess();
        _scores.TryRemove(Context.ConnectionId, out _);
        if (_connections.Count < 2)
        {
            await Clients.All.SendAsync("StopGame");
        }
        _connections.TryRemove(Context.ConnectionId, out var playerId);
        await SendChat($"Uzivatel {name} se odpojil");
        await SendScores();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Leave();
    }

    public async Task UpdateTimer(int timeLeft)
    {
        _timeLeft = timeLeft;
        if (_timeLeft <= 0)
        {
            await StartNewRound();
        }
        await Clients.All.SendAsync("UpdateTimer", _timeLeft + "s");
    }
}