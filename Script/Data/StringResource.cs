using Godot;
using System;

public static class StringResource
{
    public const string GameManagerPath = "res://Tscn/GameManager.tscn";
    public const string PlayerPath = "res://Tscn/player.tscn";
    public const string UpdateUi = "UpdateUi";
    public const string StartGame = "StartGame";
    public const string MainGame = "res://Tscn/MainGame.tscn";
    public const string ServeMainGame = "res://Tscn/ServeMainGame.tscn";
    //public const string PlayerDataFilePath = "user://player_data.json";
    //public const string PlayerDataFilePath0 = "user://player_data0.json";
    //public const string PlayerDataFilePath1 = "user://player_data1.json";
    //public const string PlayerDataFilePath2 = "user://player_data2.json";
    public const string PlayerOriginDataFilePath = "user://player_origin_data.json";
    public const string CreateCardTscn = "res://Tscn/MainTscn/CreateCharacter.tscn";
    public const string GrillWordTscn = "res://Tscn/GameScene/GrillWord.tscn";
    public const string WordBoxTscn = "res://Tscn/GameScene/WriteBox.tscn";
    public const string TestGameTscn = "res://Tscn/GameScene/GrillGame.tscn";
    public const string HostGameTscn = "res://Tscn/MainTscn/GrillHost.tscn";
    public const string StartTscn = "res://Tscn/StartPanel.tscn";
    public const string ChoseCharacterTsce = "res://Tscn/GameScene/ChoseCharacter.tscn";
    public const string StartGameBtnTscn = "res://Tscn/GameScene/StartGameBtn.tscn";
    public static string GetPlayerSavePath(int index)
    {

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Player index cannot be negative");
        }
        return $"user://player_data_{index}.json";
    }
}
