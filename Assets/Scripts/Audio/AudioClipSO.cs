using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "AudioClip SO", menuName = "Audio SO/AudioClip")]
public class AudioClipSO : ScriptableObject
{
    public enum AudioTypes
    {
        SFX,
        Music
    }

    public AudioTypes audioType;
    public AudioMixerGroup audioMixer;
    public AudioClip clip;

    public float volume = 1;
    public float pitch = 1;
    public bool loop;
}
