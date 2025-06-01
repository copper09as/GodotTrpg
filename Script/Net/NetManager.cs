using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class NetManager : Node
{
    public static NetManager Instance { get; private set; }
    [Export] public LineEdit roomId;
    public NetServe netServe;
    //public Dictionary<int, List<int>> roomDic = new Dictionary<int, List<int>>();

    private NetManager() { }
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            this.QueueFree();
        }
    }
    private void OnEnterRoom()//向服务端发送申请加入房间
    {
        if (!Multiplayer.IsServer())
        {
            int roomid = int.Parse(roomId.Text);
            int peerId = Multiplayer.GetUniqueId();
            RpcId(1, MethodName.EnterRoom, roomid, peerId);
        }
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void EnterRoom(int roomId, int peerId)
    {
        if (RoomManager.Instance.rooms.ContainsKey(roomId))
        {

            if (RoomManager.Instance.rooms[roomId].players.Count >= 4 || RoomManager.Instance.rooms[roomId].start)
            {
                GD.Print(roomId.ToString() + "房间已满");
                return;
            }
        }
        var room = RoomManager.Instance.EnterRoom(roomId, peerId);
        RpcId(peerId, MethodName.SetPlayer, 1,room.hostId == peerId);//分配房主权限
        RpcId(peerId, MethodName.SyncEnterRoom, peerId, roomId);//自己加入自己的房间
        foreach (var existingId in RoomManager.Instance.rooms[roomId].players)
        {
            if (existingId != (int)peerId)
            {
                RpcId(existingId, MethodName.SyncEnterRoom, peerId, roomId);//自己进入别人的房间
                RpcId(peerId, MethodName.SyncEnterRoom, existingId, roomId);//别人进入自己房间
                RpcId(peerId, MethodName.SyncLoadPlayer, existingId);//自己这里加载别人
                RpcId(existingId, MethodName.SyncLoadPlayer, peerId);//别人加载自己
            }
        }
        /*if (RoomManager.Instance.rooms[roomId].players.Count >= 2)
        {
            foreach (var existingId in RoomManager.Instance.rooms[roomId].players)
            {
                RpcId(existingId, MethodName.RoomFill);
            }
        }*/
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="目标玩家"></param>
    /// <param name="目标房间"></param>
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SyncEnterRoom(int peerId, int roomId)
    {
        GD.Print("进入" + roomId + "房间");
        if (peerId == Multiplayer.GetUniqueId())
        {
            GameManager.Instance.roomId = roomId;
            RoomManager.Instance.PlayerEnterRoom();
        }
        RoomManager.Instance.players.Add(peerId);
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }
    
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncLoadPlayer(int peerId)
    {
        //var player = ResManager.Instance.CreateInstance<Player>(StringResource.PlayerPath, this, "Player" + peerId.ToString());
        if (peerId != Multiplayer.GetUniqueId())
        {
            var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, peerId.ToString());
            playerGameManager.player = null;
        }
        var node = this.GetNodeOrNull(peerId.ToString());

    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void ExitRoom(int id,int peerId)
    {
        RoomManager.Instance.servePlayers.Remove((int)id);
        ServeNetServe.GetInstance().LeaveRoom(peerId,false);
         ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }

    /// <summary>
    /// 玩家房间移除目标的玩家，并且删除目标玩家
    /// </summary>
    /// <param name="peerId"></param>
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncLeaveRoom(int peerId)
    {
        RoomManager.Instance.players.Remove(peerId);
        var node = this.GetNodeOrNull(peerId.ToString());
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        if (node != null)
            node.QueueFree();
        else
            GD.Print("转接器已删除");
        GD.Print($"玩家 {peerId} 已从房间 {GameManager.Instance.roomId} 退出");
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void LoadGameManager(int id)
    {
        var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, NetManager.Instance, id.ToString());
        RoomManager.Instance.servePlayers.Add((int)id);
        RpcId(id, MethodName.SyncGameManager, id);
        GD.Print("转接器已生成");
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SyncGameManager(int id)
    {
        // 客户端仅生成本地副本，不设置权限
        var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }
    /*
    public void SetPlayer(int id, bool isHost)
    {
        RpcId(id, MethodName.BecomeHost, id, isHost);
    }*/
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SetPlayer(int id, bool isHost)
    {
        GameManager.Instance.IsHost = isHost;
        GameManager.Instance.EnterRoom();
    }
    /*[Rpc(MultiplayerApi.RpcMode.Authority)]
    public void RoomFill()
    {
        GameManager.Instance.RoomFill();
    }*/
    public void StartGameLocal(int roomId, int offer)
    {
        Instance.RpcId(1, MethodName.StartGame, GameManager.Instance.roomId, offer);
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void StartGame(int roomId, int offer)
    {
        var room = RoomManager.Instance.rooms[roomId];
        if (room.players.Count <= 1)
        {
            return;
        }
        room.start = true;
        foreach (var existingId in room.players)
        {
            RpcId(existingId, MethodName.SyncStartGame, offer);
        }
        
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SyncStartGame(int offer)
    {
        
        ServeEventCenter.TriggerEvent(StringResource.StartGame, offer);
        ServeEventCenter.TriggerEvent(StringResource.StartGame);
        //GameManager.Instance.StartGame(offer);
    }


}
