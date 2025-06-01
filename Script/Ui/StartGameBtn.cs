
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
            return;
        NetManager.Instance.StartGameLocal(GameManager.Instance.roomId,offerOption.Selected);
        this.QueueFree();
    }
    private void UpdateOffer()
    {
        foreach (var i in GameDataCenter.Instance.gameOfferData)
        {
            offerOption.AddItem(i.Name);
        }
    }
}
