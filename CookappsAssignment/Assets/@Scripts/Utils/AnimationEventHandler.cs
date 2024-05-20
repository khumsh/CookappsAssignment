using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    public event Action OnAttack;
    public event Action OnEvent;
    public event Action OnAnimEnd;

    private void AnimationOnAttackTrigger() => OnAttack?.Invoke();
    private void AnimationOnEventAnimTrigger() => OnEvent?.Invoke();
    private void AnimationOnAnimEndTrigger() => OnAnimEnd?.Invoke();


    public void SetAllEventNull()
    {
        OnAttack = null;
        OnEvent = null;
        OnAnimEnd = null;
    }
}