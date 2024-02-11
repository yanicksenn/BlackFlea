using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractGroup<TEntity, TEvent> : ScriptableObject 
        where TEntity : Component 
        where TEvent : UnityEvent<TEntity> {

    [SerializeField, TextArea]
    private string description;

    public abstract TEvent OnAddEntityEvent {get;}

    public abstract TEvent OnRemoveEntityEvent {get;}

    private readonly HashSet<TEntity> instances = new();
    public IEnumerable<TEntity> Instances => instances;

    public void Add(TEntity obj)
    {
        instances.Add(obj);
        OnAddEntityEvent.Invoke(obj);
    }

    public void Remove(TEntity obj)
    {
        instances.Remove(obj);
        OnRemoveEntityEvent.Invoke(obj);
    }
}