using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class GameDataCenter : Node
{
    public static GameDataCenter Instance;
    [Export] public GrillOfferData currentOfferData;
    [Export] public Godot.Collections.Array<GrillOfferData> gameOfferData;
    public GrillPlayerData currentPlayerData;
    public List<GrillPlayerData> gamePlayerData;
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

    public void UpdatePlayerList()
    {
        gamePlayerData.Clear();

        const int MAX_PLAYERS = 3;

        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            string savePath = StringResource.GetPlayerSavePath(i);
            GrillPlayerData playerData = GameDataHandler.Load<GrillPlayerData>(savePath);

            if (playerData != null)
            {
                gamePlayerData.Add(playerData);
            }
        }
    }

}
