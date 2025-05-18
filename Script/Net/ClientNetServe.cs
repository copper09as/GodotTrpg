using Godot;

public class ClientNetServe : NetServe
{
    public ClientNetServe(MultiplayerApi Multiplayer,string ip,int port) : base(Multiplayer)
    {
        var peer = new ENetMultiplayerPeer();
        if (peer.CreateClient(ip, port) == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            Multiplayer.ConnectedToServer += OnConnectedToServer;
            Multiplayer.ConnectionFailed += OnConnectionFailed;
            Multiplayer.ServerDisconnected += OnServerDisconnected;
            GD.Print("正在尝试连接到服务器...");
        }
        else
        {
            GD.Print("客户端创建失败。");
        }
    }

    public override void EnterRoom()
    {
        throw new System.NotImplementedException();
    }
    private void OnConnectedToServer()
    {
        GD.Print("成功连接到服务器。");
        // 连接成功后，可以在这里进行一些初始化操作
    }
    private void OnConnectionFailed()
    {
        GD.Print("连接到服务器失败。");
        // 可以在这里进行重连或其他错误处理
    }
    private void OnServerDisconnected()
    {
        GD.Print("与服务器断开连接。");
        NetManager.Instance.GetTree().Quit();
    }
}