using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 엔티티(플레이어, 적) 상태 관리 머신
/// </summary>
public class EntityStateMachine
{
    public float stateTimer;

    private Dictionary<string, IState> states;
    private IState currentState;

    public EntityStateMachine()
    {
        states = new Dictionary<string, IState>();
    }

    public void AddState(IState state)
    {
        states[state.StateName] = state;
    }

    public void SetState(string stateName)
    {
        if (currentState != null)
            currentState.Exit();

        if (states.TryGetValue(stateName, out var nextState))
        {
            currentState = nextState;
            currentState.Enter();
            stateTimer = 0;
        }
        else
        {
            Debug.LogError($"'{stateName}'상태가 존재하지 않습니다.");
        }
    }

    public string GetCurrentStateName()
    {
        return currentState.StateName;
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
            stateTimer += Time.deltaTime;
        }

    }
}
