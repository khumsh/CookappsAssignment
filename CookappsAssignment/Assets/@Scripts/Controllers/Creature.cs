using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    public ECreatureState CreatureState { get; protected set; }

    public virtual void SetInfo(int templateId)
    {

    }

    #region AI
    protected virtual void UpdateAnimation()
    {
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

    protected virtual void UpdateIdle()
    {
    }

    protected virtual void UpdateMove()
    {

    }

    protected virtual void UpdateDead()
    {
    }

    #endregion

    protected virtual void OnDead()
    {

    }

}