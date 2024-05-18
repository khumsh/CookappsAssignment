using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using static Define;
using static UnityEngine.EventSystems.EventTrigger;

public class Creature : BaseObject
{
    public ECreatureState CreatureState { get; protected set; }
    public EntityStateMachine StateMachine { get; protected set; }

    public Animator animator;

    public Creature Target { get; set; }
    public bool IsPlayer => this is Hero;

    public event Action onDeath;
    public event Action onDamage;

    protected override bool Init()
    {
        if (!base.Init())
            return false;



        return true;
    }

    private void Update()
    {
        if (StateMachine != null)
            StateMachine.Update();
    }

    public virtual void SetInfo(int templateId)
    {
    }

    protected virtual void StateMachineInit()
    {
        StateMachine = new EntityStateMachine();

        onDeath += () => ChangeState(ECreatureState.Dead);
    }

    public void ChangeState(ECreatureState nextState)
    {
        StateMachine.SetState(nextState.ToString());
    }

    public virtual void OnDamaged(float dmg, Creature attacker, Skill skill)
    {
        Debug.Log($"{name} get damaged by {attacker} {skill.SkillSlot}! ");


    }

    protected virtual void OnDead()
    {

    }

    public Creature GetTargetInRange(Vector3 point, float range, ETargetType targetType)
    {
        Creature target = null;

        int targetLayer;
        if (targetType == ETargetType.Hero)
            targetLayer = LayerMask.GetMask("Hero");
        else
            targetLayer = LayerMask.GetMask("Monster");

        Collider2D targetCollider = Physics2D.OverlapCircle(point, range, targetLayer);
        if (targetCollider != null)
            target = targetCollider.GetComponent<Creature>();

        return target;
    }

    public Creature[] GetTargetsInRange(Vector3 point, float range, ETargetType targetType)
    {
        List<Creature> targets = new List<Creature>();

        int targetLayer;
        if (targetType == ETargetType.Hero)
            targetLayer = LayerMask.GetMask("Hero");
        else
            targetLayer = LayerMask.GetMask("Monster");

        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(point, range, targetLayer);
        if (targetColliders.Length > 0)
        {
            for(int i = 0; i < targetColliders.Length; i++) 
            {
                Creature c = targetColliders[i].GetComponent<Creature>();
                if (c != null)
                    targets.Add(c);
            }
        }

        return targets.ToArray();
    }

    #region Anim
    public bool IsAnimationDone(string key)
    {
        //애니메이션 존재 여부 확인
        if (!animator.HasState(0, Animator.StringToHash(key)))
        {
            Debug.Log($"{gameObject.name} Not Found AnimatorState : {key}");
            return true;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(key)) return false;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f) return false;

        return true;
    }

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName, 0);
    }
    #endregion
}