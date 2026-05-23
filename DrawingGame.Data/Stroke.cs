namespace DrawingGame.Data;

public record Stroke
{
    public float X { get; set; } = 0;
    public float Y { get; set; } = 0;
    public string Color { get; set; } = "Black";
}