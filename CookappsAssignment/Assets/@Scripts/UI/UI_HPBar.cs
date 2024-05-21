using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum Texts
    {
        HpValueText,
    }

    enum Sliders
    {
        BaseHPBar,
        EaseHPBar,
    }

    public Creature Owner;

    Slider baseHpBar;
    Slider easeHPBar;

    private float lerpSpeed = 0.15f;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindSlider(typeof(Sliders));

        baseHpBar = GetSlider((int)Sliders.BaseHPBar);
        easeHPBar = GetSlider((int)Sliders.EaseHPBar);

        return true;
    }

    public void SetInfo(Creature owner)
    {
        if (!_init) Init();

        Owner = owner;

        if (Owner.IsPlayer)
        {
            Hero hero = Owner as Hero;
            if (hero.IsValid()) 
            {
                float ratio = hero.HeroStats.Hp / hero.HeroStats.MaxHp.Value;
                SetHpRatio(ratio);
            }
        }
        else
        {
            Monster monster = Owner as Monster;
            if (monster.IsValid())
            {
                float ratio = monster.MonsterStats.Hp / monster.MonsterStats.MaxHp.Value;
                SetHpRatio(ratio);
            }
        }
        
    }

    public void SetHpRatio(float ratio)
    {
        if (Owner == null) return;

        if (Owner.IsValid())
        {
            if (baseHpBar.value != ratio)
            {
                baseHpBar.value = ratio;
                if (Owner.IsPlayer)
                {
                    Hero hero = Owner as Hero;
                    if (hero.IsValid()) 
                    {
                        GetText((int)Texts.HpValueText).text = hero.HeroStats.Hp.ToString();
                    }
                }
                else
                {
                    Monster monster = Owner as Monster;
                    if (monster.IsValid())
                    {
                        if (monster.MonsterType == Define.EMonsterType.Normal)
                            GetText((int)Texts.HpValueText).text = monster.MonsterStats.Hp.ToString();
                        else if (monster.MonsterType == Define.EMonsterType.Boss)
                            GetText((int)Texts.HpValueText).text = $"{monster.MonsterStats.Hp} / {monster.MonsterStats.MaxHp.Value}";

                    }
                }
                
            }

            if (easeHPBar.value != baseHpBar.value)
            {
                EaseSliderLerp(ratio);
            }
        }


    }

    private void EaseSliderLerp(float ratio)
    {
        if (!Owner.IsValid())
            return;
        if (!gameObject.IsValid())
            return;

        StopAllCoroutines();
        StartCoroutine(EaseHPBarLerpCo(ratio));
    }

    IEnumerator EaseHPBarLerpCo(float ratio)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (easeHPBar.value != ratio)
        {
            easeHPBar.value = Mathf.Lerp(easeHPBar.value, ratio, lerpSpeed);
            yield return wait;
        }
    }
}
