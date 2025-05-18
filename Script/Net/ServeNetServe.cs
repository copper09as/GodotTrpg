using Godot;

public class ServeNetServe : NetServe
{
    public ServeNetServe(MultiplayerApi Multiplayer,int port,int max) : base(Multiplayer)
    {
        var peer = new ENetMultiplayerPeer();
        if (peer.CreateServer(12345, max) == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            Multiplayer.PeerConnected += OnPlayerConnected;
            Multiplayer.PeerDisconnected += OnPlayerDisconnected;
            GD.Print("服务器已成功创建并运行。");
            ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        }
        else
        {
            GD.Print("服务器创建失败。");
        }
    }

    public override void EnterRoom()
    {
        throw new System.NotImplementedException();
    }
    private void OnPlayerConnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, NetManager.Instance, id.ToString());
            NetManager.Instance.RpcId(id, "SyncGameManager", (int)id);
            RoomManager.Instance.servePlayers.Add((int)id);
            GD.Print(id);
            ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        }
    }
    private void OnPlayerDisconnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            RoomManager.Instance.servePlayers.Remove((int)id);
            LeaveRoom(id);
            ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        }
    }
    private void LeaveRoom(long id)
    {
        int roomId = RoomManager.Instance.LeaveRoom((int)id);
        if (RoomManager.Instance.rooms.ContainsKey(roomId))//如果服务端房间存在
        {
            foreach (var existingId in RoomManager.Instance.rooms[roomId].players)
            {
                NetManager.Instance.RpcId(existingId, "SyncLeaveRoom", id);
            }
        }
        var node = NetManager.Instance.GetNodeOrNull(id.ToString());
        node.QueueFree();
    }
}