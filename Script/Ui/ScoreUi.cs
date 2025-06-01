using Godot;
using System;
using System.Security.AccessControl;

public partial class ScoreUi : Node
{
    [Export] private Label descriptionTxt;
    [Export] private Control containNode;
    public override void _Ready()
    {
        base._Ready();
        containNode.Hide();
    }

    public void UpdateUi(string message)
    {
        containNode.Show();
        descriptionTxt.Text = message;
    }
}
