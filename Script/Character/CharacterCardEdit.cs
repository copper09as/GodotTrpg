using Godot;
using System;

public partial class CharacterCardEdit : Node
{
    [Export] private LineEdit playerName;
    [Export] private OptionButton penmanshipEdit;
    [Export] private OptionButton languageEdit;
    [Export] private OptionButton heartEdit;
    [Export] private OptionButton pathEdit;
    [Export] private Button ExitBtn;

    [Export] private TextureButton saveBtn;
    public override void _Ready()
    {
        base._Ready();
        if (saveBtn != null)
        {
            saveBtn.Pressed += OnSaveBtnPress;
        }
        ExitBtn.Pressed += LeaveScene;
    }
    private void LeaveScene()
    {
        SceneChangeManager.Instance.ChangeScene(StringResource.StartTscn);
    }
    private void OnSaveBtnPress()
    {
        if (!CanCreatePlayer())
            return;
        var player = GetGrillPlayerData();
        switch (pathEdit.Selected)
        {
            case 0: GameDataHandler.Save<GrillPlayerData>(player, StringResource.PlayerDataFilePath0); break;
            case 1: GameDataHandler.Save<GrillPlayerData>(player, StringResource.PlayerDataFilePath1); break;
            case 2: GameDataHandler.Save<GrillPlayerData>(player, StringResource.PlayerDataFilePath2); break;
        }
        GameDataCenter.Instance.UpdatePlayerList();
    }
    private GrillPlayerData GetGrillPlayerData()
    {

        GrillPlayerData player = new GrillPlayerData
        {
            Name = playerName.Text,
            Penmanship = penmanshipEdit.Selected,
            Language = languageEdit.Selected,
            Heart = heartEdit.Selected
        };
        return player;
    }
    private bool CanCreatePlayer()
    {
        if (string.IsNullOrWhiteSpace(playerName.Text))
        {
            return false;
        }
        if (penmanshipEdit.Selected == -1)
        {
            return false;
        }
        if (languageEdit.Selected == -1)
        {
            return false;
        }
        if (heartEdit.Selected == -1)
        {
            return false;
        }
        if (pathEdit.Selected == -1)
            return false;
        return true;
    }
}
