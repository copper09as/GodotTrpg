using Godot;
using System;

public class GrillPlayerData
{
    private string name;
    private int penmanship;
    private int language;
    private int heart;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int Penmanship
    {
        get { return penmanship; }
        set
        {
            if (value < 0 || value >= 3)
            {
                value = 0;
            }
            penmanship = value;
        }
    }
    public int Language
    {
        get { return language; }
        set
        {
            if (value < 0 || value >= 3)
            {
                value = 0;
            }
            language = value;
        }
    }
    public int Heart
    {
        get { return heart; }
        set
        {
            if (value < 0 || value >= 3)
            {
                value = 0;
            }
            heart = value;
        }
    }
}
