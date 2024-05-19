using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.TerrainUtils;

public class FloatingText : MonoBehaviour
{
    TextMeshPro _floatingText;

    public void SetInfo(string msg, Vector2 pos, Color color, float fontSize = 3, Transform parent = null)
    {
        _floatingText = GetComponent<TextMeshPro>();
        transform.position = pos;

        _floatingText.text = msg;
        _floatingText.fontSize = fontSize;
        _floatingText.color = color;
        _floatingText.alpha = 1;

        if (parent != null)
        {
            GetComponent<MeshRenderer>().sortingOrder = 322;
        }

        DoAnimation();
    }

    private void DoAnimation()
    {
        Sequence seq = DOTween.Sequence();

        //1. 처음엔 빠르지만 갈수록 천천히 올라간다.
        //2. 서서히 사라진다.

        seq.Append(transform.DOMove(transform.position + Vector3.up * 0.75f, 0.5f).SetEase(Ease.OutQuad))
            .Append(_floatingText.DOFade(0, 0.3f).SetEase(Ease.InQuint))
            .Append(_floatingText.DOFade(0, 1f).SetEase(Ease.InBounce))
            .OnComplete(() =>
            {
                Managers.Resource.Destroy(gameObject);
            });

    }
}
