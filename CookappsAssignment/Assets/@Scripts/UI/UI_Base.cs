using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : InitBase
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    private void Awake()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TMP_Text>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindToggle(Type type) { Bind<Toggle>(type); }
    protected void BindSlider(Type type) { Bind<Slider>(type); }

    protected T Get<T>(Enum _enum) where T : UnityEngine.Object
    {
        return Get<T>(Convert.ToInt32(_enum));
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(Enum _enum) { return Get<GameObject>(_enum); }
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    protected TMP_Text GetText(Enum _enum) { return Get<TMP_Text>(_enum); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }

    protected Button GetButton(Enum _enum) { return Get<Button>(_enum); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }

    protected Image GetImage(Enum _enum) { return Get<Image>(_enum); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    protected Toggle GetToggle(Enum _enum) { return Get<Toggle>(_enum); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }

    protected Slider GetSlider(Enum _enum) { return Get<Slider>(_enum); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }

    public static void BindEvent(GameObject go, Action action = null, Action<PointerEventData> dragAction = null, Define.EUIEvent type = Define.EUIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.EUIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.EUIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.EUIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
            case Define.EUIEvent.Drag:
                evt.OnDragHandler -= dragAction;
                evt.OnDragHandler += dragAction;
                break;
            case Define.EUIEvent.BeginDrag:
                evt.OnBeginDragHandler -= dragAction;
                evt.OnBeginDragHandler += dragAction;
                break;
            case Define.EUIEvent.EndDrag:
                evt.OnEndDragHandler -= dragAction;
                evt.OnEndDragHandler += dragAction;
                break;
        }
    }

    public void PopupOpenAnimation(GameObject contentObject) // 팝업 오픈 연출
    {
        //연출 후에 수정
        contentObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        contentObject.transform.DOScale(1f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true);
    }
}
