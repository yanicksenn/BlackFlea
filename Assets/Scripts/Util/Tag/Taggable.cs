using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Taggable : MonoBehaviour {

    [SerializeField]
    private List<Tag> tags = new();
    public IEnumerable<Tag> Tags => tags;

    private void OnEnable() {
        tags.ForEach(t => t.Add(gameObject));
    }

    private void OnDisable() {
        tags.ForEach(t => t.Remove(gameObject));
    }
}