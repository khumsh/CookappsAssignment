using System.Collections;
using Data;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    public MonsterData MonsterData;

    protected override bool Init()
    {
        base.Init();
        ObjectType = EObjectType.Monster;
        return true;
    }

    public override void SetInfo(int templateId)
    {
        base.SetInfo(templateId);
        //MonsterData = CreatureData as MonsterData;
    }

    #region AI
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                break;
            case ECreatureState.Atk:
                break;
            case ECreatureState.Move:
                break;
            case ECreatureState.Dead:
                break;
            default:
                break;
        }
    }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();
    }

    protected override void UpdateMove()
    {
        
    }

    protected override void UpdateDead()
    {
        base.UpdateDead();
    }

    #endregion
    
    protected override void OnDead()
    {
        base.OnDead();

        
    }
    
}
