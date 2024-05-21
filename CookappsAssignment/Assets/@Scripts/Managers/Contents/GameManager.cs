using System;
using System.Collections.Generic;
using Data;
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

    public int EnemyKillCount { get; set; } = 0;
    public bool IsGameOver { get; set; } = false;

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

        StageLevel++;
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
}