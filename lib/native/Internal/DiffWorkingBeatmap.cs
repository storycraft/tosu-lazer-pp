using System.IO;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps;
using osu.Game.Skinning;

namespace binding.Internal;

internal sealed class DiffWorkingBeatmap(IBeatmap beatmap) : WorkingBeatmap(beatmap.BeatmapInfo, null)
{
    private readonly IBeatmap beatmap = beatmap;

    protected override IBeatmap GetBeatmap() => beatmap;

    public override Texture GetBackground() => throw new System.NotImplementedException();
    public override Stream GetStream(string storagePath) => throw new System.NotImplementedException();
    protected override Track GetBeatmapTrack() => throw new System.NotImplementedException();
    protected override ISkin GetSkin() => throw new System.NotImplementedException();
}