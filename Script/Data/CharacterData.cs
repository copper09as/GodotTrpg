using Godot;
using System;

public class CharacterData
{
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
            else if (value < 15)
                str = 15;
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
            else if (value < 15)
                con = 15;
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
            else if (value < 40)
                siz = 40;
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
            else if (value < 15)
                dex = 15;
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
            else if (value < 15)
                app = 15;
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
            else if (value < 40)
                wisdom = 40;
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
            else if (value < 15)
                pow = 15;
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
            else if (value < 40)
                edu = 40;
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
            else if (value < 15)
                lucky = 15;
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
            else if (value < 0)
                age = 0;
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
            if (dex < siz && str < siz)
            {
                return 7;
            }
            if (dex > siz && str > siz)
            {
                return 9;
            }
            return 7;
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
}
