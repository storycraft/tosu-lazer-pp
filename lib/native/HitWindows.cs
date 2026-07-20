using System.Collections.Generic;
using System.Linq;
using Microsoft.JavaScript.NodeApi;
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

[JSExport]
public class HitWindows(osu.Game.Rulesets.Scoring.HitWindows inner)
{
    /// <summary>
    /// Get all hit windows available.
    /// </summary>
    public IEnumerable<(OsuHitResult, double)> AllAvailableWindows()
    {
        return inner.GetAllAvailableWindows().Select(r => ((OsuHitResult)r.result, r.length));
    }

    /// <summary>
    /// Get the hit window for a specific hit result.
    /// The returned value is +- range milliseconds and clock rate is not applied.
    /// </summary>
    public double WindowFor(OsuHitResult result)
    {
        return inner.WindowFor((HitResult)result);
    }

    /// <summary>
    /// Get the hit result for a specific time offset.
    /// </summary>
    /// <param name="timeOffset">Time offset in milliseconds, without clock rate applied.</param>
    /// <returns></returns>
    public OsuHitResult ResultFor(double timeOffset)
    {
        return (OsuHitResult) inner.ResultFor(timeOffset);
    }
}