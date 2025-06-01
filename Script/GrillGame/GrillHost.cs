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
        for (int i = 0; i < SendBtn.Count; i++)
        {
            int index = i; // 创建局部变量避免闭包问题
            SendBtn[i].Pressed += () => OnSendBtnPress(index);
        }
        letterDic = new Godot.Collections.Dictionary<int, RichTextLabel>();
        if (RoomManager.Instance != null && RoomManager.Instance.players != null)
        {
            int playerCount = Mathf.Min(RoomManager.Instance.players.Count - 1, players.Count);

            for (int i = 0; i < playerCount; i++)
            {
                int id = RoomManager.Instance.players[i + 1];
                if (i < letters.Count)
                {
                    letterDic[id] = letters[i];
                }
                if (i < players.Count)
                {
                    players[i].Text = id.ToString();
                }
            }
        }
        else
        {
            GD.Print("RoomManager not available or players list missing");
        }

        offerUiManager.UpdateUi(GameDataCenter.Instance.currentOfferData);
    }
    private void OnSendBtnPress(int index)
    {
        if (string.IsNullOrWhiteSpace(letters[index].Text))
        {
            return;
        }
        // 安全检查
            if (index < 0 || index >= players.Count || index >= ScoreOption.Count)
            {
                GD.PushError($"Invalid button index: {index}");
                return;
            }

        // 获取玩家ID文本
        string playerIdText = players[index].Text;

        // 验证输入
        if (string.IsNullOrWhiteSpace(playerIdText))
        {
            GD.Print("Player ID is empty");
            return;
        }

        if (ScoreOption[index].Selected == -1)
        {
            GD.Print("No score option selected");
            return;
        }

        // 安全转换ID
        if (int.TryParse(playerIdText, out int playerId))
        {
            GameManager.Instance?.FinishScore(playerId, ScoreOption[index].Selected);
        }
        else
        {
            GD.PushError($"Invalid player ID format: {playerIdText}");
        }
    }
    public void ReceiveMessage(int id, string message)
    {
        letterDic[id].Text = message;
    }


}
