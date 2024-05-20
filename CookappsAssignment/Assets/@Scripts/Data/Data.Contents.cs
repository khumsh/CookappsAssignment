using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using static Define;

namespace Data
{
    #region HeroData
    [Serializable]
    public class HeroData
    {
        public int DataId;
        public string PrefabPath;
        public float MaxHp;
        public float Atk;
        public float DefaultAtkRange;
        public float DefaultAtkCooltime;
        public float SkillRange;
        public float SkillCooltime;
        public float MoveSpeed;
        public int DefaultSkillId;
        public int SpecialSkillId;
    }

    [Serializable]
    public class HeroDataLoader : ILoader<int, HeroData>
    {
        public List<HeroData> heroes = new List<HeroData>();
        public Dictionary<int, HeroData> MakeDict()
        {
            Dictionary<int, HeroData> dict = new Dictionary<int, HeroData>();
            foreach (HeroData hero in heroes)
                dict.Add(hero.DataId, hero);
            return dict;
        }
    }
    #endregion

    #region MonsterData
    [Serializable]
    public class MonsterData
    {
        public int DataId;
        public string PrefabPath;
        public float MaxHp;
        public float Atk;
        public float AtkCountPerSecond; // 초당 공격 횟수
        public float AtkRange;
        public float MoveSpeed;
    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();
        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
                dict.Add(monster.DataId, monster);
            return dict;
        }
    }
    #endregion
    
    #region ProjectileData
    [Serializable]
    public class ProjectileData
    {
        public int DataId;
        public string PrefabPath;
        public float ProjSpeed;
    }
    [Serializable]
    public class ProjectileDataLoader : ILoader<int, ProjectileData>
    {
        public List<ProjectileData> projectiles = new List<ProjectileData>();

        public Dictionary<int, ProjectileData> MakeDict()
        {
            Dictionary<int, ProjectileData> dict = new Dictionary<int, ProjectileData>();
            foreach (ProjectileData projectile in projectiles)
                dict.Add(projectile.DataId, projectile);
            return dict;
        }
    }
    #endregion

    #region SkillData
    [Serializable]
    public class SkillData
    {
        public int DataId;
        public string SkillName;
        public ESkillType SkillType;
        public float AtkRate;
        public ETargetType TargetType;
        public ESkillTargetSearchType SkillTargetSearchType; // 단일, 범위
        public ETargetSearchResult TargetSearchResult;
        public bool canSelf;
        public List<ESkillEffectType> SkillEffectTypes = new List<ESkillEffectType>(); // 공격, 회복, 스턴
    }
    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();

        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.DataId, skill);
            return dict;
        }
    }
    #endregion

    #region StageData
    [Serializable]
    public class StageData
    {
        public int StageIndex;
        public string PrefabPath;
        public float HeroRespawnTime;
        public float MonsterRespawnTime;
        public int MonsterMaxSpawnOnce;
    }
    [Serializable]
    public class StageDataLoader : ILoader<int, StageData>
    {
        public List<StageData> stages = new List<StageData>();

        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
            foreach (StageData stage in stages)
                dict.Add(stage.StageIndex, stage);
            return dict;
        }
    }
    #endregion
}