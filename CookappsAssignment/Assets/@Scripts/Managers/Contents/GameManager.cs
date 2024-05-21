using System;
using System.Collections.Generic;
using Data;
using JetBrains.Annotations;
using UnityEngine;
using static Define;

public class GameManager
{
    public int StageLevel { get; set; } = 1;

    private int _gold;
    public int Gold 
    {
        get { return _gold; }
        set
        {
            _gold = Mathf.Max(0, value);
            EventManager.TriggerEvent(UIEvent_GameScene.GoldChanged);
        }
    }

    private int _atkStatBuffLevel;
    public int AtkStatBuffLevel 
    {
        get { return _atkStatBuffLevel; } 
        set
        {
            _atkStatBuffLevel = value;
            OnAtkBuffLevelChanged?.Invoke();
        }
    }

    private int _hpStatBuffLevel;
    public int HpStatBuffLevel
    {
        get { return _hpStatBuffLevel; }
        set
        {
            _hpStatBuffLevel = value;
            OnHpBuffLevelChanged?.Invoke();
        }
    }

    public int EnemyKillCount { get; set; } = 0;
    public bool IsGameOver { get; set; } = false;

    public Action OnStageClear;
    public Action OnAtkBuffLevelChanged;
    public Action OnHpBuffLevelChanged;

    public float AtkStatBuffPercentAmount => AtkStatBuffLevel * 0.025f;
    public float HpStatBuffPercentAmount => HpStatBuffLevel * 0.01f;
    public int AtkStatBuffPrice => Mathf.RoundToInt(1 + Mathf.Pow(1.2f, AtkStatBuffLevel));
    public int HpStatBuffPrice => Mathf.RoundToInt(1 + Mathf.Pow(1.2f, HpStatBuffLevel));

    public void Init()
    {
        StageLevel = 1;
        EnemyKillCount = 0;
        Gold = 0;
        AtkStatBuffLevel = 1;
        HpStatBuffLevel = 1;
    }



    public void StageClear()
    {
        Managers.UI.ShowToast("Stage Clear!");
        foreach (Monster monster in Managers.Object.Monsters)
        {
            if (monster.MonsterType != EMonsterType.Boss)
                monster.OnDamaged(monster.MonsterStats.MaxHp.Value + 99999, null, null);
        }

        foreach (Hero hero in Managers.Object.Heroes)
        {
            hero.Target = null;
            hero.ChangeState(ECreatureState.Idle);
            hero.PlayAnimation("Victory");
        }

        OnStageClear?.Invoke();
    }

    public void CheckGameOver()
    {
        bool gameOver = true;
        foreach (var hero in Managers.Object.Heroes)
        {
            if (hero.IsValid())
            {
                gameOver = false;
                break;
            }
        }

        if (gameOver)
        {
            IsGameOver = true;
            Managers.UI.ShowToast("Stage Failed...");
        }
    }

    public bool GoldCheckAndPay(int amount)
    {
        if (Gold < amount)
        {
            Managers.UI.ShowToast("Not Enough Gold...");
            return false;
        }

        Gold -= amount;

        return true;
    }
}