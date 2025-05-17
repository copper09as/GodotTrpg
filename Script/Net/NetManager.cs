using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class NetManager : Node
{
    public static NetManager Instance { get; private set; }
    [Export] public Button serBtn;
    [Export] public Button cliBtn;
    [Export] public LineEdit roomId;
    [Export] public Button enterBtn;
    public Dictionary<int, GameManager> playerDic = new Dictionary<int, GameManager>();
    public Dictionary<int, List<int>> roomDic = new Dictionary<int, List<int>>();
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




    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void EnterRoom(int roomId, int peerId)
    {
        GD.Print(roomId + "房间");
        if (roomDic.ContainsKey(roomId))
        {
            roomDic[roomId].Add(peerId);
        }
        else
        {
            roomDic[roomId] = new List<int>();
            roomDic[roomId].Add(peerId);
        }
        ResManager.Instance.CreateInstance<Player>(StringResource.PlayerPath, this, "Player" + peerId.ToString());

        RpcId(peerId, MethodName.SyncEnterRoom, peerId, roomId);
        RpcId(peerId, MethodName.SyncLoadPlayer, peerId);

        foreach (var existingId in roomDic[roomId])
        {
            if (existingId != (int)peerId)
            {
                RpcId(existingId, MethodName.SyncEnterRoom, peerId, roomId);
                RpcId(peerId, MethodName.SyncLoadPlayer, existingId);
                RpcId(existingId, MethodName.SyncLoadPlayer, peerId);
            }
        }
    }
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="目标玩家"></param>
    /// <param name="目标房间"></param>
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncEnterRoom(int peerId, int roomId)
    {
        GD.Print(roomId);
        if (peerId == Multiplayer.GetUniqueId())
        {
            GameManager.Instance.roomId = roomId;
        }
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncLoadPlayer(int peerId) =>
    ResManager.Instance.CreateInstance<Player>(StringResource.PlayerPath, this, "Player" + peerId.ToString());
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
    }
    private void OnPlayerConnected(long id)
    {
        if (Multiplayer.IsServer())
        {
            var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
            playerGameManager.clientId = id;
            playerDic[(int)id] = playerGameManager;
            RpcId(id, MethodName.SyncPlayer, (int)id);
            GD.Print(id);
        }
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncPlayer(int id)
    {
        // 客户端仅生成本地副本，不设置权限
        var playerGameManager = ResManager.Instance.CreateInstance<GameManager>(StringResource.GameManagerPath, this, id.ToString());
        playerGameManager.clientId = id;
        playerDic[(int)id] = playerGameManager;
    }
    public void OnServeBtnPress()
    {
        var peer = new ENetMultiplayerPeer();
        if (peer.CreateServer(8888, 10) == Error.Ok)
        {
            Multiplayer.MultiplayerPeer = peer;
            Multiplayer.PeerConnected += OnPlayerConnected;
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
}
