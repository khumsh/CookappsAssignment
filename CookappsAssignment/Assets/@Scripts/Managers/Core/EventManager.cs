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
이 코드는 이벤트 관리를 수행하는 C# 클래스로, 
게임 내에서 어디서든 이벤트를 트리거하거나 구독하는 데 사용됩니다.

GameEvent 구조체는 이벤트의 이름을 나타내는 문자열을 가지며, 
Trigger 메소드는 이벤트를 트리거하고 EventManager의 TriggerEvent 메소드를 호출합니다.

EventManager 클래스는 이벤트를 구독하는 AddListener 및 RemoveListener 메소드를 제공하며, 
TriggerEvent 메소드는 이벤트를 트리거하고 이벤트 유형을 구독하고 있는 모든 리스너에게 이벤트를 전달합니다.

이 클래스는 인터페이스 EventListener를 구현하는 클래스를 사용하여 이벤트를 수신하고 처리할 수 있습니다. 
OnEnable 및 OnDisable 메소드에서 해당 이벤트를 구독하고 구독 취소하고, OnEvent 메소드에서 이벤트를 처리합니다.

EventManager는 실행되는 모든 스크립트에서 사용할 수 있으므로 게임 내에서 이벤트를 쉽게 관리할 수 있습니다. 
 */

/// <summary>
/// 이 클래스는 이벤트 관리를 처리하며, 게임 내에서 이벤트를 브로드캐스트하여 어떤 클래스에게 (하나 또는 여러 개) 무언가가 일어났다고 알릴 수 있습니다. 
/// 이벤트는 구조체이며, 원하는 종류의 이벤트를 정의할 수 있습니다. 
/// 이 매니저에는 GameEvent라는 이벤트가 있으며, 기본적으로는 문자열로 이루어져 있지만 원하는 경우 더 복잡한 이벤트를 사용할 수도 있습니다.
/// 
/// 새 이벤트를 어디서든 트리거하려면 YOUR_EVENT.Trigger(YOUR_PARAMETERS)를 사용합니다. 
/// 예를 들어 GameEvent.Trigger("Save");는 Save GameEvent를 트리거합니다.
///
/// EventManager.TriggerEvent(YOUR_EVENT)를 호출할 수도 있습니다. 
/// 예를 들어 EventManager.TriggerEvent(new GameEvent("GameStart"));는 GameStart라는 이름의 GameEvent를 모든 리스너에게 브로드캐스트합니다.
///
/// 어떤 클래스에서든 이벤트를 수신하려면 세 가지 작업을 수행해야 합니다 :
/// 
/// 1. 해당 유형의 이벤트에 대해 EventListener 인터페이스를 구현한다고 알려야 합니다. 
/// 예를 들어 public class GUIManager : Singleton<GUIManager>, EventListener<GameEvent>와 같이 작성합니다. 
/// 이는 하나 이상 (이벤트 유형당 하나)의 인터페이스를 가질 수 있습니다.
/// 
/// 2. OnEnable과 OnDisable에서 각각 이벤트 수신을 시작하고 중지해야 합니다. 
/// 예를 들어 다음과 같이 작성합니다.
/// void OnEnable() { this.EventStartListening<GameEvent>(); } 
/// void OnDisable() { this.EventStopListening<GameEvent>(); }
/// 
/// 3. 해당 이벤트에 대한 EventListener 인터페이스를 구현해야 합니다. 
/// 예를 들어 다음과 같이 작성하면 게임에서 발생한 모든 GameEvent를 수신하고,
/// 이름이 GameOver인 경우에 대해 동작을 수행할 수 있습니다.
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
    /// 특정 이벤트에 구독자를 추가합니다.
    /// </summary>
    /// <param name="listener">구독자.</param>
    /// <typeparam name="EventType">이벤트타입</typeparam>
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
    /// 특정 이벤트에서 구독자를 제거합니다.
    /// </summary>
    /// <param name="listener">구독자</param>
    /// <typeparam name="EventType">이벤트타입</typeparam>
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
    /// 해당 이벤트를 구독하고 있는 모든 객체에 대해 이벤트 트리거를 발동합니다. 
    /// </summary>
    /// <param name="newEvent">트리거를 발동할 이벤트</param>
    /// <typeparam name="EventType">이벤트타입</typeparam>
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
    /// 특정 이벤트에 대해 구독자가 있는지 체크합니다.
    /// </summary>
    /// <returns><c>true</c> 구독되어있다면, <c>false</c> 아니라면.</returns>
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
/// 모든 클래스에 대해 이벤트를 구독하거나 제거하게 해주는 스태틱 클래스
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
/// Event listener 기본 인터페이스
/// </summary>
public interface EventListenerBase { };

/// <summary>
/// 원하는 이벤트타입을 구독하기 위해 필요한 인터페이스
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
