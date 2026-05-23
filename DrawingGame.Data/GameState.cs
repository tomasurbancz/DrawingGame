namespace DrawingGame.Data;

public record GameState
{
    public List<string> Players = new();
    public string CurrentDrawer = "";
    public int CurrentRound = 0;
    public List<Stroke> Strokes = new();
    public string Timer = "";
    public bool Started = false;
    public List<Score> Scores = new();
}