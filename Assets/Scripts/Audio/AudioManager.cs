using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    #region --Clips--

    [Header("Audio Scriptable Objects")]
    [SerializeField] MusicClipsSO musicClips;
    [SerializeField] UIAudioClips uiAudioClips;
    [SerializeField] VFXClipsSO vfxClips;
    
    #endregion

    AudioSource musicAudioSource;

    public static AudioManager instance;

    private int levelOffset = 2;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        musicAudioSource = GetComponent<AudioSource>();
    }
    
    public void PlaySound(AudioClipSO soundSO)
    {
        GameObject soundObject = new GameObject("Audio Clip Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = soundSO.clip;
        audioSource.loop = soundSO.loop;
        audioSource.volume = soundSO.volume;
        audioSource.pitch = soundSO.pitch;
        audioSource.outputAudioMixerGroup = soundSO.audioMixer;

        audioSource.Play();
        Destroy(soundObject, soundSO.clip.length);
    }

    public void PlayMusic(int track)
    {
        musicAudioSource.Stop();
        
        musicAudioSource.clip = musicClips.musicClip[track].clip;
        musicAudioSource.loop = musicClips.musicClip[track].loop;
        musicAudioSource.volume = musicClips.musicClip[track].volume;
        musicAudioSource.pitch = musicClips.musicClip[track].pitch;
        musicAudioSource.outputAudioMixerGroup = musicClips.musicClip[track].audioMixer;
    
        musicAudioSource.Play();
    }
    
    #region -- UI SFX --

    public void PlayUISelect()
    {
        PlaySound(uiAudioClips.selectSound);
    }
    
    public void PlayUIClose()
    {
       PlaySound(uiAudioClips.closeSound);
    }
    
    public void PlayUIError()
    {
        PlaySound(uiAudioClips.errorSound);
    }

    public void PlayLevelCleared()
    {
        PlaySound(vfxClips.levelCleared);
    }
    
    public void PlayGameOver()
    {
        if(vfxClips.gameOver != null)
            PlaySound(vfxClips.gameOver);
    }
    
    #endregion
}
