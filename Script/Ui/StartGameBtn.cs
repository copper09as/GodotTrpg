
using Godot;
using System;

public partial class StartGameBtn : TextureButton
{
    [Export] private OptionButton offerOption;
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnStartGame;
        UpdateOffer();
        
    }
    private void OnStartGame()
    {
        if (offerOption.Selected == -1)
        {
            GD.Print("未选定");
            return;
        }
        ServeEventCenter.RegisterOneTimeEvent(StringResource.StartGame,StartGame);
        NetManager.Instance.StartGameLocal(GameManager.Instance.roomId, offerOption.Selected);
        
    }
    private void UpdateOffer()
    {
        foreach (var i in GameDataCenter.Instance.gameOfferData)
        {
            offerOption.AddItem(i.Name);
        }
    }
    private void StartGame(int offer)
    {
        this.Hide();
    }
}
