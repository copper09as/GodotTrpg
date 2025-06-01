using Godot;
using System;

public partial class OfferUiManager : FlowUi
{
    // UI 元素
    [Export] private Label NameTxt;
    [Export] private Label DescriptionTxt;
    [Export] private Label RulesTxt;

    public void UpdateUi(GrillOfferData grillOfferData)
    {
        NameTxt.Text = grillOfferData.Name;
        DescriptionTxt.Text = grillOfferData.Description;
        RulesTxt.Text = grillOfferData.Rules;
    }
}