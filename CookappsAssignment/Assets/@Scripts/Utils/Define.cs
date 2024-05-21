using System;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{
    #region HARD CODING
    // TEMP 하드 코딩
    public static int ID_HERO_KNIGHT = 2001;
    public static int ID_HERO_THIEF = 2002;
    public static int ID_HERO_ARCHER = 2003;
    public static int ID_HERO_PRIEST = 2004;
    public static int ID_MONSTER_NORMAL = 5001;
    public static int ID_MONSTER_BOSS = 5002;

    public static float DETECT_RANGE_MONSTER = 6f; // 과제 파일에 없어서 임의로 지정

    public static float STAT_BONUS_MULT = 0.5f;

    public static float HERO_REVIVAL_TIME = 5;

    public static float MONSTER_KILL_EXP = 10;
    public static float MAX_EXP_DEFAULT = 30;
    public static float MAX_EXP_BONUS_MULT = 0.3f; // 레벨당 최대 경험치 30% 증가

    public static int MONSTER_DROP_GOLD = 5;
    #endregion


    #region Enum

    public enum ECreatureState
    {
        Idle,
        Move,
        Atk,
        Dead,
        Stun
    }

    public enum EMoveState
    {
        Patrol,
        Chase
    }

    public enum EAtkState
    {
        DefaultAtk,
        Skill
    }

    public enum ESkillState
    {
        Ready,
        Use,
        Cooldown
    }

    public enum EObjectType
    {
        Creature,
        Hero,
        Monster,
        Projectile
    }

    public enum ETargetType
    {
        Hero,
        Monster,
    }

    public enum EScene
    {
        Unknown,
        AssignmentScene,
    }

    public enum EUIEvent
    {
        Click,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag
    }

    public enum EDamageResult
    {
        None,
        Hit,
        CriticalHit,
        Miss,
        Heal,
        CriticalHeal
    }

    public enum ETargetSearchResult
    {
        None,
        Closest,
        MinHp,
        MinHpRatio
    }

    public enum EStatModType
    {
        Add, // 그냥 합산
        PercentAdd,
        PercentMult,
    }

    public enum ESkillType
    {
        Default,
        Special
    }

    public enum ESkillTargetSearchType
    {
        Single,
        Range
    }

    public enum ESkillEffectType
    {
        DealDamage,
        Heal,
        Stun
    }

    public enum EMonsterType
    {
        Normal,
        Boss
    }

    #endregion

}

public static class SortingLayers
{
    public const int SPELL_INDICATOR = 200;
    public const int HERO = 300;
    public const int NPC = 300;
    public const int MONSTER = 300;
    public const int BOSS = 300;
    public const int GATHERING_RESOURCES = 300;
    public const int PROJECTILE = 310;
    public const int DROP_ITEM = 310;
    public const int SKILL_EFFECT = 315;
    public const int DAMAGE_FONT = 410;
}

public static class AnimName
{
    public const string ATTACK = "attack";
    public const string IDLE = "idle";
    public const string MOVE = "move";
    public const string DAMAGED = "hit";
    public const string DEAD = "dead";
    public const string EVENT_ATTACK_A ="event_attack";
    public const string EVENT_ATTACK_B ="event_attack";
    public const string EVENT_SKILL_A = "event_attack";    
    public const string EVENT_SKILL_B = "event_attack";
}

