using System;
using System.Collections.Generic;
using Microsoft.JavaScript.NodeApi;
using osu.Game.Online.API;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;

namespace binding;

[JSExport]
public struct LazerMod
{
    public string Acronym { get; set; }
    public Dictionary<string, object> Settings { get; set; }

    internal Mod ToMod(Ruleset ruleset)
    {
        return new APIMod
        {
            Acronym = Acronym,
            Settings = Settings,
        }.ToMod(ruleset);
    }
}