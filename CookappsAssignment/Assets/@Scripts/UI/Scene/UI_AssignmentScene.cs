using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct HeroUIInfo
{
    public Hero hero;
    public int level;
    public int maxHp;
    public int hp;
    public int maxExp;
    public int exp;
    public int atk;
}

public struct UIEvent_HeroSlot
{
    public HeroUIInfo EventHeroUIInfo;
    public UIEvent_HeroSlot(HeroUIInfo heroUIInfo)
    {
        EventHeroUIInfo = heroUIInfo;
    }
    static UIEvent_HeroSlot e;
    public static void Trigger(HeroUIInfo heroUIInfo)
    {
        e.EventHeroUIInfo = heroUIInfo;

        EventManager.TriggerEvent(e);
    }
}

public enum UIEvent_GameScene { GoldChanged, SpawnBoss, StageChanged, HeroRevive }

public class UI_AssignmentScene : UI_Scene, EventListener<UIEvent_HeroSlot>, EventListener<UIEvent_GameScene>
{
    enum Objects
    {
        BossHpBar,
        StatBuffButton
    }


    enum Texts
    {
        GoldValueText,
        StageInfoText,

        // LevelText
        Hero1_LevelText,
        Hero2_LevelText, 
        Hero3_LevelText,
        Hero4_LevelText,

        // AtkStatText
        Hero1_AtkStatText,
        Hero2_AtkStatText,
        Hero3_AtkStatText,
        Hero4_AtkStatText,

        // HpStatText
        Hero1_HpStatText,
        Hero2_HpStatText,
        Hero3_HpStatText,
        Hero4_HpStatText,

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

        // RevivalTimeText
        Hero1_RevivalTimeText,
        Hero2_RevivalTimeText,
        Hero3_RevivalTimeText,
        Hero4_RevivalTimeText,
    }

    enum Images
    {
        GoldImage,

        // RevivalTimeCounter
        Hero1_RevivalTimeCounter,
        Hero2_RevivalTimeCounter,
        Hero3_RevivalTimeCounter,
        Hero4_RevivalTimeCounter,

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

    [HideInInspector] public Hero[] _heroes;

    private AssignmentScene _scene;
    

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        // Bind
        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindSlider(typeof(Sliders));

        return true;
    }

    public void SetInfo(AssignmentScene scene, Hero[] heroes)
    {
        _scene = scene;
        _heroes = heroes;

        for (int i = 0; i < _heroes.Length; i++)
        {
            int idx = i;

            _heroes[idx].TriggerHeroUIInfo();
            _heroes[idx].OnDeadAction += () => StartRevivalCo(idx);
        }

        GetImage(Images.Hero1_Icon).gameObject.BindEvent(() => { _scene.virtualCamera.Follow = _heroes[0].transform; });
        GetImage(Images.Hero2_Icon).gameObject.BindEvent(() => { _scene.virtualCamera.Follow = _heroes[1].transform; });
        GetImage(Images.Hero3_Icon).gameObject.BindEvent(() => { _scene.virtualCamera.Follow = _heroes[2].transform; });
        GetImage(Images.Hero4_Icon).gameObject.BindEvent(() => { _scene.virtualCamera.Follow = _heroes[3].transform; });

        GetImage(Images.Hero1_RevivalTimeCounter).gameObject.SetActive(false);
        GetImage(Images.Hero2_RevivalTimeCounter).gameObject.SetActive(false);
        GetImage(Images.Hero3_RevivalTimeCounter).gameObject.SetActive(false);
        GetImage(Images.Hero4_RevivalTimeCounter).gameObject.SetActive(false);

        GetText(Texts.Hero1_RevivalTimeText).text = "";
        GetText(Texts.Hero2_RevivalTimeText).text = "";
        GetText(Texts.Hero3_RevivalTimeText).text = "";
        GetText(Texts.Hero4_RevivalTimeText).text = "";

        GetObject(Objects.BossHpBar).SetActive(false);
        GetObject(Objects.StatBuffButton).BindEvent(OpenStatBuffPopup);
    }

    private void OnEnable()
    {
        EventManager.AddListener<UIEvent_HeroSlot>(this);
        EventManager.AddListener<UIEvent_GameScene>(this);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<UIEvent_HeroSlot>(this);
        EventManager.RemoveListener<UIEvent_GameScene>(this);
    }

    public void OnEvent(UIEvent_HeroSlot eventType)
    {
        for (int i = 0; i < _heroes.Length; i++)
        {
            if (_heroes[i] == eventType.EventHeroUIInfo.hero)
            {
                OnHeroInfoChanged(i, eventType.EventHeroUIInfo);
                break;
            }
        }
    }

