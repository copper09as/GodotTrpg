using Godot;
using System;
using System.Security.AccessControl;

public partial class ScoreUi : Node
{
    [Export] private Label descriptionTxt;
    [Export] private Control containNode;
    [Export] private Button RestartBtn;
    public override void _Ready()
    {
        base._Ready();
        RestartBtn.Pressed += Restart;
        containNode.Hide();
    }
    private void Restart()
    {
        NetManager.Instance.RpcId(1,"ExitRoom",GameManager.Instance.roomId,Multiplayer.GetUniqueId());
        SceneChangeManager.Instance.ChangeScene(StringResource.MainGame);
    }
    public void UpdateUi(string message)
    {
        containNode.Show();
        descriptionTxt.Text = message;
    }
}
