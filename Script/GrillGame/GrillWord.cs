using Godot;
using System;
using System.Collections.Generic;

public partial class GrillWord : Control
{
    [Export] private Label wordTxt;
    [Export] private TextureRect background;
    [Export] private Texture2D defaultBack;
    [Export] private Texture2D transBack;
    private GrillGameManager grillGameManager;
    [Export] private ShaderMaterial lightningMaterial;
    public Word word;
    private int lvId = 0;
    public int LvId
    {
        get => lvId;
        set
        {
            lvId = value;
            UpdateWord();
        }
    }
    public void Init(string lword, string hword)
    {
        grillGameManager = GrillGameManager.Instance;
        GuiInput += OnInputEvent;
        word = new Word
        {
            lWord = lword,
            hWord = hword
        };
        lightningMaterial.SetShaderParameter("active", false);
        background.Material = lightningMaterial;
        UpdateWord();
    }
    private void UpdateWord()
    {
        if (!IsInstanceValid(wordTxt))
        {
            wordTxt = (Label)GetChild(1);
        }
        wordTxt.Text = GetWord();

        if (IsInstanceValid(lightningMaterial))
        {
            lightningMaterial.SetShaderParameter("active", lvId == 1);
        }

    }
    private void OnInputEvent(InputEvent @event)
    {
        // 处理触摸事件（移动端）
        if (@event is InputEventScreenTouch touchEvent)
        {
            if (touchEvent.Pressed)
            {
                grillGameManager.TransWord(this);
            }
        }
        // 处理鼠标事件（桌面端）
        else if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
            {
                grillGameManager.TransWord(this);
            }

        }
    }
    public void Enter()
    {
        background.Texture = transBack;
        GD.Print(wordTxt.Text + "进入");
    }
    public void Exit()
    {
        background.Texture = defaultBack;
        GD.Print(wordTxt.Text + "退出");
    }
    public string GetWord() => lvId == 0 ? word.lWord : word.hWord;
}
public struct Word
{
    public string lWord;
    public string hWord;
}