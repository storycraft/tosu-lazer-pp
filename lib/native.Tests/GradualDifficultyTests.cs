using binding;
using binding.Internal;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.IO.Archives;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Tests.Resources;

namespace native.Tests;

[TestFixture]
public class GradualDifficultyTests
{
    private static IEnumerable<IBeatmap> TestBeatmaps()
    {
        using var archive = new ZipArchiveReader(TestResources.GetTestBeatmapStream(true));
        var decoder = new LegacyBeatmapDecoder();
        foreach (var name in archive.Filenames)
        {
            if (!name.EndsWith(".osu")) continue;

            using var reader = new LineBufferedReader(archive.GetStream(name));
            yield return decoder.Decode(reader);
        }
    }

    private static IEnumerable<DifficultyAttributes> CalculateGradual(IBeatmap beatmap)
    {
        var playBeatmap = PlayBeatmap.FromBeatmap(beatmap);

        var gradual = playBeatmap.CreateGradualDifficulty();
        while (gradual.Advance())
        {
            yield return gradual.CreateDifficultyAttrs().Inner;
        }
    }

    [Test]
    [TestCaseSource(nameof(TestBeatmaps))]
    public void TestWithTimed(IBeatmap beatmap)
    {
        var ruleset = Rulesets.FromLegacyGameMode(beatmap.BeatmapInfo.Ruleset.OnlineID)!;

        var timedAttrs =
            ruleset
            .CreateDifficultyCalculator(new FlatWorkingBeatmap(beatmap))
            .CalculateTimed()
            .Select(timed => timed.Attributes);

        Assert.That(
            timedAttrs,
            Is.EqualTo(CalculateGradual(beatmap)).UsingPropertiesComparer()
        );
    }
}