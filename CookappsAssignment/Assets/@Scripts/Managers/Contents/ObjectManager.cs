using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class ObjectManager
{
    public HashSet<Hero> Heroes { get; } = new HashSet<Hero>();
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();
    public HashSet<Projectile> Projectiles { get; } = new HashSet<Projectile>();

    #region Roots
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }
    public Transform HeroRoot { get { return GetRootTransform("@Heroes"); } }
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform ProjectileRoot { get { return GetRootTransform("@Projectiles"); } }
    #endregion

    public ObjectManager()
    {
        Init();
    }

    public void Init()
    {
    }

    public void Clear()
    {
        Heroes.Clear();
        Monsters.Clear();
        Projectiles.Clear();
    }

    public DamageFont ShowDamageFont(Vector2 pos, float damage, Transform parent, EDamageResult result)
    {
        string prefabPath = "UI/DamageFont";

        GameObject go = Managers.Resource.Instantiate(prefabPath, pooling: true);
        DamageFont damageText = go.GetComponent<DamageFont>();
        damageText.SetInfo(pos, damage, parent, result);

        return damageText;
    }

    public FloatingText ShowFloatingText(string msg, Vector2 pos, Color color, float fontSize = 3, Transform parent = null)
    {
        string prefabPath = "UI/FloatingText";

        GameObject go = Managers.Resource.Instantiate(prefabPath, pooling: true);
        go.transform.position = pos;
        FloatingText ft = go.GetOrAddComponent<FloatingText>();
        ft.SetInfo(msg, pos, color, fontSize, parent);

        return ft;
    }

    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseObject
    {
        System.Type type = typeof(T);

        Vector3 spawnPos = position;

        if (type == typeof(Hero))
        {
            var data = Managers.Data.HeroDic[templateID];

            GameObject go = Managers.Resource.Instantiate(data.PrefabPath);
            go.transform.position = spawnPos;
            go.transform.parent = HeroRoot;
            Hero hc = go.GetOrAddComponent<Hero>();
            Heroes.Add(hc);
            hc.SetInfo(templateID);

            return hc as T;
        }
        if (type == typeof(Monster))
        {
            var data = Managers.Data.MonsterDic[templateID];

            GameObject go = Managers.Resource.Instantiate(data.PrefabPath, pooling: true);
            go.transform.position = spawnPos;
            go.transform.parent = MonsterRoot;
            Monster mc = go.GetOrAddComponent<Monster>();
            Monsters.Add(mc);
            mc.SetInfo(templateID);
            return mc as T;
        }

        return null;
    }
    
    public void Despawn<T>(T obj) where T : BaseObject
    {
        System.Type type = typeof(T);

        if (type == typeof(Hero))
        {
            Heroes.Remove(obj as Hero);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(Monster))
        {
            Monsters.Remove(obj as Monster);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }

    public GameObject SpawnProjectile(Vector3 position, string prefabName)
    {
        GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
        go.transform.position = position;
        go.transform.parent = ProjectileRoot;
        return go;
    }

    public void DespawnProjectile(Projectile projectile)
    {
        Projectiles.Remove(projectile);
        Managers.Resource.Destroy(projectile.gameObject);
    }

    private T FindClosestTarget<T>(Creature source, List<T> targets) where T : Creature
    {
        float minDistance = float.MaxValue;
        T closestTarget = null;

        foreach (T target in targets)
        {
            float distance = (source.transform.position - target.transform.position).sqrMagnitude;

            if (!target.IsValid())
            {
                continue;
            }

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }


}