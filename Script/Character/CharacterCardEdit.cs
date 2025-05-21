
using Godot;
using System;

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
    [Export] private Button createDataBtn;
    public override void _Ready()
    {
        base._Ready();
        createDataBtn.Pressed += OnCretaDataBtnPress;
    }
    private void OnCretaDataBtnPress()
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
        GD.Print("开始建造");
        CharacterData player;
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
        player = CharacterCardCreater.Instance
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
        .GetPlayerData();
        GameDataHandler.Save<CharacterData>(player, StringResource.PlayerDataFilePath);
    }
    private bool TryParse(LineEdit lineEdit) => !string.IsNullOrWhiteSpace(lineEdit.Text);

}

