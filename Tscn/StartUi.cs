using Godot;
using System;

public partial class StartUi : Control
{
    [Export] public Button ServeBtn;
    [Export] public Button ClientBtn;
    [Export] public LineEdit Iptxt;
    [Export] public LineEdit PortTxt;
    public override void _Ready()
    {
        base._Ready();
        ServeBtn.Pressed += OnServeBtn;
        ClientBtn.Pressed += OnClientBtn;
    }
    public void OnServeBtn()
    {
        GetTree().ChangeSceneToFile(StringResource.MainGame);
        NetManager.Instance.netServe = new ServeNetServe(NetManager.Instance.Multiplayer, int.Parse(PortTxt.Text),10);
    }
    public void OnClientBtn()
    {
        GetTree().ChangeSceneToFile(StringResource.MainGame);
        NetManager.Instance.netServe = new ClientNetServe(NetManager.Instance.Multiplayer, Iptxt.Text,int.Parse(PortTxt.Text));
    }

}
