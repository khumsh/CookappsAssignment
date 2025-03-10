using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 코루틴 필요한 매니저에서 끌어다 쓰는 용도
/// </summary>
public class CoroutineManager : MonoBehaviour
{
    private static MonoBehaviour monoInstance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(CoroutineManager)}]").AddComponent<CoroutineManager>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }

    public static void StartDelayAction(float delay, Action callback)
    {
        StartCoroutine(WaitCo(delay, callback));
    }

    private static IEnumerator WaitCo(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);

        callback?.Invoke();
    }
}