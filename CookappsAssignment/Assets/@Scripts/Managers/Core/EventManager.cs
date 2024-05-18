using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;


public struct GameEvent
{
    public string EventName;
    public GameEvent(string newName)
    {
        EventName = newName;
    }
    static GameEvent e;
    public static void Trigger(string newName)
    {
        e.EventName = newName;
        EventManager.TriggerEvent(e);
    }
}

/*
�� �ڵ�� �̺�Ʈ ������ �����ϴ� C# Ŭ������, 
���� ������ ��𼭵� �̺�Ʈ�� Ʈ�����ϰų� �����ϴ� �� ���˴ϴ�.

GameEvent ����ü�� �̺�Ʈ�� �̸��� ��Ÿ���� ���ڿ��� ������, 
Trigger �޼ҵ�� �̺�Ʈ�� Ʈ�����ϰ� EventManager�� TriggerEvent �޼ҵ带 ȣ���մϴ�.

EventManager Ŭ������ �̺�Ʈ�� �����ϴ� AddListener �� RemoveListener �޼ҵ带 �����ϸ�, 
TriggerEvent �޼ҵ�� �̺�Ʈ�� Ʈ�����ϰ� �̺�Ʈ ������ �����ϰ� �ִ� ��� �����ʿ��� �̺�Ʈ�� �����մϴ�.

�� Ŭ������ �������̽� EventListener�� �����ϴ� Ŭ������ ����Ͽ� �̺�Ʈ�� �����ϰ� ó���� �� �ֽ��ϴ�. 
OnEnable �� OnDisable �޼ҵ忡�� �ش� �̺�Ʈ�� �����ϰ� ���� ����ϰ�, OnEvent �޼ҵ忡�� �̺�Ʈ�� ó���մϴ�.

EventManager�� ����Ǵ� ��� ��ũ��Ʈ���� ����� �� �����Ƿ� ���� ������ �̺�Ʈ�� ���� ������ �� �ֽ��ϴ�. 
 */

/// <summary>
/// �� Ŭ������ �̺�Ʈ ������ ó���ϸ�, ���� ������ �̺�Ʈ�� ��ε�ĳ��Ʈ�Ͽ� � Ŭ�������� (�ϳ� �Ǵ� ���� ��) ���𰡰� �Ͼ�ٰ� �˸� �� �ֽ��ϴ�. 
/// �̺�Ʈ�� ����ü�̸�, ���ϴ� ������ �̺�Ʈ�� ������ �� �ֽ��ϴ�. 
/// �� �Ŵ������� GameEvent��� �̺�Ʈ�� ������, �⺻�����δ� ���ڿ��� �̷���� ������ ���ϴ� ��� �� ������ �̺�Ʈ�� ����� ���� �ֽ��ϴ�.
/// 
/// �� �̺�Ʈ�� ��𼭵� Ʈ�����Ϸ��� YOUR_EVENT.Trigger(YOUR_PARAMETERS)�� ����մϴ�. 
/// ���� ��� GameEvent.Trigger("Save");�� Save GameEvent�� Ʈ�����մϴ�.
///
/// EventManager.TriggerEvent(YOUR_EVENT)�� ȣ���� ���� �ֽ��ϴ�. 
/// ���� ��� EventManager.TriggerEvent(new GameEvent("GameStart"));�� GameStart��� �̸��� GameEvent�� ��� �����ʿ��� ��ε�ĳ��Ʈ�մϴ�.
///
/// � Ŭ���������� �̺�Ʈ�� �����Ϸ��� �� ���� �۾��� �����ؾ� �մϴ� :
/// 
/// 1. �ش� ������ �̺�Ʈ�� ���� EventListener �������̽��� �����Ѵٰ� �˷��� �մϴ�. 
/// ���� ��� public class GUIManager : Singleton<GUIManager>, EventListener<GameEvent>�� ���� �ۼ��մϴ�. 
/// �̴� �ϳ� �̻� (�̺�Ʈ ������ �ϳ�)�� �������̽��� ���� �� �ֽ��ϴ�.
/// 
/// 2. OnEnable�� OnDisable���� ���� �̺�Ʈ ������ �����ϰ� �����ؾ� �մϴ�. 
/// ���� ��� ������ ���� �ۼ��մϴ�.
/// void OnEnable() { this.EventStartListening<GameEvent>(); } 
/// void OnDisable() { this.EventStopListening<GameEvent>(); }
/// 
/// 3. �ش� �̺�Ʈ�� ���� EventListener �������̽��� �����ؾ� �մϴ�. 
/// ���� ��� ������ ���� �ۼ��ϸ� ���ӿ��� �߻��� ��� GameEvent�� �����ϰ�,
/// �̸��� GameOver�� ��쿡 ���� ������ ������ �� �ֽ��ϴ�.
/// public void OnEvent(GameEvent gameEvent) 
/// { 
///   if (gameEvent.EventName == "GameOver") 
///     { // DO SOMETHING } 
/// }
/// </summary>
[ExecuteAlways]
public static class EventManager
{
    private static Dictionary<Type, List<EventListenerBase>> _subscribersList;

    static EventManager()
    {
        _subscribersList = new Dictionary<Type, List<EventListenerBase>>();
    }

