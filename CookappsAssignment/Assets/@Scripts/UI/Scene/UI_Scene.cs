﻿public class UI_Scene : UI_Base
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false);
        return true;
    }
}
