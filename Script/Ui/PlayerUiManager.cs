using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerUiManager : FlowUi
{
    // UI 元素
    [Export] private Label NameTxt;
    [Export] private Label penmanshipTxt;
    [Export] private Label languageTxt;
    [Export] private Label heartTxt;
    private Dictionary<int, string> playerLv = new Dictionary<int, string>();
    // 动画参数

    public override void _Ready()
    {
        base._Ready();
        playerLv[0] = "可怜";
        playerLv[1] = "普通";
        playerLv[2] = "优秀";

    }

    public void UpdateUi(GrillPlayerData grillPlayerData)
    {
        NameTxt.Text = grillPlayerData.Name;
        languageTxt.Text = "语言" + playerLv[grillPlayerData.Language];
        penmanshipTxt.Text = "书法" + playerLv[grillPlayerData.Penmanship];
        heartTxt.Text = "心灵" + playerLv[grillPlayerData.Heart];
    }
}
