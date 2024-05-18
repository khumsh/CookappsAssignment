using System;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseObject : InitBase
{
    public Define.EObjectType ObjectType { get; set; }
    public Vector3 Position => transform.position;

    bool _lookLeft = true;
    public bool LookLeft
    {
        get { return _lookLeft; }
        set
        {
            _lookLeft = value;
            //Flip(!value);
        }
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;



        return true;
    }

    public void LookAtTarget(BaseObject target)
    {
        if(target == null)
            return;
        Vector2 dir = target.transform.position - transform.position;
        if (dir.x < 0)
            LookLeft = true;
        else if(dir.x > 0)
            LookLeft = false;
    }

}

