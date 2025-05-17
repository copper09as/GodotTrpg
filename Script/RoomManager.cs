using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RoomManager : Node
{
    public static RoomManager Instance { get; private set; }
    public Dictionary<int, Room> rooms;
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        rooms = new Dictionary<int, Room>();
    }
    /// <summary>
    /// 请求者加入指定房间
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

}
public class Room()
{
    public int id;
    public string name;
    public List<int> players;
}