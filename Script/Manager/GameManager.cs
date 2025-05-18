using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public int roomId;
    public Player player;
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
        if (Multiplayer.IsServer())
            return;
        if (player == null)
            return;
        if (int.Parse(Name) != Multiplayer.GetUniqueId())
            return;
        base._Process(delta);
        var pos = player.MoveInput();
        RpcId(1, MethodName.ServerMoveRequest, pos, Multiplayer.GetUniqueId(), GameManager.Instance.roomId);
    }


    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void ServeSendMsg(long senderPeerId, int roomId, string message)
    {
        foreach (var id in RoomManager.Instance.rooms[roomId].players)
        {
            RpcId(id, MethodName.ClientUpdateMessage, senderPeerId, message);
        }
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void ClientUpdateMessage(long playerId, string message)
    {
        ChatManager.Instance.UpdateMessage(message);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void ServerMoveRequest(Vector2 position, long senderPeerId, int roomId)
    {
        foreach (var id in RoomManager.Instance.rooms[roomId].players)
        {
            RpcId(id, MethodName.ClientUpdatePos, senderPeerId, position);
        }
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void ClientUpdatePos(long playerId, Vector2 position)
    {
        player.UpdatePos(position);
    }
}
