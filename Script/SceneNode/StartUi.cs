using Godot;
using System;
public partial class StartUi : Control
{
    [Export] private Button ServeBtn;
    [Export] private Button ClientBtn;
    [Export] private Button CreateCardBtn;
    [Export] public LineEdit Iptxt;
    [Export] public LineEdit PortTxt;
    public override void _Ready()
    {
        base._Ready();
        ServeBtn.Pressed += OnServeBtn;
        ClientBtn.Pressed += OnClientBtn;
        CreateCardBtn.Pressed += OnCreateBtn;
    }
    public void OnServeBtn()
    {
        NetManager.Instance.netServe = ServeNetServe.GetInstance(NetManager.Instance.Multiplayer, int.Parse(PortTxt.Text), 10);
    }
    public void OnClientBtn()
    {
        NetManager.Instance.netServe = ClientNetServe.GetInstance(NetManager.Instance.Multiplayer, Iptxt.Text, int.Parse(PortTxt.Text));
    }
    public void OnCreateBtn()
    {
        SceneChangeManager.Instance.ChangeScene(StringResource.CreateCardTscn);
    }

}
