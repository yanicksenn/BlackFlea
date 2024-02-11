using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tag", menuName = "Create tag")]
public class Tag : ScriptableObject
{
    [SerializeField, TextArea]
    private string description;

    private readonly HashSet<GameObject> instances = new();
    public IEnumerable<GameObject> Instances => instances;

    public void Add(GameObject obj)
    {
        instances.Add(obj);
    }

    public void Remove(GameObject obj)
    {
        instances.Remove(obj);
    }
}