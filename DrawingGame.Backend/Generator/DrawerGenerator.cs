using System.Collections.Concurrent;

namespace DrawingGame.Backend.Generator;

public static class DrawerGenerator
{
    public static string GenerateDrawer(ConcurrentDictionary<string, string> connections)
    {
        var keys = connections.Keys.ToArray();
        return keys[Random.Shared.Next(keys.Length)];
    }
}