using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;

namespace binding.Internal;

internal static class Rulesets
{
    public static readonly OsuRuleset osuRuleset = new();
    public static readonly TaikoRuleset taikoRuleset = new();
    public static readonly CatchRuleset catchRuleset = new();
    public static readonly ManiaRuleset maniaRuleset = new();

    /// <summary>
    /// Returns a ruleset instance from a legacy online ruleset id.
    /// </summary>
    /// <param name="gameMode">Legacy online ruleset id</param>
    /// <returns>An Ruleset instance implementing ILegacyRuleset</returns>
    public static Ruleset? FromLegacyGameMode(int gameMode) => gameMode switch
    {
        0 => osuRuleset,
        1 => taikoRuleset,
        2 => catchRuleset,
        3 => maniaRuleset,
        _ => null,
    };
}