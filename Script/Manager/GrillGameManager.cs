using Godot;
using System;
using System.Collections.Generic;

public partial class GrillGameManager : Control
{
    private GrillOfferData grillOffer;
    [Export] private VBoxContainer vBoxContainer;
    [Export] private HBoxContainer writeboxContainer;
    [Export] private GrillWord currentWord;
    [Export] private Button LanguageBtn;
    [Export] private OfferUiManager offerUiManager;
    [Export] private PlayerUiManager playerUiManager;
    [Export] private ScoreUi scoreUi;
    [Export] private Label ScoreTxt;
    [Export] private Label LanguageTxt;
    private int languageTimes;
    public int LanguageTimes
    {
        get
        {
            return languageTimes;
        }
        set
        {
            if (value <= 0) value = 0;
            LanguageTxt.Text = "剩余灵感" + value.ToString();
            languageTimes = value;
        }
    }
    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            if (value < 0) value = 0;
            ScoreTxt.Text = "Score:" + value.ToString();
            score = value;
        }
    }
    public static GrillGameManager Instance;
    private GrillPlayerData grillPlayer;
    private List<GrillWord> wordNodes;
    private Godot.Collections.Array<int> dice;
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null)
            Instance = this;
        grillOffer = GameDataCenter.Instance.currentOfferData;
        if (GameDataCenter.Instance.currentPlayerData == null)
        {
            
        }
        else
            grillPlayer = GameDataCenter.Instance.currentPlayerData;
        InitData();
        CreataWord();

    }
    public override void _ExitTree()
    {
        base._ExitTree();
        Instance = null;
    }

    private void InitData()
    {
        dice = CaculateTool.CaculateDice(1, 6);
        wordNodes = new List<GrillWord>();
        LanguageBtn.Pressed += OnLanguageBtnPress;
        Score = 0;
        offerUiManager.UpdateUi(grillOffer);
        playerUiManager.UpdateUi(grillPlayer);
        LanguageTimes = (grillPlayer.Language + 1) * 5;
        grillOffer = GameDataCenter.Instance.currentOfferData;
    }
    private void CreataWord()
    {
        foreach (var i in grillOffer.InkPot)
        {
            var wordNode = ResManager.Instance.CreateInstance<GrillWord>(StringResource.GrillWordTscn, vBoxContainer, "Word");
            wordNode.Init(i.Key, i.Value);
            wordNodes.Add(wordNode);
        }
        for (int i = 0; i < 5; i++)
        {
            var box = new VBoxContainer();
            writeboxContainer.AddChild(box);
            var wordNode = ResManager.Instance.CreateInstance<WriteBox>(StringResource.WordBoxTscn, box, "WordBox");
            wordNode.id = i;
        }
    }
    public void TransWord(GrillWord word)
    {
        if (currentWord != null) currentWord.Exit();
        word.Enter();
        this.currentWord = word;
    }
    public bool SelectWrite(WriteWord writeWord)
    {
        if (currentWord == null)
            return false;
        if (writeWord.InSelect(currentWord.GetWord(), currentWord.LvId))
        {

            if (currentWord.LvId == 1)
                Score += 1;
            currentWord.Hide();
            currentWord.Exit();
            this.currentWord = null;
            foreach (var i in wordNodes)
            {
                i.LvId = 0;
            }
        }
        return true;
    }
    private void OnLanguageBtnPress()
    {
        int value = CaculateTool.Roll(dice);
        if (LanguageTimes == 0)
        {
            return;
        }

        if (value >= 5)
        {
            foreach (var i in wordNodes)
            {
                i.LvId = 1;
            }
            GD.Print(value);
        }
        LanguageTimes -= 1;


    }
    public void RollPenmanship()
    {

        for (int j = 0; j < grillPlayer.Penmanship + 1; j++)
        {
            int value = CaculateTool.Roll(dice);
            GD.Print("书法鉴定中");
            if (value >= 5)
            {
                GD.Print("书法鉴定成功");
                Score += 1;
                break;
            }
        }
    }
    public bool RollHeart()
    {
        for (int j = 0; j < grillPlayer.Heart + 1; j++)
        {
            int value = CaculateTool.Roll(dice);
            GD.Print("心灵鉴定中");
            if (value >= 5)
            {
                GD.Print("心灵鉴定成功");
                return true;
            }
        }
        return false;
    }
    public void ReceiveLetter(int score)
    {
        int sum = score + this.score;
        string message = "";
        if (sum <= 7)
        {
            message = grillOffer.ConsequencesL;
        }
        else if (sum <= 11)
        {
            message = grillOffer.ConsequencesM;
        }
        else if (sum < 15)
        {
            message = grillOffer.ConsequencesH;
        }
        else
        {
            message = grillOffer.ConsequencesB;
        }
        scoreUi.UpdateUi(message);
    }
}
