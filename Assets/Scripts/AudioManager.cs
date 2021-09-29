using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MyMonoBehaviour
{
    [SerializeField] AudioSource[] voices;
    [SerializeField] AudioSource finalVoice;
    [SerializeField] AudioSource[] musics;

    AudioSource currentVoice;

    private void Start()
    {
        if (levels.currentLevel != Levels.Credits)
        {
            PlayMusic(levels.currentLevel);
            PlayVoice(levels.currentLevel);
        }
        else
            StartCoroutine(credits.StartCreditsSecuence());
    }

    internal void PlayMusic (Levels musicLevel)
    {
        StopAll();
        musics[(int)musicLevel].Play();
    }

    private void StopAll()
    {
        foreach (AudioSource music in musics)
            music.Stop();
    }

    internal void PlayVoice(Levels voiceLevel)
    {
        CancelInvoke();
        currentVoice = voices[(int)voiceLevel];
        Invoke("PlayCurrentVoice", 7);
        Invoke("PlayCurrentVoice", 60);
        InvokeRepeating("PlayCurrentVoice", 130, 90);
    }
    internal void CancelVoices()
    {
        CancelInvoke();
        foreach (AudioSource voice in voices)
            voice.Stop();
    }
    void PlayCurrentVoice()
    {
        currentVoice.Play();
    }

    internal void PlayFinalVoice()
    {
        currentVoice = finalVoice;
        Invoke("PlayCurrentVoice", 7);
    }
}
