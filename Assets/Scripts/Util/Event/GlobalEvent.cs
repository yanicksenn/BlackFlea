using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GlobalEvent", menuName = "Create global event")]
public class GlobalEvent : ScriptableObject
{
    [SerializeField, TextArea]
    private string description;

    [SerializeField, Space]
    private UnityEvent @event = new();

    public void AddListener(UnityAction action)
    {
        @event.AddListener(action);
    }
    public void RemoveListener(UnityAction action)
    {
        @event.RemoveListener(action);
    }

    public void Invoke()
    {
        @event.Invoke();
    }
}