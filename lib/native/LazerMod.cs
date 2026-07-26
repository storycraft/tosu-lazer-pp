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
    public Dictionary<string, object>? Settings { get; set; }

    internal readonly Mod ToMod(Ruleset ruleset)
    {
        var apiMod = new APIMod
        {
            Acronym = Acronym,
        };
        if (Settings != null)
        {
            apiMod.Settings = Settings;
        }

        return apiMod.ToMod(ruleset);
    }
}