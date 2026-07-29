using binding;
using binding.Internal;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.IO.Archives;
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

    private static IEnumerable<DifficultyAttributes> CalculateGradual(PlayBeatmap playBeatmap)
    {
        var gradual = playBeatmap.CreateGradualDifficulty();
        while (gradual.Advance())
        {
            yield return gradual.CreateDifficultyAttrs().Inner;
        }
    }

    private static IEnumerable<DifficultyAttributes> CalculateTimed(PlayBeatmap playBeatmap)
    {
        var workingBeatmap = new DiffWorkingBeatmap(playBeatmap.Beatmap, playBeatmap.GetPlayableBeatmap());
        return playBeatmap.ruleset
            .CreateDifficultyCalculator(workingBeatmap)
            .CalculateTimed(playBeatmap.Mods)
            .Select(timed => timed.Attributes);
    }

    [Test, Parallelizable(ParallelScope.Children)]
    [TestCaseSource(nameof(TestBeatmaps))]
    public void TestWithTimed(IBeatmap beatmap)
    {
        var playBeatmap = PlayBeatmap.FromBeatmap(beatmap);

        Assert.That(
            CalculateTimed(playBeatmap),
            Is.EqualTo(CalculateGradual(playBeatmap)).UsingPropertiesComparer(),
            "Difficulty does not match without mods"
        );

        var ruleset = playBeatmap.ruleset;
        var testMods = ruleset.GetModsFor(ModType.DifficultyIncrease)
            .Concat(ruleset.GetModsFor(ModType.DifficultyReduction))
            .Concat(ruleset.GetModsFor(ModType.Conversion))
            .Concat(ruleset.GetModsFor(ModType.System));

        foreach (var mod in testMods)
        {
            playBeatmap.ApplyMods([mod]);

            Assert.That(
                CalculateTimed(playBeatmap),
                Is.EqualTo(CalculateGradual(playBeatmap))
                    .UsingPropertiesComparer(config => config.Excluding("Mods")),
                $"Difficulty does not match with mod {mod.Name} ({mod.Acronym})"
            );
        }
    }
}