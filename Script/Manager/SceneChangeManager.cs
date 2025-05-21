using Godot;
using System;

public partial class SceneChangeManager : Node
{
    public static SceneChangeManager Instance;
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
    }
    public void ChangeScene(string path)
    {
        GetTree().ChangeSceneToFile(path);
    }

}
