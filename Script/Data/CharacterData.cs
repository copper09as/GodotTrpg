using Godot;
using System;

public class CharacterData
{
    private string name;
    private int sex;
    private int era;
    private string home;
    private string homeTown;
    private int str;//力量
    private int con;//体质
    private int siz;//体型
    private int dex;//敏捷
    private int app;//外貌
    private int wisdom;//智慧
    private int pow;//意志
    private int edu;//教育
    private int lucky;//幸运
    private int age;//年龄
/*
    public int initialStr { get; set; }
    public int initialCon { get; set; }
    public int initialSiz { get; set; }
    public int initialDex { get; set; }
    public int initialApp { get; set; }
    public int initialWisdom { get; set; }
    public int initialPow { get; set; }
    public int initialEdu { get;set; }
    public int initialLucky { get; set; }

    public void SetOrigin()
    {
        this.initialStr = str;
        this.initialCon = con;
        this.initialSiz = siz;
        this.initialDex = dex;
        this.initialApp = app;
        this.initialWisdom = wisdom;
        this.initialPow = pow;
        this.initialEdu = edu;
        this.initialLucky = lucky;
    }
    */
    public int Str
    {
        get
        {
            return str;
        }
        set
        {
            if (value > 99)
                str = 99;
            else if (value < 1)
                str = 1;
            else
                str = value;
        }
    }

    public int Con
    {
        get
        {
            return con;
        }
        set
        {
            if (value > 99)
                con = 99;
            else if (value < 1)
                con = 1;
            else
                con = value;
        }
    }

    public int Siz
    {
        get
        {
            return siz;
        }
        set
        {
            if (value > 99)
                siz = 99;
            else if (value < 1)
                siz = 1;
            else
                siz = value;
        }
    }

    public int Dex
    {
        get
        {
            return dex;
        }
        set
        {
            if (value > 99)
                dex = 99;
            else if (value < 1)
                dex = 1;
            else
                dex = value;
        }
    }

    public int App
    {
        get
        {
            return app;
        }
        set
        {
            if (value > 99)
                app = 99;
            else if (value < 1)
                app = 1;
            else
                app = value;
        }
    }

    public int Wisdom
    {
        get
        {
            return wisdom;
        }
        set
        {
            if (value > 99)
                wisdom = 99;
            else if (value < 1)
                wisdom = 1;
            else
                wisdom = value;
        }
    }

    public int Pow
    {
        get
        {
            return pow;
        }
        set
        {
            if (value > 99)
                pow = 99;
            else if (value < 1)
                pow = 1;
            else
                pow = value;
        }
    }

    public int Edu
    {
        get
        {
            return edu;
        }
        set
        {
            if (value > 99)
                edu = 99;
            else if (value < 1)
                edu = 1;
            else
                edu = value;
        }
    }

    public int Lucky
    {
        get
        {
            return lucky;
        }
        set
        {
            if (value > 99)
                lucky = 99;
            else if (value < 1)
                lucky = 1;
            else
                lucky = value;
        }
    }

    public int Age
    {
        get
        {
            return age;
        }
        set
        {
            if (value > 120)
                age = 120;
            else if (value < 1)
                age = 1;
            else
                age = value;
        }
    }

    public int ExDamage//伤害加值
    {
        get
        {
            return str + siz;
        }
    }
    public int Physique//体格
    {
        get
        {
            return str + siz;
        }
    }
    public int MaxHp//最大生命
    {
        get
        {
            return (int)((str + siz) / 10);
        }
    }
    public int Speed//速度
    {
        get
        {
            int baseSpeed = 0;
            if (dex < siz && str < siz) baseSpeed = 7;
            else if (dex > siz && str > siz) baseSpeed = 9;
            else baseSpeed = 8;
            if (Age >= 40 && Age <= 49)
            {
                return Math.Max(baseSpeed - 1, 1);
            }
            else if (Age >= 50 && Age <= 59)
            {
                return Math.Max(baseSpeed - 2, 1);
            }
            else if (Age >= 60 && Age <= 69)
            {
                return Math.Max(baseSpeed - 3, 1);
            }
            else if (Age >= 70 && Age <= 79)
            {
                return Math.Max(baseSpeed - 4, 1);
            }
            else if (Age >= 80 && Age <= 89)
            {
                return Math.Max(baseSpeed - 5, 1);
            }
            else
            {
                return baseSpeed;
            }
        }
    }
    public int San//理智
    {
        get
        {
            return pow;
        }
    }
    public int MaxMp//魔法值
    {
        get
        {
            return (int)(pow / 5);
        }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int Sex
    {
        get { return sex; }
        set
        {
            if (value > 2 || value < 0)
            {
                value = 2;
            }
            sex = value;
        }
    }
    public int Era
    {
        get { return era; }
        set { era = value; }
    }
    public string Home
    {
        get { return home; }
        set { home = value; }
    }
    public string HomeTown
    {
        get { return homeTown; }
        set { homeTown = value; }
    }
}
