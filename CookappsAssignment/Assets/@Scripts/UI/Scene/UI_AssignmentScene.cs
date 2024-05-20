using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HeroUIInfo
{
    public Hero hero;
    public int level;
    public int maxHp;
    public int hp;
    public int maxExp;
    public int exp;
}

public struct HeroSlotUIEvent
{
    public HeroUIInfo EventHeroUIInfo;
    public HeroSlotUIEvent(HeroUIInfo heroUIInfo)
    {
        EventHeroUIInfo = heroUIInfo;
    }
    static HeroSlotUIEvent e;
    public static void Trigger(HeroUIInfo heroUIInfo)
    {
        e.EventHeroUIInfo = heroUIInfo;

        EventManager.TriggerEvent(e);
    }
}

public class UI_AssignmentScene : UI_Scene, EventListener<HeroSlotUIEvent>
{
    enum Texts
    {
        // LevelText
        Hero1_LevelText,
        Hero2_LevelText, 
        Hero3_LevelText,
        Hero4_LevelText,

        // HpSliderText
        Hero1_HpSliderText,
        Hero2_HpSliderText,
        Hero3_HpSliderText,
        Hero4_HpSliderText,

        // ExpSliderText
        Hero1_ExpSliderText,
        Hero2_ExpSliderText,
        Hero3_ExpSliderText,
        Hero4_ExpSliderText,
    }

    enum Images
    {
        // Icon
        Hero1_Icon,
        Hero2_Icon,
        Hero3_Icon,
        Hero4_Icon,
    }

    enum Sliders
    {
        // HpSlider
        Hero1_HpSlider,
        Hero2_HpSlider,
        Hero3_HpSlider,
        Hero4_HpSlider,

        // ExpSlider
        Hero1_ExpSlider,
        Hero2_ExpSlider,
        Hero3_ExpSlider,
        Hero4_ExpSlider,
    }

    public Hero[] heroes = new Hero[4];

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        // Bind
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindSlider(typeof(Sliders));

        

        return true;
    }

    private void OnEnable()
    {
        EventManager.AddListener(this);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(this);
    }

    public void OnEvent(HeroSlotUIEvent eventType)
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            if (heroes[i] == eventType.EventHeroUIInfo.hero)
            {
                OnHeroInfoChanged(i, eventType.EventHeroUIInfo);
                break;
            }
        }
    }

    private void OnHeroInfoChanged(int idx, HeroUIInfo info)
    {
        switch(idx)
        {
            case 0:
                OnHero1InfoChanged(info);
                break;
            case 1:
                OnHero2InfoChanged(info);
                break;
            case 2:
                OnHero3InfoChanged(info);
                break;
            case 3:
                OnHero4InfoChanged(info);
                break;
        }
    }

    private void OnHero1InfoChanged(HeroUIInfo info)
    {
        if (info.hero == null) return;

        // LevelText
        GetText(Texts.Hero1_LevelText).text = $"Lv. {info.level}";

        // HpSliderText
        GetText(Texts.Hero1_HpSliderText).text = $"{info.hp} / {info.maxHp}";

        // ExpSliderText
        GetText(Texts.Hero1_ExpSliderText).text = $"{info.exp} / {info.maxExp}";

        // HpSlider
        GetSlider(Sliders.Hero1_HpSlider).value = (float)info.hp / info.maxHp;

        // ExpSlider
        GetSlider(Sliders.Hero1_ExpSlider).value = (float)info.exp / info.maxExp;
    }

    private void OnHero2InfoChanged(HeroUIInfo info)
    {
        if (info.hero == null) return;

        // LevelText
        GetText(Texts.Hero2_LevelText).text = $"Lv. {info.level}";

        // HpSliderText
        GetText(Texts.Hero2_HpSliderText).text = $"{info.hp} / {info.maxHp}";

        // ExpSliderText
        GetText(Texts.Hero2_ExpSliderText).text = $"{info.exp} / {info.maxExp}";

        // HpSlider
        GetSlider(Sliders.Hero2_HpSlider).value = (float)info.hp / info.maxHp;

        // ExpSlider
        GetSlider(Sliders.Hero2_ExpSlider).value = (float)info.exp / info.maxExp;
    }

    private void OnHero3InfoChanged(HeroUIInfo info)
    {
        if (info.hero == null) return;

        // LevelText
        GetText(Texts.Hero3_LevelText).text = $"Lv. {info.level}";

        // HpSliderText
        GetText(Texts.Hero3_HpSliderText).text = $"{info.hp} / {info.maxHp}";

        // ExpSliderText
        GetText(Texts.Hero3_ExpSliderText).text = $"{info.exp} / {info.maxExp}";

        // HpSlider
        GetSlider(Sliders.Hero3_HpSlider).value = (float)info.hp / info.maxHp;

        // ExpSlider
        GetSlider(Sliders.Hero3_ExpSlider).value = (float)info.exp / info.maxExp;
    }

    private void OnHero4InfoChanged(HeroUIInfo info)
    {
        if (info.hero == null) return;

        // LevelText
        GetText(Texts.Hero4_LevelText).text = $"Lv. {info.level}";

        // HpSliderText
        GetText(Texts.Hero4_HpSliderText).text = $"{info.hp} / {info.maxHp}";

        // ExpSliderText
        GetText(Texts.Hero4_ExpSliderText).text = $"{info.exp} / {info.maxExp}";

        // HpSlider
        GetSlider(Sliders.Hero4_HpSlider).value = (float)info.hp / info.maxHp;

        // ExpSlider
        GetSlider(Sliders.Hero4_ExpSlider).value = (float)info.exp / info.maxExp;
    }
}
