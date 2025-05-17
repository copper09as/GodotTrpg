using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RoomManager : Node
{
    public static RoomManager Instance { get; private set; }
    public HashSet<Room> rooms;
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        rooms = new HashSet<Room>();
    }
    private void EnterRoom(int roomId, int peerId)
    {
        GD.Print(roomId + "房间");
        var room = FindRoom(roomId);
        if (room != null)
        {
            room.players.Add(peerId);
        }
        /*ResManager.Instance.CreateInstance<Player>(StringResource.PlayerPath, this, "Player" + peerId.ToString());

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
        }*/
    }
    private Room CreateRoom(int roomId, string roomName)
    {
        var room = new Room()
        {
            id = roomId,
            name = roomName,
            players = new HashSet<int>()
        };
        return room;
    }
    private Func<int,Room> FindRoom => (roomId)=>rooms.First(i => i.id == roomId);
}
public class Room()
{
    public int id;
    public string name;
    public HashSet<int> players;
}