using System;
using System.Collections.Generic;
using static Define;

namespace Data
{
    #region HeroData
    [Serializable]
    public class HeroData
    {
        public int DataId;
        public string PrefabName;
        public float MaxHp;
        public float Atk;
        public float DefaultAtkRange;
        public float DefaultAtkCooltime;
        public float SkillRange;
        public float SkillCooltime;
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
        public string PrefabName;
        public float MaxHp;
        public float Atk;
        public float AtkCountPerSecond; // 초당 공격 횟수
        public float AtkRange;
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
        public string PrefabName;
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

    #region StageData
    [Serializable]
    public class StageData
    {
        public int StageIndex;
        public string PrefabName;
        public float HeroRespawnTime;
        public float MonsterRespawnTime;
        public float MonsterMaxSpawnOnce;
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