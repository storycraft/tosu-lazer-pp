using System.Collections.Generic;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;

namespace binding.Internal;

internal sealed class DiffWorkingBeatmap : FlatWorkingBeatmap
{
    private readonly IBeatmap playableBeatmap;

    internal DiffWorkingBeatmap(IBeatmap beatmap, IBeatmap playableBeatmap) : base(beatmap)
    {
        this.playableBeatmap = playableBeatmap;
    }

    // Bypass beatmap conversion logic since the beatmap is already playable state to given ruleset and mods.
    public override IBeatmap GetPlayableBeatmap(IRulesetInfo ruleset, IReadOnlyList<Mod> mods, CancellationToken token) => playableBeatmap;
}
