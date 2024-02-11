
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public static class UIUtils {

    public static IEnumerator FadeInOverSeconds(TMP_Text text, float duration, Action completion = null) {
        var maxDuration = duration;
        var timeLeft = maxDuration;
        while (timeLeft > 0) {
            yield return 0;
            timeLeft = Mathf.Clamp01(timeLeft - Time.deltaTime);
            SetAlphaColor(text, 1f - (timeLeft / maxDuration));
        }
        completion?.Invoke();
    }

    public static IEnumerator FadeOutOverSeconds(TMP_Text text, float duration, Action completion = null) {
        var maxDuration = duration;
        var timeLeft = maxDuration;
        while (timeLeft > 0) {
            yield return 0;
            timeLeft = Mathf.Clamp01(timeLeft - Time.deltaTime);
            SetAlphaColor(text, timeLeft / maxDuration);
        }
        completion?.Invoke();
    }

    public static void SetAlphaColor(TMP_Text text, float alpha) {
        var originalColor = text.color;
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}