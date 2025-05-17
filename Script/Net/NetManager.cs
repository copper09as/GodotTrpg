using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class NetManager : Node
{
    public static NetManager Instance { get; private set; }
    [Export] public Button serBtn;
    [Export] public Button cliBtn;
    [Export] public LineEdit roomId;
    [Export] public Button enterBtn;
    public Dictionary<int, GameManager> playerDic = new Dictionary<int, GameManager>();
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
        serBtn.Pressed += OnServeBtnPress;
        cliBtn.Pressed += OnClientBtnPress;
        enterBtn.Pressed += OnEnterBtnPress;
        // 监听连接成功和失败事件
        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
        Multiplayer.ServerDisconnected += OnServerDisconnected;
    }
    private void OnEnterBtnPress()//向服务端发送申请加入房间
    {
        if (!Multiplayer.IsServer())
        {
            int roomid = int.Parse(roomId.Text);
            int peerId = Multiplayer.GetUniqueId();
            RpcId(1, MethodName.EnterRoom, roomid, peerId);
        }
    }
    public void OnServeBtnPress()
    {
        var peer = new ENetMultiplayerPeer();
        if (peer.CreateServer(8888, 10) == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            Multiplayer.PeerConnected += OnPlayerConnected;
            Multiplayer.PeerDisconnected += OnPlayerDisconnected;
            GD.Print("服务器已成功创建并运行。");
        }
        else
        {
            GD.Print("服务器创建失败。");
        }
    }
    public void OnClientBtnPress()
    {
        var peer = new ENetMultiplayerPeer();
        if (peer.CreateClient("127.0.0.1", 8888) == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            GD.Print("正在尝试连接到服务器...");
        }
        else
        {
            GD.Print("客户端创建失败。");
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void EnterRoom(int roomId, int peerId)
    {
        GD.Print(roomId + "房间");
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
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void LeaveRoom(int peerId)
    {

    }
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="目标玩家"></param>
    /// <param name="目标房间"></param>
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    public void SyncEnterRoom(int peerId, int roomId)
    {
        GD.Print(roomId);
        if (peerId == Multiplayer.GetUniqueId())
        {
            GameManager.Instance.roomId = roomId;
        }
        RoomManager.Instance.players.Add(peerId);
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncLoadPlayer(int peerId)
    {
        var player = ResManager.Instance.CreateInstance<Player>(StringResource.PlayerPath, this, "Player" + peerId.ToString());
        if (peerId != Multiplayer.GetUniqueId())
        {
            var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, peerId.ToString());
            playerDic[peerId] = playerGameManager;
            playerGameManager.player = player;
        }
        player.GetParent().RemoveChild(player);
        var node = this.GetNodeOrNull(peerId.ToString());
        node.AddChild(player);
        
    }
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
    private void OnPlayerConnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
            playerDic[(int)id] = playerGameManager;
            RpcId(id, MethodName.SyncGameManager, (int)id);
            GD.Print(id);
        }
    }
    private void OnPlayerDisconnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            int roomId = RoomManager.Instance.LeaveRoom((int)id);
            if (RoomManager.Instance.rooms.ContainsKey(roomId))
            {
                foreach (var existingId in RoomManager.Instance.rooms[roomId].players)
                {
                    RpcId(existingId, MethodName.SyncLeaveRoom, id);
                }
            }
            var node = this.GetNodeOrNull(id.ToString());
            node.QueueFree();
        }
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncLeaveRoom(int peerId)
    {
        var node = this.GetNodeOrNull(peerId.ToString());
        node.QueueFree();
        GD.Print($"玩家 {peerId} 已从房间 {GameManager.Instance.roomId} 退出");
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncGameManager(int id)
    {
        // 客户端仅生成本地副本，不设置权限
        var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
        playerDic[(int)id] = playerGameManager;
    }



}
