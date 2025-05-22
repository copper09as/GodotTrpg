using Godot;
using System;

public partial class CharacterCardCreater : Node
{
    private CharacterData player;
    public static CharacterCardCreater Instance;
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null)
            Instance = this;
        else
            this.QueueFree();
    }
    public CharacterCardCreater Resert()
    {
        player = new CharacterData();
        return this;
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
    public CharacterCardCreater CreateName(string name)
    {
        player.Name = name;
        return this;
    }
    public CharacterCardCreater CreateSex(int sex)
    {
        player.Sex = sex;
        return this;
    }

    public CharacterCardCreater CreateEra(int era)
    {
        player.Era = era;
        return this;
    }

    public CharacterCardCreater CreateHome(string home)
    {
        player.Home = home;
        return this;
    }

    public CharacterCardCreater CreateHomeTown(string homeTown)
    {
        player.HomeTown = homeTown;
        return this;
    }
    /*public CharacterCardCreater CreateOrigin()
    {
        player.SetOrigin();
        return this;
    }*/
    public CharacterData GetPlayerData()
    {
        return player;
    }


}
