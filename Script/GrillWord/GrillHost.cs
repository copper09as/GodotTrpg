using Godot;
using System;

public partial class GrillHost : Control
{
    public Godot.Collections.Dictionary<int, RichTextLabel> letterDic;

    public static GrillHost Instance;
    [Export] private Godot.Collections.Array<OptionButton> ScoreOption;
    [Export] private Godot.Collections.Array<Label> players;
    [Export] private Godot.Collections.Array<TextureButton> SendBtn;
    [Export] private Godot.Collections.Array<RichTextLabel> letters;
    [Export] private OfferUiManager offerUiManager;
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        SendBtn[0].Pressed += OnSendBtnPress1;
        SendBtn[1].Pressed += OnSendBtnPress2;
        SendBtn[2].Pressed += OnSendBtnPress3;
        letterDic = new Godot.Collections.Dictionary<int, RichTextLabel>();
        try
        {
            for (int i = 0; i < RoomManager.Instance.players.Count - 1; i++)
            {
                int id = RoomManager.Instance.players[i + 1];
                letterDic[id] = letters[i];
                players[i].Text = id.ToString();
            }
        }
        catch
        {
            GD.Print("Can't find RoomManager");
        }

        offerUiManager.UpdateUi(GameDataCenter.Instance.currentOfferData);
    }
    private void OnSendBtnPress1()
    {
        if (ScoreOption[0].Selected != -1 && players[0].Text != string.Empty)
            GameManager.Instance.FinishScore(int.Parse(players[0].Text), ScoreOption[0].Selected);
    }
    private void OnSendBtnPress2()
    {
        if (ScoreOption[1].Selected != -1 && players[1].Text != string.Empty)
            GameManager.Instance.FinishScore(int.Parse(players[1].Text), ScoreOption[1].Selected);
    }
    private void OnSendBtnPress3()
    {
        if (ScoreOption[2].Selected != -1 && players[2].Text != string.Empty)
            GameManager.Instance.FinishScore(int.Parse(players[2].Text), ScoreOption[2].Selected);
    }
    public void ReceiveMessage(int id, string message)
    {
        letterDic[id].Text = message;
    }


}
