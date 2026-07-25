using Microsoft.JavaScript.NodeApi;
using osu.Game.Rulesets.Difficulty;
using binding.Data;
using binding.Internal;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;

namespace binding;

[JSExport]
public class GradualDifficulty
{
    private readonly Ruleset ruleset;
    private readonly Mod[] mods;
    private readonly GradualDifficultyEnumerator inner;

    internal GradualDifficulty(Ruleset ruleset, Mod[] mods, GradualDifficultyEnumerator inner)
    {
        this.ruleset = ruleset;
        this.mods = mods;
        this.inner = inner;
    }

    public bool Advance()
    {
        return inner.Advance();
    }

    /// <summary>
    /// Skips hit objects up to the specified time.
    /// </summary>
    /// <param name="time">The time to skip to, in milliseconds.</param>
    /// <returns>The number of hit objects skipped.</returns>
    public int SkipToTime(double time)
    {
        return inner.SkipToTime(time);
    }

    /// <summary>
    /// Skips the specified number of hit objects.
    /// </summary>
    /// <param name="offset">The number of hit objects to skip.</param>
    public void Skip(int offset)
    {
        inner.Skip(offset);
    }

    /// <summary>
    /// Skips all remaining hit objects.
    /// </summary>
    public void SkipToEnd()
    {
        inner.SkipToEnd();
    }

    public DifficultyAttrs CreateDifficultyAttrs()
    {
        return new(inner.CreateDifficultyAttributes());
    }

    /// <summary>
    /// Calculates the accuracy up to current sections based on hit results.
    /// </summary>=
    public double CalculateProgressiveAccuracy(ScoreInfoData data)
    {
        return AccuracyCalculator.Calculate(ruleset.RulesetInfo.OnlineID, inner.ProgressiveBeatmap, data.CreateStatistics(), mods);
    }

    /// <summary>
    /// Simulates a score with the given accuracy up to current sections.
    /// </summary>
    public ScoreInfoData CreateProgressiveScore(double accuracy)
    {
        return ScoreInfoData.FromScoreInfo(ScoreSimulator.CreateScoreInfo(ruleset, inner.ProgressiveBeatmap, mods, accuracy));
    }

    /// <summary>
    /// Gets the strains of the current difficulty section.
    /// </summary>
    public StrainsData GetCurrentStrains() => StrainsData.FromEnumerator(inner);
}