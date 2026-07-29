using System;
using System.IO;
using System.Text;
using Microsoft.JavaScript.NodeApi;
using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using binding.Data;
using binding.Internal;
using Decoder = osu.Game.Beatmaps.Formats.Decoder;
using System.Threading;
using System.Linq;

namespace binding;

/// <summary>
/// A beatmap with a ruleset and applied mods.
/// </summary>
[JSExport]
public class PlayBeatmap
{
    private readonly FlatWorkingBeatmap workingBeatmap;

    internal IBeatmap Beatmap => workingBeatmap.Beatmap;

    internal readonly Ruleset ruleset;

    private Mod[] mods;

    /// <summary>
    /// The online ID of the current beatmap's ruleset. Also known as legacy gamemode ID.
    /// </summary>
    public int Mode => ruleset.RulesetInfo.OnlineID;

    private PlayBeatmap(FlatWorkingBeatmap workingBeatmap, Ruleset ruleset)
    {
        this.workingBeatmap = workingBeatmap;
        this.ruleset = ruleset;
        mods = [];
    }

    /// <summary>
    /// Set beatmap mods.
    /// </summary>
    public void ApplyMods(LazerMod[] mods)
    {
        InvalidatePlayableBeatmap();
        this.mods = [.. mods.Select(m => m.ToMod(ruleset))];
    }

    private IBeatmap? cachedPlayableBeatmap;

    /// <summary>
    /// Invalidate cached playable beatmap. Should be called when changing mods.
    /// </summary>
    private void InvalidatePlayableBeatmap() => cachedPlayableBeatmap = null;

    /// <summary>
    /// Lazily construct or get beatmap with current mods and ruleset applied.
    /// </summary>
    internal IBeatmap GetPlayableBeatmap()
    {
        if (cachedPlayableBeatmap != null)
        {
            return cachedPlayableBeatmap;
        }

        return cachedPlayableBeatmap = workingBeatmap.GetPlayableBeatmap(
            ruleset.RulesetInfo,
            mods,
            CancellationToken.None
        );
    }

    /// <summary>
    /// Perform beatmap conversion to another gamemode.
    /// Applied mods will not be retained to returned beatmap.
    /// </summary>
    public PlayBeatmap? Convert(int gameMode)
    {
        var ruleset = Rulesets.FromLegacyGameMode(gameMode);
        if (ruleset is null)
        {
            return null;
        }

        return new(workingBeatmap, ruleset);
    }

    /// <summary>
    /// Get the original beatmap difficulty without any mods applied.
    /// </summary>
    /// <returns></returns>
    public BeatmapDifficultyData GetOriginalBeatmapDifficulty() =>
        BeatmapDifficultyData.FromDifficulty(workingBeatmap.BeatmapInfo.Difficulty);

    /// <summary>
    /// Get beatmap difficulty with current mods applied
    /// </summary>
    public BeatmapDifficultyData GetBeatmapDifficulty() =>
        BeatmapDifficultyData.FromDifficulty(
            ruleset.GetAdjustedDisplayDifficulty(
                GetPlayableBeatmap().BeatmapInfo,
                mods
            )
        );

    /// <summary>
    /// Create gradual difficulty calculator with current mods applied.
    /// </summary>
    public GradualDifficulty CreateGradualDifficulty()
    {
        return new GradualDifficulty(
            ruleset,
            mods,
            ruleset.CreateDifficultyCalculator(
                new DiffWorkingBeatmap(workingBeatmap.Beatmap, GetPlayableBeatmap())
            ).CreateGradualDifficulty(mods)
        );
    }

    /// <summary>
    /// Calculate performance
    /// </summary>
    public PerformanceAttrsData CalculatePerformance(
        DifficultyAttrs attrs,
        ScoreInfoData score
    )
    {
        var calc = ruleset.CreatePerformanceCalculator();
        if (calc is null)
        {
            return default;
        }

        return PerformanceAttrsData.FromAttrs(
            calc.Calculate(
                score.ToPerformanceScoreInfo(this),
                attrs.Inner
            )
        );
    }

    /// <summary>
    /// Create a score closest to given accuracy with the current beatmap.
    /// Generated score only have hit results and accuracy.
    /// </summary>
    public ScoreInfoData CreateScore(double accuracy) => ScoreInfoData.FromScoreInfo(
        ScoreSimulator.CreateScoreInfo(
            ruleset,
            GetPlayableBeatmap(),
            mods,
            accuracy
        )
    );

    /// <summary>
    /// Calculate accuracy based on hit results and mods.
    /// The beatmap's gamemode is determined by its ruleset.
    /// From https://github.com/ppy/osu-tools/blob/master/PerformanceCalculatorGUI/RulesetHelper.cs
    /// </summary>
    public double CalculateAccuracy(ScoreInfoData score) => AccuracyCalculator.Calculate(
        ruleset.RulesetInfo.OnlineID,
        GetPlayableBeatmap(),
        score.CreateStatistics(),
        mods
    );

    /// <summary>
    /// Create an hit windows instance for the beatmap with mods applied.
    /// </summary>
    public HitWindows CreateHitWindows() => new(
        ruleset.CreateDrawableRulesetWith(
            GetPlayableBeatmap(),
            mods
        ).FirstAvailableHitWindows
        ?? osu.Game.Rulesets.Scoring.HitWindows.Empty
    );

    /// <summary>
    /// Parse string osu file into Beatmap
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public static PlayBeatmap Parse(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        using var reader = new LineBufferedReader(new MemoryStream(bytes));
        
        return FromBeatmap(Decoder.GetDecoder<Beatmap>(reader).Decode(reader));
    }

    internal static PlayBeatmap FromBeatmap(IBeatmap beatmap)
    {
        var rulesetId = beatmap.BeatmapInfo.Ruleset.OnlineID;
        var ruleset = Rulesets.FromLegacyGameMode(rulesetId) ?? throw new InvalidOperationException("Invalid ruleset: " + rulesetId);

        return new(new(beatmap), ruleset);
    }
}
