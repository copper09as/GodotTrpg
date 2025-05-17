using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public long clientId;
    public int roomId;
    public Room room;
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionPressed("Up"))
        {
            //RpcId(1, MethodName.ServerMoveRequest,roomId); // 1 是服务器 Peer ID
        }
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]//所有人都可以发送
    private void ServerMoveRequest(int roomId)
    {
        foreach (var i in NetManager.Instance.roomDic[roomId])
        {
           RpcId(i,MethodName.ClientUpdatePos,roomId);
        }

    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]//服务器可以发送
    private void ClientUpdatePos(int roomId)
    {
        GD.Print("传输成功" + clientId);
    }
}
