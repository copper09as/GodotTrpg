
using Godot;
using System;
using System.Linq;

public partial class CharacterCardEdit : Node
{
    [Export] private LineEdit strTxt;
    [Export] private LineEdit conTxt;
    [Export] private LineEdit sizTxt;
    [Export] private LineEdit dexTxt;
    [Export] private LineEdit appTxt;
    [Export] private LineEdit wisdomTxt;
    [Export] private LineEdit powTxt;
    [Export] private LineEdit eduTxt;
    [Export] private LineEdit luckyTxt;
    [Export] private LineEdit ageTxt;
    [Export] private Label sanTxt;
    [Export] private Label mpTxt;
    [Export] private Label exDmTxt;
    [Export] private Label speedTxt;
    [Export] private Label PhysiqueTxt;
    [Export] private LineEdit nameTxt;
    [Export] private LineEdit homeTxt;
    [Export] private LineEdit homeTownTxt;
    [Export] private OptionButton eraTxt;
    [Export] private OptionButton sexTxt;
    [Export] private Button saveBtn;
    [Export] private Button loadDataBtn;
    [Export] private Button rollBtn;
    private CharacterData player;

    public override void _Ready()
    {
        base._Ready();
        saveBtn.Pressed += OnSaveBtnPress;
        loadDataBtn.Pressed += LoadData;
        rollBtn.Pressed += OnRollBtnPress;
    }
    private void OnSaveBtnPress()
    {
        if (!TryParse(strTxt)) return;
        if (!TryParse(conTxt)) return;
        if (!TryParse(sizTxt)) return;
        if (!TryParse(dexTxt)) return;
        if (!TryParse(appTxt)) return;
        if (!TryParse(wisdomTxt)) return;
        if (!TryParse(powTxt)) return;
        if (!TryParse(eduTxt)) return;
        if (!TryParse(luckyTxt)) return;
        if (!TryParse(ageTxt)) return;
        if (!TryParse(nameTxt)) return;
        if (!TryParse(homeTxt)) return;
        if (!TryParse(homeTownTxt)) return;
        if (!TryParse(sexTxt)) return;
        if (!TryParse(eraTxt)) return;
        GameDataHandler.Save<CharacterData>(player, StringResource.PlayerDataFilePath);
    }
    private CharacterData ReplaceCharacter(CharacterData player)
    {
        int str = int.Parse(strTxt.Text);
        int con = int.Parse(conTxt.Text);
        int siz = int.Parse(sizTxt.Text);
        int dex = int.Parse(dexTxt.Text);
        int app = int.Parse(appTxt.Text);
        int wisdom = int.Parse(wisdomTxt.Text);
        int pow = int.Parse(powTxt.Text);
        int edu = int.Parse(eduTxt.Text);
        int lucky = int.Parse(luckyTxt.Text);
        int age = int.Parse(ageTxt.Text);
        string name = nameTxt.Text;
        string home = homeTxt.Text;
        string homeTown = homeTownTxt.Text;
        int sex = sexTxt.Selected;
        int era = eraTxt.Selected;
        player.Str = str;
        player.Con = con;
        player.Siz = siz;
        player.Dex = dex;
        player.App = app;
        player.Wisdom = wisdom;
        player.Pow = pow;
        player.Edu = edu;
        player.Lucky = lucky;
        player.Age = age;
        player.Name = name;
        player.Home = home;
        player.HomeTown = homeTown;
        player.Sex = sex;
        player.Era = era;
        return player;
    }
    private CharacterData CreateCharacter()
    {
        int str = int.Parse(strTxt.Text);
        int con = int.Parse(conTxt.Text);
        int siz = int.Parse(sizTxt.Text);
        int dex = int.Parse(dexTxt.Text);
        int app = int.Parse(appTxt.Text);
        int wisdom = int.Parse(wisdomTxt.Text);
        int pow = int.Parse(powTxt.Text);
        int edu = int.Parse(eduTxt.Text);
        int lucky = int.Parse(luckyTxt.Text);
        int age = int.Parse(ageTxt.Text);
        string name = nameTxt.Text;
        string home = homeTxt.Text;
        string homeTown = homeTownTxt.Text;
        int sex = sexTxt.Selected;
        int era = eraTxt.Selected;
        var player = CharacterCardCreater.Instance
        .Resert()
        .CreateStr(str)
        .CreateCon(con)
        .CreateSiz(siz)
        .CreateDex(dex)
        .CreateApp(app)
        .CreateWisdom(wisdom)
        .CreatePow(pow)
        .CreateEdu(edu)
        .CreateLucky(lucky)
        .CreateAge(age)
        .CreateName(name)
        .CreateHome(home)
        .CreateHomeTown(homeTown)
        .CreateSex(sex)
        .CreateEra(era)
        .CreateOrigin()
        .GetPlayerData();
        return player;
    }
    private void UpdateTxt(CharacterData player)
    {
        if (player == null)
        {
            GD.PrintErr("注入Player为空");
            return;
        }
        nameTxt.Text = player.Name;
        homeTxt.Text = player.Home;
        homeTownTxt.Text = player.HomeTown;
        sexTxt.Select(player.Sex);
        eraTxt.Select(player.Era);
        strTxt.Text = player.Str.ToString();
        conTxt.Text = player.Con.ToString();
        sizTxt.Text = player.Siz.ToString();
        dexTxt.Text = player.Dex.ToString();
        appTxt.Text = player.App.ToString();
        wisdomTxt.Text = player.Wisdom.ToString();
        powTxt.Text = player.Pow.ToString();
        eduTxt.Text = player.Edu.ToString();
        luckyTxt.Text = player.Lucky.ToString();
        ageTxt.Text = player.Age.ToString();
        mpTxt.Text = player.MaxMp.ToString();
        sanTxt.Text = player.San.ToString();
        exDmTxt.Text =
        CaculateTool.ExDamageCaculate(player.ExDamage).Min().ToString() +
        "——" + CaculateTool.ExDamageCaculate(player.ExDamage).Max();
        speedTxt.Text = player.Speed.ToString();
        PhysiqueTxt.Text = CaculateTool.PhysiqueCaclute(player.Physique).ToString();
    }
    private void LoadData()
    {
        player = GameDataHandler.Load<CharacterData>(StringResource.PlayerDataFilePath);
        if (player != null) UpdateTxt(player);
    }
    private void OnRollBtnPress()
    {
        if (!TryParse(ageTxt)) return;
        if (!TryParse(nameTxt)) return;
        if (!TryParse(homeTxt)) return;
        if (!TryParse(homeTownTxt)) return;
        if (!TryParse(sexTxt)) return;
        if (!TryParse(eraTxt)) return;
        Godot.Collections.Array<int> T3D6 = CaculateTool.CaculateDice(3, 6);
        Godot.Collections.Array<int> T2D6E6 = CaculateTool.CaculateDice(2, 6);
        int str = CaculateTool.Roll(T3D6) * 5;
        int con = CaculateTool.Roll(T3D6) * 5;
        int siz = CaculateTool.Roll(T2D6E6) * 5;
        int dex = CaculateTool.Roll(T3D6) * 5;
        int app = CaculateTool.Roll(T3D6) * 5;
        int wisdom = CaculateTool.Roll(T2D6E6) * 5;
        int pow = CaculateTool.Roll(T3D6) * 5;
        int edu = CaculateTool.Roll(T2D6E6) * 5;
        int lucky = CaculateTool.Roll(T3D6) * 5;
        strTxt.Text = str.ToString();
        conTxt.Text = con.ToString();
        sizTxt.Text = siz.ToString();
        dexTxt.Text = dex.ToString();
        appTxt.Text = app.ToString();
        wisdomTxt.Text = wisdom.ToString();
        powTxt.Text = pow.ToString();
        eduTxt.Text = edu.ToString();
        luckyTxt.Text = lucky.ToString();
        if (FileAccess.FileExists(StringResource.PlayerDataFilePath))
        {
            player = GameDataHandler.Load<CharacterData>(StringResource.PlayerDataFilePath);
            player = ReplaceCharacter(player);
            GD.Print("存在文件");
        }
        else
        {
            player = CreateCharacter();
            GD.Print("不存在文件");
        }
        CaculateTool.AdjustAttributes(player);
        UpdateTxt(player);

    }
    private bool TryParse(LineEdit lineEdit) => !string.IsNullOrWhiteSpace(lineEdit.Text);
    private bool TryParse(OptionButton option) => !(option.Selected == -1);

}

