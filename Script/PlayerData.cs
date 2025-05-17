using Godot;
using System;

public class PlayerData
{
    public int Level;          // 等级（60/70/80/90级）
    public int[] Attributes;   // 筋力/魔力/敏捷/耐力/幸运/其他
    public int Race;          // 种族（枚举类型）
    public int Class;        // 职阶（Saber/Lancer等枚举）
    //public List<Skill> Skills; // 符卡技能（E~EX级）
    //public List<Ultimate> Ultimates; // 必杀宝具
    public int HP, MP;         // 当前生命值与魔力值
    //public int SelfMachine;    // 自机剩余数量
}
