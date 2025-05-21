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
            if (RoomManager.Instance.rooms[roomId].players.Count >= 4)
            {
                GD.Print(roomId.ToString() + "房间已满");
                return;
            }
        }
        var room = RoomManager.Instance.EnterRoom(roomId, peerId);
        RpcId(peerId, MethodName.SyncEnterRoom, peerId, roomId);//自己加入自己的房间
        RpcId(peerId, MethodName.SyncLoadPlayer, peerId);//自己加载自己
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
        }
        RoomManager.Instance.players.Add(peerId);
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncLoadPlayer(int peerId)
    {
        var player = ResManager.Instance.CreateInstance<Player>(StringResource.PlayerPath, this, "Player" + peerId.ToString());
        if (peerId != Multiplayer.GetUniqueId())
        {
            var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, peerId.ToString());
            playerGameManager.player = player;
        }
        player.GetParent().RemoveChild(player);
        var node = this.GetNodeOrNull(peerId.ToString());
        node.AddChild(player);

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
        node.QueueFree();
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        GD.Print($"玩家 {peerId} 已从房间 {GameManager.Instance.roomId} 退出");
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void LoadGameManager(int id)
    {
        var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, NetManager.Instance, id.ToString());
        RoomManager.Instance.servePlayers.Add((int)id);
        RpcId(id, MethodName.SyncGameManager, id);
        GD.Print(id);
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SyncGameManager(int id)
    {
        // 客户端仅生成本地副本，不设置权限
        var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }



}
