using System;
using UnityEngine;

public static class UnityExtensions {
    public static void TryGetComponent<T>(this GameObject me, Action<T> action) where T : Component {
        if (me.TryGetComponent(out T component)) {
            action.Invoke(component);
        }
    }

    public static void TryGetComponent<T>(this Component me, Action<T> action) where T : Component {
        if (me.TryGetComponent(out T component)) {
            action.Invoke(component);
        }
    }

    public static Optional<T> TryGetComponent<T>(this GameObject me) where T : Component {
        return Optional<T>.OfNullable(me.GetComponent<T>());
    }

    public static Optional<T> TryGetComponent<T>(this Component me) where T : Component {
        return Optional<T>.OfNullable(me.GetComponent<T>());
    }
}