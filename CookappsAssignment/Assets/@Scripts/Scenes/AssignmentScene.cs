using Cinemachine;
using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class AssignmentScene : BaseScene
{
    public CinemachineVirtualCamera virtualCamera;

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        Managers.Data.Init();

        // Spawn Hero
        {
            Hero knight = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_KNIGHT);
            Hero thief = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_THIEF);
            Hero archer = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_ARCHER);
            Hero priest = Managers.Object.Spawn<Hero>(Util.RandomPointInAnnulus(Vector2.zero, 1, 2), ID_HERO_PRIEST);

            virtualCamera.Follow = knight.transform;
        }
        

        Managers.Game.StageLevel = 1;

        StartCoroutine(StartStageCo(Managers.Game.StageLevel));

        return true;
    }

    public override void Clear()
    {
        
    }

    IEnumerator StartStageCo(int stageIndex)
    {
        StageData stageData = Managers.Data.StageDic[stageIndex];

        var wait = new WaitForSeconds(stageData.MonsterRespawnTime);

        Managers.Game.EnemyKillCount = 0;

        while (true)
        {
            yield return wait;

            int spawnCount = Random.Range(1, stageData.MonsterMaxSpawnOnce + 1);
            
            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 spawnPos = Util.RandomPointInAnnulus(Managers.Object.Heroes.FirstOrDefault().Position, 5, 10);
                Managers.Object.Spawn<Monster>(spawnPos, ID_MONSTER_NORMAL);
            }

            if (Managers.Game.EnemyKillCount >= 5)
            {
                Managers.UI.ShowToast("Stage Clear!");
                break;
            }
        }
    }

}
