using Godot;
using System;

public partial class WriteWord : Control
{
    [Export] public LineEdit wordEdit;
    [Export] private WriteBox writeBox;
    private bool isFillInk = false;

    //private int lvId;//0 or 1
    public bool IsFillInk
    {
        get { return isFillInk; }
        set
        {
            wordEdit.Editable = !value;
            isFillInk = value;
        }
    }
    public override void _Ready()
    {
        base._Ready();
        wordEdit.GuiInput += OnInputEvent;
    }

    private void OnInputEvent(InputEvent @event)
    {
        // 处理触摸事件（移动端）
        if (@event is InputEventScreenTouch touchEvent)
        {
            if (touchEvent.Pressed)
            {
                GrillGameManager.Instance.SelectWrite(this);
            }
        }
        // 处理鼠标事件（桌面端）
        else if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
            {
                GrillGameManager.Instance.SelectWrite(this);
            }
        }
    }
    public bool InSelect(string word,int lvId)
    {
        if (writeBox.isFinish || isFillInk)
        {
            return false;
        }
        wordEdit.Text = word;
        writeBox.isFinish = true;
        IsFillInk = true;
        writeBox.lvId = lvId;
        return true;
    }

}
