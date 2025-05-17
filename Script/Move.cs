using Godot;
using System;

public partial class Move : Sprite2D
{
    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!Multiplayer.IsServer() && long.Parse(Name) == Multiplayer.GetUniqueId())
        {
            MoveInput();
        }
    }
    private void MoveInput()
    {
        // 检测输入方向
        Vector2 inputDirection = Vector2.Zero;
        if (Input.IsActionPressed("Up")) inputDirection.Y -= 1;
        if (Input.IsActionPressed("Down")) inputDirection.Y += 1;
        if (Input.IsActionPressed("Left")) inputDirection.X -= 1;
        if (Input.IsActionPressed("Right")) inputDirection.X += 1;

        // 发送输入到服务器（仅发送方向，非具体坐标）
        RpcId(1, MethodName.ServerMoveRequest, inputDirection, int.Parse(Name)); // 1 是服务器 Peer ID
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void ServerMoveRequest(Vector2 direction, long senderPeerId)
    {
        var player = NetManager.Instance.playerDic[(int)senderPeerId];
        // 服务器计算新位置
       // Vector2 newPos = player.Position + direction * 10;
        //player.Position = newPos;
        // 广播新位置给所有客户端（包括发送者）
        //Rpc(MethodName.ClientUpdatePos, senderPeerId, newPos);
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void ClientUpdatePos(long playerId, Vector2 position)
    {
       var player = NetManager.Instance.playerDic[(int)playerId];
       // player.Position = position;
    }
}



