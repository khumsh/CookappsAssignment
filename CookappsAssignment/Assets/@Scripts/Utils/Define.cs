using System;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class Define
{
    #region Enum

    public enum ECreatureState
    {
        Idle,
        Move,
        Atk,
        Dead
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

    public enum EObjectType
    {
        Creature,
        Hero,
        Monster,
        Projectile
    }

    public enum EScene
    {
        Unknown,
        TestScene,
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

