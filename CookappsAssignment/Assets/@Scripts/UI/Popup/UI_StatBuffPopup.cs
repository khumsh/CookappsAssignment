using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatBuffPopup : UI_Popup
{
    enum Objects
    {
        ClosePopupPanel,
        PopupWindow
    }

    enum Texts
    {
        AtkStatBuffLevelText,
        HpStatBuffLevelText,

        AtkStatValueText,
        HpStatValueText,

        AtkStatBuffPriceText,
        HpStatBuffPriceText,
    }

    enum Buttons
    {
        AtkStatBuffButton,
        HpStatBuffButton
    }

    GameManager _game;

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _game = Managers.Game;

        GetObject(Objects.ClosePopupPanel).BindEvent(ClosePopupUI);

        GetButton(Buttons.AtkStatBuffButton).gameObject.BindEvent(AtkStatBuffButton);
        GetButton(Buttons.HpStatBuffButton).gameObject.BindEvent(HpStatBuffButton);

        Refresh();

        return true;
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject(Objects.PopupWindow));
    }

    public void Refresh()
    {
        GetText(Texts.AtkStatBuffLevelText).text = $"Lv.{_game.AtkStatBuffLevel}";
        GetText(Texts.HpStatBuffLevelText).text = $"Lv.{_game.HpStatBuffLevel}";

        GetText(Texts.AtkStatValueText).text = $"+{_game.AtkStatBuffPercentAmount * 100:F3}%";
        GetText(Texts.HpStatValueText).text = $"+{_game.HpStatBuffPercentAmount * 100:F3}%";

        GetText(Texts.AtkStatBuffPriceText).text = $"{_game.AtkStatBuffPrice}";
        GetText(Texts.HpStatBuffPriceText).text = $"{_game.HpStatBuffPrice}";
    }

    public void AtkStatBuffButton()
    {
        if (!_game.GoldCheckAndPay(_game.AtkStatBuffPrice)) 
            return;

        _game.AtkStatBuffLevel++;
        Refresh();
    }

    public void HpStatBuffButton()
    {
        if (!_game.GoldCheckAndPay(_game.HpStatBuffPrice))
            return;

        _game.HpStatBuffLevel++;
        Refresh();
    }
}
