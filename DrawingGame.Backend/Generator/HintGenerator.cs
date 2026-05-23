namespace DrawingGame.Backend.Generator;

public static class HintGenerator
{
    public record Hint()
    {
        public string HintText = "";
        public List<int> Current = new();
    }
    
    public static Hint GenerateHint(string answer, List<int> current, int add = 1)
    {
        if (current.Count < answer.Length)
        {
            while (true)
            {
                int i = Random.Shared.Next(answer.Length);
                if(!current.Contains(answer[i]))
                {
                    current.Add(answer[i]);
                    break;
                }
            }
        }

        string hint = "";
        for (int i = 0; i < answer.Length; i++)
        {
            if (current.Contains(answer[i]))
            {
                hint += answer[i];
            }
            else hint += "_";
        }
        return new Hint(){HintText = hint, Current = current};
    }
}