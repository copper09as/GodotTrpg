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
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="目标玩家"></param>
    /// <param name="目标房间"></param>
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SyncEnterRoom(int peerId, int roomId)
    {
        GD.Print("进入"+roomId + "房间");
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
    #region 已由桥接模式移植
    /*
    private void OnConnectedToServer()
    {
        GD.Print("成功连接到服务器。");
        // 连接成功后，可以在这里进行一些初始化操作
    }
    private void OnConnectionFailed()
    {
        GD.Print("连接到服务器失败。");
        // 可以在这里进行重连或其他错误处理
    }
    private void OnServerDisconnected()
    {
        GD.Print("与服务器断开连接。");
        GetTree().Quit();
    }
    
    /*private void OnPlayerConnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
            RpcId(id, MethodName.SyncGameManager, (int)id);
            RoomManager.Instance.servePlayers.Add((int)id);
            GD.Print(id);
            ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        }
    }*/
    /*private void OnPlayerDisconnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            RoomManager.Instance.servePlayers.Remove((int)id);
            LeaveRoom(id);
            ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
        }
    }*/
    /*private void LeaveRoom(long id)
    {
        int roomId = RoomManager.Instance.LeaveRoom((int)id);
        if (RoomManager.Instance.rooms.ContainsKey(roomId))//如果服务端房间存在
        {
            foreach (var existingId in RoomManager.Instance.rooms[roomId].players)
            {
                RpcId(existingId, MethodName.SyncLeaveRoom, id);
            }
        }
        var node = this.GetNodeOrNull(id.ToString());
        node.QueueFree();
    }*/
#endregion
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
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SyncGameManager(int id)
    {
        // 客户端仅生成本地副本，不设置权限
        var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
        ServeEventCenter.TriggerEvent(StringResource.UpdateUi);
    }



}
