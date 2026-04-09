using Microsoft.JavaScript.NodeApi;
using osu.Game.Rulesets.Difficulty;
using binding.Data;

namespace binding;

[JSExport]
[method: JSExport(false)]
public class DifficultyAttrs(DifficultyAttributes inner)
{
    [JSExport(false)]
    public DifficultyAttributes Inner { get; init; } = inner;

    public DifficultyAttrsData GetData() => DifficultyAttrsData.FromAttrs(Inner);
}