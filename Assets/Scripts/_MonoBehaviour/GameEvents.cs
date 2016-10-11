using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameEvents : MonoBehaviour
{
    #region Variables
    private Dictionary<string, UnityEvent> events;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        events = new Dictionary<string, UnityEvent>();
    }

    #endregion MonoBehaviour

    #region Red
    public void StartListening(string event_name, UnityAction listener)
    {
        UnityEvent unity_event = null;

        if (events.TryGetValue(event_name, out unity_event))
        {
            unity_event.AddListener(listener);
        }
        else
        {
            unity_event = new UnityEvent();
            unity_event.AddListener(listener);
            events.Add(event_name, unity_event);
        }
    }

    public void StopListening(string event_name, UnityAction listener)
    {
        UnityEvent unity_event = null;

        if (events.TryGetValue(event_name, out unity_event))
        {
            unity_event.RemoveListener(listener);
        }
    }

    public void TriggerEvent(string eventName)
    {
        UnityEvent unity_event = null;

        if (events.TryGetValue(eventName, out unity_event))
        {
            unity_event.Invoke();
        }
    }
    #endregion Red
}