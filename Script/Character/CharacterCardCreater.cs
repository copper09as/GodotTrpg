using Godot;
using System;

public partial class CharacterCardCreater : Node
{
    private CharacterData player;
    public static CharacterCardCreater Instance;
    public override void _Ready()
    {
        base._Ready();
        player = new CharacterData();
        if (Instance == null)
            Instance = this;
        else
            this.QueueFree();
    }
    public CharacterCardCreater CreateStr(int str)
    {
        player.Str = str;
        return this;
    }

    public CharacterCardCreater CreateCon(int con)
    {
        player.Con = con;
        return this;
    }

    public CharacterCardCreater CreateSiz(int siz)
    {
        player.Siz = siz;
        return this;
    }

    public CharacterCardCreater CreateDex(int dex)
    {
        player.Dex = dex;
        return this;
    }

    public CharacterCardCreater CreateApp(int app)
    {
        player.App = app;
        return this;
    }

    public CharacterCardCreater CreateWisdom(int wisdom)
    {
        player.Wisdom = wisdom;
        return this;
    }

    public CharacterCardCreater CreatePow(int pow)
    {
        player.Pow = pow;
        return this;
    }

    public CharacterCardCreater CreateEdu(int edu)
    {
        player.Edu = edu;
        return this;
    }

    public CharacterCardCreater CreateLucky(int lucky)
    {
        player.Lucky = lucky;
        return this;
    }

    public CharacterCardCreater CreateAge(int age)
    {
        player.Age = age;
        return this;
    }
    public CharacterData GetPlayerData() => this.player;
    

}
