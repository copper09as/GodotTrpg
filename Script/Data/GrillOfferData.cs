using Godot;
using System;
using System.Collections.Generic;
[GlobalClass]
public partial class GrillOfferData : Resource
{
    [Export] public Godot.Collections.Dictionary<string, string> InkPot { get; set; }
    [Export] public string Name { get; set; }
    [Export(PropertyHint.MultilineText)] public string Description { get; set; }
    [Export(PropertyHint.MultilineText)] public string Rules { get; set; }
    [Export(PropertyHint.MultilineText)] public string ConsequencesL { get; set; }
    [Export(PropertyHint.MultilineText)] public string ConsequencesM { get; set; }
    [Export(PropertyHint.MultilineText)] public string ConsequencesH { get; set; }
    [Export(PropertyHint.MultilineText)] public string ConsequencesB { get; set; }
}