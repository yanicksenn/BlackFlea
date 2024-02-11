using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GlobalAudioSource : MonoBehaviour {
    public static Optional<GlobalAudioSource> Instance = Optional<GlobalAudioSource>.OfEmpty();

    [SerializeField]
    private float allowRepeatingClipAtPercentage = 0f;

    private AudioSource audioSource;
    private readonly Dictionary<AudioClip, float> playingClipCache = new();

    private void Awake() {
        if (Instance.IsPresent && Instance.Get() != this) {
            Destroy(this);
        } else {
            Instance = Optional<GlobalAudioSource>.Of(this);
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnDestroy() {
        if (Instance.Get() == this) {
            Instance = Optional<GlobalAudioSource>.OfEmpty();
        }
    }

    public void PlayOneShot(AudioClipDefinition definition) {
        definition.GetAudioClip().IfPresent(clip => {
            PlayOneShot(clip, definition.Volume);
        });
    }

    public void PlayOneShot(AudioClip audioClip, float volume) {
        if (playingClipCache.TryGetValue(audioClip, out var finishTime)) {
            if (Time.realtimeSinceStartup < finishTime) {
                return;
            }
        }

        audioSource.PlayOneShot(audioClip, volume);
        var timeOffsett = audioClip.length * Mathf.Clamp01(allowRepeatingClipAtPercentage);
        playingClipCache[audioClip] = Time.realtimeSinceStartup + timeOffsett;
    }
}