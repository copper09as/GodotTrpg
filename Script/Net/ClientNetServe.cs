using System;
using Godot;

public class ClientNetServe : NetServe
{

    private static ClientNetServe instance;
    private ClientNetServe(MultiplayerApi Multiplayer) : base(Multiplayer) { }
    public static ClientNetServe GetInstance(MultiplayerApi Multiplayer, string ip, int port)
    {
        if (ClientNetServe.instance == null)
        {
            instance = new ClientNetServe(NetManager.Instance.Multiplayer);
            Multiplayer.ConnectedToServer += instance.OnConnectedToServer;
            Multiplayer.ConnectionFailed += instance.OnConnectionFailed;
            Multiplayer.ServerDisconnected += instance.OnServerDisconnected;
        }
        var peer = new ENetMultiplayerPeer();
        if (peer.CreateClient(ip, port) == Error.Ok)
        {
            GD.Print("正在尝试连接到服务器...");
            Multiplayer.MultiplayerPeer = peer;
            GD.Print("正在初始化");
        }
        return instance;

    }
    public override void EnterRoom()
    {
        throw new System.NotImplementedException();
    }
    private void OnConnectedToServer()
    {
        GD.Print("成功连接到服务器。");
        SceneChangeManager.Instance.ChangeScene(StringResource.MainGame);
        NetManager.Instance.RpcId(1, "LoadGameManager", Multiplayer.GetUniqueId());
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