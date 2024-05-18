using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    public ECreatureState CreatureState { get; protected set; }
    public EntityStateMachine StateMachine { get; protected set; }

    public SpriteRenderer SpriteRenderer { get; protected set; }
    public Animator Animator { get; protected set; }

    public event Action onDeath;
    public event Action onDamage;

    public virtual void SetInfo(int templateId)
    {

        StateInit();
    }

    protected virtual void StateInit()
    {
        StateMachine = new EntityStateMachine();

        onDeath += () => ChangeState(ECreatureState.Dead);
    }

    public void ChangeState(ECreatureState nextState)
    {
        StateMachine.SetState(nextState.ToString());
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

    #region Anim
    public bool IsAnimationDone(string key)
    {
        //애니메이션 존재 여부 확인
        if (!Animator.HasState(0, Animator.StringToHash(key)))
        {
            Debug.Log($"{gameObject.name} Not Found AnimatorState : {key}");
            return true;
        }

        if (!Animator.GetCurrentAnimatorStateInfo(0).IsName(key)) return false;
        if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f) return false;

        return true;
    }

    public void PlayAnimation(string animationName)
    {
        Animator.Play(animationName, 0);
    }
    #endregion
}