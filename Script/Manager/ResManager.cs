using Godot;
using System;

public partial class ResManager : Node
{
    public static ResManager Instance { get; private set; }
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
    public T? CreateInstance<T>(string path,Node parent,string name) where T : Node
    {
        // 加载场景资源
        PackedScene packedScene = ResourceLoader.Load<PackedScene>(path);
        if (packedScene == null)
        {
            GD.PrintErr($"Failed to load scene at path: {path}");
            return null;
        }

        // 实例化场景
        T instance = packedScene.Instantiate<T>();
        if (instance == null)
        {
            GD.PrintErr($"Failed to instantiate scene from path: {path}");
            return null;
        }
        instance.Name = name;
        if(parent != null)
            parent.AddChild(instance);
        return instance;
    }
}
