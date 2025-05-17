using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RoomManager : Node
{
    public static RoomManager Instance { get; private set; }
    public Dictionary<int, Room> rooms;
    public List<int> players = new List<int>();

    [Export] public Label count;
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        rooms = new Dictionary<int, Room>();
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        count.Text = players.Count.ToString();
    }

    /// <summary>
    /// 请求者加入指定房间,仅服务端处理
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="peerId"></param>
    public Room EnterRoom(int roomId, int peerId)
    {
        GD.Print(roomId + "房间");
        Room room;
        rooms.TryGetValue(roomId, out room);
        if (room == null)
        {
            room = CreateRoom(roomId, roomId.ToString());
        }
        rooms[roomId] = room;
        room.players.Add(peerId);
        //ResManager.Instance.CreateInstance<Player>(StringResource.PlayerPath, this, "Player" + peerId.ToString());
        return room;
    }
    public bool TryGetRoom(int roomId, out Room room)
    => rooms.TryGetValue(roomId, out room);
    private Room CreateRoom(int roomId, string roomName)
    {
        var room = new Room()
        {
            id = roomId,
            name = roomName,
            players = new List<int>()
        };
        return room;
    }
private void LeaveRoom(int roomId, int peerId)
{
    if (rooms.TryGetValue(roomId, out Room room))
    {
        lock (room.players)
        {
            room.players.Remove(peerId);
            if (room.players.Count == 0)
            {
                rooms.Remove(roomId);
            }
        }
    }
    else
    {
        GD.PrintErr($"房间 {roomId} 不存在，无法移除玩家 {peerId}");
    }
}

}
public class Room()
{
    public int id;
    public string name;
    public List<int> players;
}