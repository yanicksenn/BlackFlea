using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "AudioClipDefinition",
    menuName = "Create audio clip definition")]
public class AudioClipDefinition : ScriptableObject
{
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    private List<AudioClip> alternatives = new();

    [SerializeField]
    private float volume;
    public float Volume => volume;

    private List<AudioClip> clips;

    private void OnEnable()
    {
        clips = new();
        if (clip != null)
        {
            clips.Add(clip);
        }
        clips.AddRange(alternatives);
    }

    public Optional<AudioClip> GetAudioClip()
    {
        if (clips.Count == 0)
        {
            return Optional<AudioClip>.OfEmpty();
        }

        if (clips.Count == 1)
        {
            return Optional<AudioClip>.Of(clip);
        }
        return Optional<AudioClip>.Of(clips[Random.Range(0, clips.Count) % clips.Count]);
    }

    public void PlayOneShot()
    {
        UnityUtils.FindAnyObjectByType<GlobalAudioSource>().IfPresent(audioSource =>
        {
            GetAudioClip().IfPresent(clip =>
            {
                audioSource.PlayOneShot(clip, Volume);
            });
        });
    }
}