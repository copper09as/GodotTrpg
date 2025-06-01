using Godot;
using Microsoft.VisualBasic;
using System;

public partial class ChoseCharacter : Control
{
    [Export] private OptionButton characterOption;

    public override void _Ready()
    {
        base._Ready();
        Active();
    }

    public void Active()
    {
        if (GameManager.Instance.IsHost)
        {
            this.Hide();
            return;
        }
        GameDataCenter.Instance.currentPlayerData = GameDataCenter.Instance.gamePlayerData[0];
        this.Show();
        characterOption.ItemSelected += OnCharacterChange;
        foreach (var i in GameDataCenter.Instance.gamePlayerData)
            characterOption.AddItem(i.Name);
    }

    private void OnCharacterChange(long index)
    {
        GameDataCenter.Instance.currentPlayerData = GameDataCenter.Instance.gamePlayerData[characterOption.Selected];
    }
}