    public void OnEvent(UIEvent_GameScene eventType)
    {
        switch(eventType)
        {
            case UIEvent_GameScene.GoldChanged:
                OnGoldChanged();
                break;
            case UIEvent_GameScene.SpawnBoss:
                OnBossSpawned();
                break;
            case UIEvent_GameScene.StageChanged:
                GetText(Texts.StageInfoText).text = $"Stage{Managers.Game.StageLevel}";
                break;
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

    private void OnGoldChanged()
    {

        GetText(Texts.GoldValueText).text = Managers.Game.Gold.ToString();
        GetText(Texts.GoldValueText).transform.DOKill(complete: true);
        GetText(Texts.GoldValueText).transform.DOScale(1.2f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        
        GetImage(Images.GoldImage).rectTransform.DOKill(complete: true);
        GetImage(Images.GoldImage).rectTransform.DOScale(1.2f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutFlash);
    }

    private void OnHero1InfoChanged(HeroUIInfo info)
    {
        if (info.hero == null) return;

        // HeroIcon
        GetImage(Images.Hero1_Icon).sprite = Managers.Resource.Load<Sprite>(info.hero.HeroData.IconPath);

        // LevelText
        GetText(Texts.Hero1_LevelText).text = $"Lv. {info.level}";

        // AtkStatText
        GetText(Texts.Hero1_AtkStatText).text = info.atk.ToString();

        // HpStatText
        GetText(Texts.Hero1_HpStatText).text = info.maxHp.ToString();

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

        // HeroIcon
        GetImage(Images.Hero2_Icon).sprite = Managers.Resource.Load<Sprite>(info.hero.HeroData.IconPath);

        // LevelText
        GetText(Texts.Hero2_LevelText).text = $"Lv. {info.level}";

        // AtkStatText
        GetText(Texts.Hero2_AtkStatText).text = info.atk.ToString();

        // HpStatText
        GetText(Texts.Hero2_HpStatText).text = info.maxHp.ToString();

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

        // HeroIcon
        GetImage(Images.Hero3_Icon).sprite = Managers.Resource.Load<Sprite>(info.hero.HeroData.IconPath);

        // LevelText
        GetText(Texts.Hero3_LevelText).text = $"Lv. {info.level}";

        // AtkStatText
        GetText(Texts.Hero3_AtkStatText).text = info.atk.ToString();

        // HpStatText
        GetText(Texts.Hero3_HpStatText).text = info.maxHp.ToString();

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

        // HeroIcon
        GetImage(Images.Hero4_Icon).sprite = Managers.Resource.Load<Sprite>(info.hero.HeroData.IconPath);

        // LevelText
        GetText(Texts.Hero4_LevelText).text = $"Lv. {info.level}";

        // AtkStatText
        GetText(Texts.Hero4_AtkStatText).text = info.atk.ToString();

        // HpStatText
        GetText(Texts.Hero4_HpStatText).text = info.maxHp.ToString();

        // HpSliderText
        GetText(Texts.Hero4_HpSliderText).text = $"{info.hp} / {info.maxHp}";

        // ExpSliderText
        GetText(Texts.Hero4_ExpSliderText).text = $"{info.exp} / {info.maxExp}";

        // HpSlider
        GetSlider(Sliders.Hero4_HpSlider).value = (float)info.hp / info.maxHp;

        // ExpSlider
        GetSlider(Sliders.Hero4_ExpSlider).value = (float)info.exp / info.maxExp;
    }

    private void StartRevivalCo(int idx)
    {
        StartCoroutine(RevivalCo(idx));
    }

    private IEnumerator RevivalCo(int idx)
    {
        // fillAmount : 1 -> 0
        // TimeText : 5 -> 0

        Images imgEnum = Util.ParseEnum<Images>($"Hero{idx + 1}_RevivalTimeCounter");
        Texts textEnum = Util.ParseEnum<Texts>($"Hero{idx + 1}_RevivalTimeText");

        Image revivalImg = GetImage(imgEnum);
        TMP_Text revivalText = GetText(textEnum);

        float cooldown = Define.HERO_REVIVAL_TIME;
        float currentTime = cooldown;

        revivalImg.gameObject.SetActive(true);
        revivalImg.fillAmount = 1;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            revivalImg.fillAmount = currentTime / cooldown;
            revivalText.text = currentTime.ToString("F2");

            yield return null;
        }

        revivalImg.fillAmount = 0;
        revivalText.text = "";
    }

    private void OnBossSpawned()
    {
        GetObject(Objects.BossHpBar).SetActive(true);
        UI_HPBar bossHpBar = GetObject(Objects.BossHpBar).GetComponent<UI_HPBar>();

        if (bossHpBar == null)
        {
            Debug.LogWarning("BossHpBar 찾을 수 없음!");
            return;
        }


        foreach (var monster in Managers.Object.Monsters)
        {
            if (monster.MonsterType == Define.EMonsterType.Boss)
            {
                bossHpBar.SetInfo(monster);

                monster.OnHpChangedAction -= () => bossHpBar.SetHpRatio(monster.MonsterStats.Hp / monster.MonsterStats.MaxHp.Value);
                monster.OnHpChangedAction += () => bossHpBar.SetHpRatio(monster.MonsterStats.Hp / monster.MonsterStats.MaxHp.Value);

                monster.OnDeadAction -= () => bossHpBar.gameObject.SetActive(false);
                monster.OnDeadAction += () => bossHpBar.gameObject.SetActive(false);
            }
                
        }
    }

    private void OpenStatBuffPopup()
    {
        Managers.UI.ShowPopupUI<UI_StatBuffPopup>();
    }
}
