using Godot;
using System;

public static class CaculateTool
{
    public static int[] CaculateDice(int times, int type)//2,6
    {
        int cap = times * (type - 1) + 1;//计算可能数，例如1D6为1*5+1=6，2D6=2*5+1=11，3D6=3*5+1=16
        int[] ans = new int[cap];
        for (int i = 0; i < cap; i++)
        {
            ans[i] = i + times;
        }
        return ans;
    }
    public static int[] CaculateDice(int times, int type, int ex)
    {
        int cap = times * (type - 1) + 1;
        int[] ans = new int[cap];
        for (int i = 0; i < cap; i++)
        {
            ans[i] = i + times+ex;
        }
        return ans;
    }
    public static int[] ExDamageCaculate(int sum)
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
}
