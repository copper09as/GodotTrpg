using Godot;
using System;
using Newtonsoft.Json;

public static class GameDataHandler
{
    public static void Save<T>(T data, string savePath)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        using (var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write))
        {
            file.StoreString(json);
            GD.Print("保存成功：");
        }
    }
    public static T? Load<T>(string loadPath) where T : class
    {

        if (FileAccess.FileExists(loadPath))
        {
            using (var file = FileAccess.Open(loadPath, FileAccess.ModeFlags.Read))
            {
                string json = file.GetAsText();
                T data = JsonConvert.DeserializeObject<T>(json);
                GD.Print("加载成功：");
                return data;
            }
        }
        return null;
    }

}
