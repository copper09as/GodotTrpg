using Godot;
using System;

public partial class Player : Sprite2D,IProcess
{
    public override void _Ready()
    {
        base._Ready();
        if (!Multiplayer.IsServer() && Name == "Player" + Multiplayer.GetUniqueId().ToString())
        {
            GameManager.Instance.player = this;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

    }
    public Vector2 MoveInput()
    {
        // 检测输入方向
        Vector2 inputDirection = Vector2.Zero;
        if (Input.IsActionPressed("Up")) inputDirection.Y -= 1;
        if (Input.IsActionPressed("Down")) inputDirection.Y += 1;
        if (Input.IsActionPressed("Left")) inputDirection.X -= 1;
        if (Input.IsActionPressed("Right")) inputDirection.X += 1;
        //RpcId(1, MethodName.ServerMoveRequest, this.Position+ inputDirection * 10, Multiplayer.GetUniqueId(),GameManager.Instance.roomId); // 1 是服务器 Peer ID
        // 发送输入到服务器（仅发送方向，非具体坐标）
        return this.Position + inputDirection * 10;
    }
    public void UpdatePos(Vector2 pos)
    {
        this.Position = pos;
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void ServerMoveRequest(Vector2 position, long senderPeerId, int roomId)
    {
        foreach (var id in RoomManager.Instance.rooms[roomId].players)
        {
            RpcId(id,MethodName.ClientUpdatePos, senderPeerId, position);
        }
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void ClientUpdatePos(long playerId, Vector2 position)
    {
       this.Position = position;
    }

    public void InProcess()
    {
        
    }

}