using Godot;

public class ServeNetServe : NetServe
{
    private static ServeNetServe instance;
    public static ServeNetServe GetInstance(MultiplayerApi Multiplayer, int port, int max)
    {
        if (instance == null)
        {
            instance = new ServeNetServe(NetManager.Instance.Multiplayer, port, max);
            Multiplayer.PeerConnected += instance.OnPlayerConnected;
            Multiplayer.PeerDisconnected += instance.OnPlayerDisconnected;
        }
        var peer = new ENetMultiplayerPeer();
        if (peer.CreateServer(port, max) == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            if (Multiplayer.IsServer())
            {
                SceneChangeManager.Instance.ChangeScene(StringResource.ServeMainGame);
                GD.Print("服务器已成功创建并运行。");
            }
            ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        }
        return instance;
    }
    public static ServeNetServe GetInstance()
    {
        return instance;
    }
    private ServeNetServe(MultiplayerApi Multiplayer, int port, int max) : base(Multiplayer) { }
    public override void EnterRoom()
    {
        throw new System.NotImplementedException();
    }
    private void OnPlayerConnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            GD.Print(id);
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
    public void LeaveRoom(long id,bool isQueue = true)
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
        if(isQueue)
            node.QueueFree();
    }
}