using System;
using System.Collections;
using Data;
using UnityEngine;

public class Projectile : BaseObject
{
    protected Creature _owner;
    protected ProjectileData _projectileData;
    protected SpriteRenderer _projectileSprite;
    protected Vector3 endPos;
    
    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _projectileSprite.sortingOrder = SortingLayers.PROJECTILE;
        ObjectType = Define.EObjectType.Projectile;
        return true;
    }

    public virtual void SetInfo(int templateId, Creature owner)
    {
        _projectileData = Managers.Data.ProjectileDic[templateId];
        _owner = owner;

        if (gameObject.IsValid())
            StartCoroutine(CoReserveDestroy());
    }


    private IEnumerator CoReserveDestroy()
    {
        yield return new WaitForSeconds(5f);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Managers.Object.DespawnProjectile(this);
    }

}
