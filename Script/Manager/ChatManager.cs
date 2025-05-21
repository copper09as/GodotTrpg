using Godot;
using System;

public partial class ChatManager : ItemList
{
    [Export] public TextEdit messageTxt;
    [Export] public Button SendBtn;
    [Export] public LineEdit nameTxt;
    public static ChatManager Instance;
    public override void _Ready()
    {
        base._Ready();
        SendBtn.Pressed += OnSendBtn;
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            this.QueueFree();
        }

    }
    public void OnSendBtn()
    {
        string name = "";
        if (nameTxt.Text.Equals(string.Empty))
        {
            name = "不知道是谁";
        }
        else
        {
            name = nameTxt.Text;
        }
        var message = name + ":" + messageTxt.Text;
        GameManager.Instance.RpcId(1, "ServeSendMsg", Multiplayer.GetUniqueId(), GameManager.Instance.roomId, message);
    }
    public void UpdateMessage(string message)
    {
        this.AddItem(message);
    }
}
