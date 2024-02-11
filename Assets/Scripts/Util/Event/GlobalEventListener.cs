using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventListeners : MonoBehaviour
{
    [SerializeField]
    private List<GlobalEventListener> listeners = new();

    private void OnEnable()
    {
        listeners.ForEach(l => l.OnEnable());
    }

    private void OnDisable()
    {
        listeners.ForEach(l => l.OnDisable());
    }

    [Serializable]
    private class GlobalEventListener
    {
        [SerializeField]
        private GlobalEvent globalEvent;

        [SerializeField]
        private UnityEvent @event = new();

        public void OnEnable()
        {
            globalEvent.AddListener(OnInvoke);
        }

        public void OnDisable()
        {
            globalEvent.RemoveListener(OnInvoke);
        }

        private void OnInvoke()
        {
            @event.Invoke();
        }
    }
}