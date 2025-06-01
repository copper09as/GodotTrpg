using Godot;
using Godot.Collections;
using System;

public static class CaculateTool
{
    public static Array<int> CaculateDice(int times, int type)//2,6
    {
        int cap = times * (type - 1) + 1;//计算可能数，例如1D6为1*5+1=6，2D6=2*5+1=11，3D6=3*5+1=16
        Array<int> ans = new Array<int>();
        for (int i = 0; i < cap; i++)
        {
            ans.Add(i + times);
        }
        return ans;
    }
    public static Array<int> CaculateDice(int times, int type, int ex)
    {
        int cap = times * (type - 1) + 1;
        Array<int> ans = new Array<int>();
        for (int i = 0; i < cap; i++)
        {
            ans.Add(i + times + ex);
        }
        return ans;
    }
    public static int Roll(Array<int> dice)
    {
        return dice.PickRandom();
    }
    /*
    public static Array<int> ExDamageCaculate(int sum)
    {
        if (sum >= 2 && sum <= 64) return [-2];
        if (sum >= 65 && sum <= 84) return [-1];
        if (sum >= 85 && sum <= 124) return [0];
        if (sum >= 125 && sum <= 164) return CaculateDice(1, 4);
        if (sum >= 165 && sum <= 204) return CaculateDice(1, 6);
        if (sum >= 205 && sum <= 284) return CaculateDice(2, 6);
        if (sum >= 285 && sum <= 364) return CaculateDice(3, 6);
        if (sum >= 365 && sum <= 444) return CaculateDice(4, 6);
        if (sum >= 445 && sum <= 524) return CaculateDice(5, 6);
        int ex = (sum - 525) / 80 + 1;
        int minTimes = Math.Min(5, ex);
        int sumDice = 30 + ex * 6;
        int type = sumDice / minTimes;
        return CaculateDice(minTimes, type);
    }
    
    public static int PhysiqueCaclute(int sum)
    {
        if (sum >= 2 && sum <= 64) return -2;
        if (sum >= 65 && sum <= 84) return -1;
        if (sum >= 85 && sum <= 124) return 0;
        if (sum >= 125 && sum <= 164) return 1;
        if (sum >= 165 && sum <= 204) return 2;
        if (sum >= 205 && sum <= 284) return 3;
        if (sum >= 285 && sum <= 364) return 4;
        if (sum >= 365 && sum <= 444) return 5;
        if (sum >= 445 && sum <= 524) return 6;
        return 7 + (sum - 525) / 80;
    }

    

    #region 用于处理年龄修正AdjustAttributes
    public static void AdjustAttributes(CharacterData character)
    {
        int value = character.Age;
        if (value >= 15 && value <= 19)
        {
            // 减少力量和体质合计5点
            ReduceAttributes(character, 5, "Strength", "Constitution");
            character.Edu -= 5;
        }
        else if (value >= 20 && value <= 39)
        {
            EnhanceEducation(character, 1);
        }
        else if (value >= 40 && value <= 49)
        {
            // 减少力量、体质、敏捷合计5点
            ReduceAttributes(character, 5, "Strength", "Constitution", "Dexterity");
            character.App -= 5;
            // 对教育进行2次增强检定
            EnhanceEducation(character, 2);
        }
        else if (value >= 50 && value <= 59)
        {
            // 减少力量、体质、敏捷合计10点
            ReduceAttributes(character, 10, "Strength", "Constitution", "Dexterity");
            character.App -= 10;
            // 对教育进行3次增强检定
            EnhanceEducation(character, 3);
        }
        else if (value >= 60 && value <= 69)
        {
            // 减少力量、体质、敏捷合计15点
            ReduceAttributes(character, 15, "Strength", "Constitution", "Dexterity");
            character.App -= 15;
            // 对教育进行4次增强检定
            EnhanceEducation(character, 4);
        }
        else if (value >= 70 && value <= 79)
        {
            // 减少力量、体质、敏捷合计20点
            ReduceAttributes(character, 20, "Strength", "Constitution", "Dexterity");
            character.App -= 20;
            // 对教育进行4次增强检定
            EnhanceEducation(character, 4);
        }
        else if (value >= 80 && value <= 89)
        {
            // 减少力量、体质、敏捷合计25点
            ReduceAttributes(character, 25, "Strength", "Constitution", "Dexterity");
            character.App -= 25;
            // 对教育进行4次增强检定
            EnhanceEducation(character, 4);
        }
    }
    private static void ReduceAttributes(CharacterData character, int totalReduction, params string[] attributeNames)
    {
        int[] reductions = new int[attributeNames.Length];
        int remainingReduction = totalReduction;
        Random random = new Random();
        for (int i = 0; i < attributeNames.Length - 1; i++)
        {
            int reduction = random.Next(0, remainingReduction + 1);
            reductions[i] = reduction;
            remainingReduction -= reduction;
        }
        reductions[attributeNames.Length - 1] = remainingReduction;

        for (int i = 0; i < attributeNames.Length; i++)
        {
            switch (attributeNames[i])
            {
                case "Strength":
                    character.Str -=reductions[i];
                    break;
                case "Constitution":
                    character.Con -=reductions[i];
                    break;
                case "Dexterity":
                    character.Dex -=reductions[i];
                    break;
            }
        }
    }
    private static void EnhanceEducation(CharacterData character, int times)
    {
        var diceHunderd = CaculateDice(1, 100);
        var diceTen = CaculateDice(1, 10);
        for (int i = 0; i < times; i++)
        {
            if (diceHunderd.PickRandom() > character.Edu)
            {
                character.Edu += diceTen.PickRandom();
            }
        }

    }
    #endregion
*/
}
