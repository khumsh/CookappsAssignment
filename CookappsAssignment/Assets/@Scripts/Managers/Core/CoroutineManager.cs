using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// �ڷ�ƾ �ʿ��� �Ŵ������� ����� ���� �뵵
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