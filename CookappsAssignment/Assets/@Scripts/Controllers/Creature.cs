using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using static Define;
using static UnityEngine.EventSystems.EventTrigger;

public class Creature : BaseObject
{
    public ECreatureState CreatureState { get; set; }
    public EntityStateMachine StateMachine { get; protected set; }
    public SkillSystem SkillSystem { get; private set; }

    [Header("Components")]
    public Animator animator;
    public AnimationEventHandler animEvtHandler;
    public Collider2D col2D;
    public SpriteRenderer[] spriteRenderers;
    

    [Header("Other Setting")]
    public UI_HPBar hpBar;
    public Transform view;
    public bool isLeftFacing;

    public event Action OnHpChangedAction;
    public event Action OnDeadAction;

    public Creature Target { get; set; }
    public bool IsPlayer => this is Hero;

    protected override bool Init()
    {
        if (!base.Init())
            return false;

        SkillSystem = gameObject.GetComponent<SkillSystem>();

        return true;
    }

    private void Update()
    {
        if (StateMachine != null)
            StateMachine.Update();
    }

    public virtual void SetInfo(int templateId)
    {
        if (hpBar.gameObject.IsValid())
        {
            hpBar.SetInfo(this);
        }

        animEvtHandler.SetAllEventNull();
        animEvtHandler.OnAttack += () =>
        {
            Skill skill = SkillSystem.GetUseableSkill();
            if (skill != null)
                skill.UseSkill();
            else
                ChangeState(ECreatureState.Idle);
        };

    }

    protected virtual void StateMachineInit()
    {
        StateMachine = new EntityStateMachine();
    }

    public void ChangeState(ECreatureState nextState)
    {
        StateMachine.SetState(nextState.ToString());
    }

    public virtual void OnHpChanged()
    {
        OnHpChangedAction?.Invoke();
    }

    public virtual void OnDamaged(float dmg, Creature attacker, Skill skill)
    {
        if (skill != null)
            Debug.Log($"{name} get damaged by {attacker}'s {skill.SkillData.SkillType}Skill! ");

        // Show Damage
        {
            EDamageResult damageResult = (dmg > 0) ? EDamageResult.Hit : EDamageResult.Heal;
            Managers.Object.ShowDamageFont(Position + Vector3.up, dmg, transform, damageResult);
        
            switch (damageResult)
            {
                case EDamageResult.Heal:
                    Managers.Resource.Instantiate("Effect/HealEffect", transform);
                    break;
            }
        }

        // Damage Effect
        PlayDamageAnim();
    }

    protected virtual void OnDead()
    {
        OnDeadAction?.Invoke();
    }

    public Creature GetTargetInRange(Vector3 point, float range, ETargetType targetType, ETargetSearchResult targetSearchResult = ETargetSearchResult.Closest, bool canSelf = false)
    {
        Creature[] targets = GetTargetsInRange(point, range, targetType, canSelf);
        if (targets != null && targets.Length > 0)
        {
            switch(targetSearchResult)
            {
                case ETargetSearchResult.Closest:
                    // 가장 가까운 타겟 찾기
                    Creature closestTarget = null;
                    float closestDistSqr = float.MaxValue;
                    foreach (Creature target in targets)
                    {
                        float distSqr = (target.Position - Position).sqrMagnitude;
                        if (distSqr < closestDistSqr)
                        {
                            closestDistSqr = distSqr;
                            closestTarget = target;
                        }
                    }

                    return closestTarget;
                case ETargetSearchResult.MinHp:
                    Creature minHpTarget = null;
                    float minHp = float.MaxValue;
                    foreach (Creature target in targets)
                    {
                        float hp = (targetType == ETargetType.Hero) ?
                            (target as Hero).HeroStats.Hp : (target as Monster).MonsterStats.Hp;
                        
                        if (hp < minHp)
                        {
                            minHpTarget = target;
                            minHp = hp;
                        }
                    }

                    return minHpTarget;
                case ETargetSearchResult.MinHpRatio:
                    Creature minHpRatioTarget = null;
                    float minHpRatio = float.MaxValue;
                    foreach (Creature target in targets)
                    {
                        float hpRatio = (targetType == ETargetType.Hero) ?
                            (target as Hero).HeroStats.Hp / (target as Hero).HeroStats.MaxHp.Value 
                            : (target as Monster).MonsterStats.Hp / (target as Monster).MonsterStats.MaxHp.Value;

                        if (hpRatio < minHpRatio)
                        {
                            minHpTarget = target;
                            minHpRatio = hpRatio;
                        }
                    }

                    return minHpRatioTarget;
            }
            
        }
            

        return null;
    }

    public Creature[] GetTargetsInRange(Vector3 point, float range, ETargetType targetType, bool canSelf = false)
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
                Creature t = targetColliders[i].GetComponent<Creature>();
                if (t != null && Vector3.Distance(Position, t.transform.position) <= range)
                {
                    if (!canSelf && t == this)
                        continue;

                    targets.Add(t);
                }
                    
            }

            return targets.ToArray();
        }

        return null;        
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

    protected void PlayDamageAnim()
    {
        float effectDuration = 0.1f;
        foreach (var sp in spriteRenderers)
        {
            sp.DOKill(complete: true);
            sp.DOColor(Color.red, effectDuration).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash);
        }
    }
    #endregion

    public void Flip()
    {
        if (Target.IsValid())
        {
            Vector3 dir = Target.Position - Position;
            Flip(dir.x > 0);
        }
    }

    public void Flip(bool isRight)
    {
        Vector3 leftVec = (isLeftFacing) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        Vector3 rightVec = (isLeftFacing) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);

        view.localScale = (isRight) ? rightVec : leftVec;
    }
}