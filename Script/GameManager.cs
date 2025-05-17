using Godot;
using System;
using System.Linq;

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
            RpcId(1, MethodName.ServerMoveRequest, roomId);
        }
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]//所有人都可以发送
    private void ServerMoveRequest(int roomId)
    {
        if (RoomManager.Instance == null)
        {
            GD.PrintErr("RoomManager 未初始化");
            return;
        }

        if (!RoomManager.Instance.TryGetRoom(roomId, out var room))
        {
            GD.PrintErr($"房间 {roomId} 不存在");
            return;
        }

        if (room.players == null || room.players.Count == 0)
        {
            GD.Print($"房间 {roomId} 无玩家");
            return;
        }

        // ==== 安全操作 ====
        foreach (var playerId in room.players)
        {
            RpcId(playerId, MethodName.ClientUpdatePos, roomId);
        }
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]//服务器可以发送
    private void ClientUpdatePos(int roomId)
    {
        GD.Print("传输成功" + clientId + "房间号" + roomId);
    }
}
