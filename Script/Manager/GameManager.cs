using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public int roomId;
    public Player player;
    public bool IsHost = false;
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null && !Multiplayer.IsServer())
        {
            Instance = this;
            ServeEventCenter.RegisterEvent(StringResource.StartGame, StartGame);
        }

    }
    public override void _ExitTree()
    {
        base._ExitTree();
        if (Instance == this)
        {
            Instance = null;
            ServeEventCenter.UnregisterEvent(StringResource.StartGame, StartGame);
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
        //if (IsHost) GD.Print("我是房主");
        var pos = player.MoveInput();
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


    public void SendLetter(string message)//本地发送信件
    {
        GD.Print("发送信件");
        GD.Print(message);
        RpcId(1, MethodName.FinishLetter, roomId, Multiplayer.GetUniqueId(), message);

    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void FinishLetter(int roomId, int peerId, string message)//服务端接受并且转发
    {
        RpcId(RoomManager.Instance.rooms[roomId].hostId, MethodName.SyncUpdateLetter, peerId, message);
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncUpdateLetter(int peerId, string message)//收信人接受信件
    {
        GrillHost.Instance.ReceiveMessage(peerId, message);
    }
    public void EnterRoom()
    {
        if (!IsHost)
            ResManager.Instance.CreateInstance<ChoseCharacter>(StringResource.ChoseCharacterTsce, GetNode("/root/MainGame2"), "Chose");
        else
            ResManager.Instance.CreateInstance<StartGameBtn>(StringResource.StartGameBtnTscn, GetNode("/root/MainGame2"), "Start");
    }
    public void StartGame(int offer)
    {
        GameDataCenter.Instance.currentOfferData = GameDataCenter.Instance.gameOfferData[offer];
        if (IsHost)
            ResManager.Instance.CreateInstance<Control>(StringResource.HostGameTscn, GetNode("/root/MainGame2"), "HostGame");
        else
            ResManager.Instance.CreateInstance<Control>(StringResource.TestGameTscn, GetNode("/root/MainGame2"), "GrillGame");

    }
    public void FinishScore(int peerId, int score)
    {
        RpcId(1, MethodName.SendScore, peerId, score);
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void SendScore(int peerId, int score)
    {
        RpcId(peerId, MethodName.SyncUpdateScore, score);
    }
    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void SyncUpdateScore(int score)
    {
        GrillGameManager.Instance.ReceiveLetter(score);
    }
}
