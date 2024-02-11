using System.Collections.Generic;
using System.Linq;

public class ScoreTable {
    public static readonly ScoreTable Instance = new();

    public int LastScore => scores.LastOrDefault();
    public int HighScore => scores.DefaultIfEmpty().Max();

    private readonly List<int> scores = new();

    private ScoreTable() {}

    public void Submit(int score) {
        scores.Add(score);
    }
}