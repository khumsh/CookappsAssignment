using Cinemachine;
using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class AssignmentScene : BaseScene
{
    public UI_AssignmentScene ui_scene;
    public CinemachineVirtualCamera virtualCamera;

    int lastValidStageIndex;

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        Managers.Data.Init();
        Managers.Game.Init();
        

        return true;
    }

    private void Start()
    {
        // Spawn Hero
        {
            Hero knight = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_KNIGHT);
            Hero thief = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_THIEF);
            Hero archer = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_ARCHER);
            Hero priest = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_PRIEST);

            ui_scene.SetInfo(this, new Hero[4] { knight, thief, archer, priest });

            virtualCamera.Follow = knight.transform;
            //virtualCamera.Follow = thief.transform;
        }

        Managers.Game.OnStageClear += () => 
        {
            Managers.Game.StageLevel++;
            StartCoroutine(StartStageCo(Managers.Game.StageLevel)); 
        };

        StartCoroutine(StartStageCo(Managers.Game.StageLevel));
    }

    public override void Clear()
    {
        
    }

    IEnumerator StartStageCo(int stageIndex)
    {
        StageData stageData;
        if (Managers.Data.StageDic.ContainsKey(stageIndex))
        {
            stageData = Managers.Data.StageDic[stageIndex];
            lastValidStageIndex = stageIndex;
        }
        else
        {
            stageData = Managers.Data.StageDic[lastValidStageIndex];
        }


        yield return new WaitForSeconds(2f);

        var wait = new WaitForSeconds(stageData.MonsterRespawnTime);

        Managers.Game.EnemyKillCount = 0;

        Managers.UI.ShowToast($"Stage{stageIndex}");
        EventManager.TriggerEvent(UIEvent_GameScene.StageChanged);

        while (true)
        {
            int spawnCount = Random.Range(1, stageData.MonsterMaxSpawnOnce + 1);

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 spawnPos = Util.RandomPointInAnnulus(Managers.Object.Heroes.FirstOrDefault().Position, 5, 10);
                Managers.Object.Spawn<Monster>(spawnPos, ID_MONSTER_NORMAL);
            }

            if (Managers.Game.EnemyKillCount >= stageData.ClearKillCount)
            {
                Managers.UI.ShowToast("Boss Spawned!");
                Vector2 spawnPos = Util.RandomPointInAnnulus(Managers.Object.Heroes.FirstOrDefault().Position, 5, 10);
                Managers.Object.Spawn<Monster>(spawnPos, ID_MONSTER_BOSS);
                EventManager.TriggerEvent(UIEvent_GameScene.SpawnBoss);
                break;
            }

            yield return wait;
        }
    }
}
