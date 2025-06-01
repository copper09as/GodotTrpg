using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class GameDataCenter : Node
{
    public static GameDataCenter Instance;
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            this.QueueFree();
        }
        gamePlayerData = new List<GrillPlayerData>();
        UpdatePlayerList();
    }
    [Export] public GrillOfferData currentOfferData;
    [Export] public Godot.Collections.Array<GrillOfferData> gameOfferData;
    public GrillPlayerData currentPlayerData;
    public List<GrillPlayerData> gamePlayerData;
    public void UpdatePlayerList()
    {
        gamePlayerData.Clear();
        GrillPlayerData playerData0 = GameDataHandler.Load<GrillPlayerData>(StringResource.PlayerDataFilePath0);
        GrillPlayerData playerData1 = GameDataHandler.Load<GrillPlayerData>(StringResource.PlayerDataFilePath1);
        GrillPlayerData playerData2 = GameDataHandler.Load<GrillPlayerData>(StringResource.PlayerDataFilePath2);
        if (playerData0 != null)
            gamePlayerData.Add(playerData0);
        if (playerData1 != null)
            gamePlayerData.Add(playerData1);
        if (playerData2 != null)
            gamePlayerData.Add(playerData2);
    }

}
