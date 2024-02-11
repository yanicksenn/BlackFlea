using System;
using System.Collections;
using UnityEngine;

public static class UnityUtils {
    public static Optional<T> FindAnyObjectByType<T>() where T : UnityEngine.Object
    {
        return Optional<T>.OfNullable(UnityEngine.Object.FindAnyObjectByType<T>());
    }

    public static IEnumerator DoAfterSeconds(float seconds, Action action) {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    }

    public static IEnumerator RepeatEvery(float seconds, Action action, Func<bool> completion)
    {
        while (true && !completion.Invoke())
        {
            yield return new WaitForSeconds(seconds);
            action.Invoke();
        }
    }
}