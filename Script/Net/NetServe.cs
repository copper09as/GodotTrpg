
using Godot;

public abstract class NetServe
{

    protected MultiplayerApi Multiplayer;
    public NetServe(MultiplayerApi Multiplayer)
    {
        this.Multiplayer = Multiplayer;
    }
    public abstract void EnterRoom();


}