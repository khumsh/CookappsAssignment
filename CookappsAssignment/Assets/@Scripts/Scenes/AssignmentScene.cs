using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignmentScene : BaseScene
{
    protected override bool Init()
    {
        if (!base.Init())
            return false;

        Managers.Data.Init();

        // 하드 코딩 (기능 구현용 과제이므로...)
        int knightId = 2001;
        int thiefId = 2002;
        int archerId = 2003;
        int priestId = 2004;
        int monsterId = 5001;


        Hero knight = Managers.Object.Spawn<Hero>(Vector3.zero, knightId);
        //Hero thief = Managers.Object.Spawn<Hero>(Vector3.zero, thiefId);
        //Hero archer = Managers.Object.Spawn<Hero>(Vector3.zero, archerId);
        //Hero priest = Managers.Object.Spawn<Hero>(Vector3.zero, priestId);

        Monster monster = Managers.Object.Spawn<Monster>(Vector3.up * 3, monsterId);

        return true;
    }

    public override void Clear()
    {
        
    }
}
