using System.Linq;
using UnityEngine;

public static class TagsExtensions
{
    public static bool HasTag(this GameObject gameObject, Tag tag)
    {
        return gameObject.TryGetComponent<Taggable>()
            .Map(t => t.Tags.Contains(tag))
            .GetOrElse(false);
    }
}