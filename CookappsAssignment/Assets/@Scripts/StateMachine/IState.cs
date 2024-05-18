using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    string StateName { get; }

    void Enter();
    void Update();
    void Exit();
}
