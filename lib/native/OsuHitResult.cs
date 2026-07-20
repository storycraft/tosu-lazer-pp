using Microsoft.JavaScript.NodeApi;
using osu.Game.Extensions;
using osu.Game.Rulesets.Scoring;

namespace binding;

[JSExport]
public enum OsuHitResult
{
    None = HitResult.None,
    Miss = HitResult.Miss,
    Meh = HitResult.Meh,
    Ok = HitResult.Ok,
    Good = HitResult.Good,
    Great = HitResult.Great,
    Perfect = HitResult.Perfect,
    SmallTickMiss = HitResult.SmallTickMiss,
    SmallTickHit = HitResult.SmallTickHit,
    LargeTickMiss = HitResult.LargeTickMiss,
    LargeTickHit = HitResult.LargeTickHit,
    SmallBonus = HitResult.SmallBonus,
    LargeBonus = HitResult.LargeBonus,
    IgnoreMiss = HitResult.IgnoreMiss,
    IgnoreHit = HitResult.IgnoreHit,
    ComboBreak = HitResult.ComboBreak,
    SliderTailHit = HitResult.SliderTailHit,
    LegacyComboIncrease = HitResult.LegacyComboIncrease,
};