    /// <summary>
    /// Ư�� �̺�Ʈ�� �����ڸ� �߰��մϴ�.
    /// </summary>
    /// <param name="listener">������.</param>
    /// <typeparam name="EventType">�̺�ƮŸ��</typeparam>
    public static void AddListener<EventType>(EventListener<EventType> listener) where EventType : struct
    {
        Type eventType = typeof(EventType);

        if (!_subscribersList.ContainsKey(eventType))
        {
            _subscribersList[eventType] = new List<EventListenerBase>();
        }

        if (!SubscriptionExists(eventType, listener))
        {
            _subscribersList[eventType].Add(listener);
        }
    }

    /// <summary>
    /// Ư�� �̺�Ʈ���� �����ڸ� �����մϴ�.
    /// </summary>
    /// <param name="listener">������</param>
    /// <typeparam name="EventType">�̺�ƮŸ��</typeparam>
    public static void RemoveListener<EventType>(EventListener<EventType> listener) where EventType : struct
    {
        Type eventType = typeof(EventType);

        if (!_subscribersList.ContainsKey(eventType))
        {
#if EVENTROUTER_THROWEXCEPTIONS
				throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
#else
            return;
#endif
        }

        List<EventListenerBase> subscriberList = _subscribersList[eventType];

#if EVENTROUTER_THROWEXCEPTIONS
	        bool listenerFound = false;
#endif

        for (int i = subscriberList.Count - 1; i >= 0; i--)
        {
            if (subscriberList[i] == listener)
            {
                subscriberList.Remove(subscriberList[i]);
#if EVENTROUTER_THROWEXCEPTIONS
					listenerFound = true;
#endif

                if (subscriberList.Count == 0)
                {
                    _subscribersList.Remove(eventType);
                }

                return;
            }
        }

#if EVENTROUTER_THROWEXCEPTIONS
		    if( !listenerFound )
		    {
				throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		    }
#endif
    }

    /// <summary>
    /// �ش� �̺�Ʈ�� �����ϰ� �ִ� ��� ��ü�� ���� �̺�Ʈ Ʈ���Ÿ� �ߵ��մϴ�. 
    /// </summary>
    /// <param name="newEvent">Ʈ���Ÿ� �ߵ��� �̺�Ʈ</param>
    /// <typeparam name="EventType">�̺�ƮŸ��</typeparam>
    public static void TriggerEvent<EventType>(EventType newEvent) where EventType : struct
    {
        List<EventListenerBase> list;
        if (!_subscribersList.TryGetValue(typeof(EventType), out list))
#if EVENTROUTER_REQUIRELISTENER
			        throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( Event ).ToString() ) );
#else
            return;
#endif

        for (int i = list.Count - 1; i >= 0; i--)
        {
            (list[i] as EventListener<EventType>).OnEvent(newEvent);
        }
    }

    /// <summary>
    /// Ư�� �̺�Ʈ�� ���� �����ڰ� �ִ��� üũ�մϴ�.
    /// </summary>
    /// <returns><c>true</c> �����Ǿ��ִٸ�, <c>false</c> �ƴ϶��.</returns>
    /// <param name="type">Type.</param>
    /// <param name="receiver">Receiver.</param>
    private static bool SubscriptionExists(Type type, EventListenerBase receiver)
    {
        List<EventListenerBase> receivers;

        if (!_subscribersList.TryGetValue(type, out receivers)) return false;

        bool exists = false;

        for (int i = receivers.Count - 1; i >= 0; i--)
        {
            if (receivers[i] == receiver)
            {
                exists = true;
                break;
            }
        }

        return exists;
    }
}

/// <summary>
/// ��� Ŭ������ ���� �̺�Ʈ�� �����ϰų� �����ϰ� ���ִ� ����ƽ Ŭ����
/// </summary>
public static class EventRegister
{
    public delegate void Delegate<T>(T eventType);

    public static void EventStartListening<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
        EventManager.AddListener<EventType>(caller);
    }

    public static void EventStopListening<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
        EventManager.RemoveListener<EventType>(caller);
    }
}

/// <summary>
/// Event listener �⺻ �������̽�
/// </summary>
public interface EventListenerBase { };

/// <summary>
/// ���ϴ� �̺�ƮŸ���� �����ϱ� ���� �ʿ��� �������̽�
/// </summary>
public interface EventListener<T> : EventListenerBase
{
    void OnEvent(T eventType);
}

public class EventListenerWrapper<TOwner, TTarget, TEvent> : EventListener<TEvent>, IDisposable
    where TEvent : struct
{
    private Action<TTarget> _callback;

    private TOwner _owner;
    public EventListenerWrapper(TOwner owner, Action<TTarget> callback)
    {
        _owner = owner;
        _callback = callback;
        RegisterCallbacks(true);
    }

    public void Dispose()
    {
        RegisterCallbacks(false);
        _callback = null;
    }

    protected virtual TTarget OnTEvent(TEvent eventType) => default;
    public void OnEvent(TEvent eventType)
    {
        var item = OnTEvent(eventType);
        _callback?.Invoke(item);
    }

    private void RegisterCallbacks(bool b)
    {
        if (b)
        {
            this.EventStartListening<TEvent>();
        }
        else
        {
            this.EventStopListening<TEvent>();
        }
    }
}
