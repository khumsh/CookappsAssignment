using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.MonsterData> MonsterDic { get; private set; } = new Dictionary<int, Data.MonsterData>();
    public Dictionary<int, Data.HeroData> HeroDic { get; private set; } = new Dictionary<int, Data.HeroData>();
    public Dictionary<int, Data.ProjectileData> ProjectileDic { get; private set; } = new Dictionary<int, Data.ProjectileData>();
    public Dictionary<int, Data.StageData> StageDic { get; private set; } = new Dictionary<int, Data.StageData>();
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();


    public void Init()
    {
        MonsterDic = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
        HeroDic = LoadJson<Data.HeroDataLoader, int, Data.HeroData>("HeroData").MakeDict();
        //ProjectileDic = LoadJson<Data.ProjectileDataLoader, int, Data.ProjectileData>("ProjectileData").MakeDict();
        StageDic = LoadJson<Data.StageDataLoader, int, Data.StageData>("StageData").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData").MakeDict();

    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        Debug.Log($"{path}");
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/JsonData/{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
	}
}
