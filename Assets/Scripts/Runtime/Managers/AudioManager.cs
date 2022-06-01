using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

[DisallowMultipleComponent]
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Audio Settings")] 
    [Tooltip("Populate with the Master Mixer Group")]
    public AudioMixerGroup masterMixerGroup;
    [Tooltip("Populate with the Music Master Mixer Group")]
    public AudioMixerGroup musicMasterMixerGroup;
    [Tooltip("Populate with the Sounds Master Mixer Group")]
    public AudioMixerGroup soundsMasterMixerGroup;
    
    [Header("Music Settings")]
    public int musicVolume = 10;
    public MusicTrackSO[] musicTracks;
    public AudioClip currentAudioClip = null;
    
    [Header("Sound Effects Settings")]
    public int soundsVolume = 8;
    public SoundEffectSO[] soundEffects;
    
    private AudioSource _musicAudioSource = null;
    private Coroutine _fadeOutMusicCoroutine;
    private Coroutine _fadeInMusicCoroutine;

    private void Start()
    {
        SetSoundsVolume(soundsVolume);
        currentAudioClip = musicTracks[0].musicClip;
        _musicAudioSource = GetComponent<AudioSource>();
        SetMusicVolume(musicVolume);
    }

    private void SetMusicVolume(int i)
    {
        const float muteDecibels = -80f;
        musicMasterMixerGroup.audioMixer.SetFloat("MusicVolume",
            musicVolume == 0 ? muteDecibels : Utils.LinearToDecibels(musicVolume));
    }

    public void PlayMusic(MusicTrackSO musicTrack, float fadeOutTime = 2f, float fadeInTime = 1f)
    {
        StartCoroutine(PlayMusicRoutine(musicTrack, fadeOutTime, fadeInTime));
    }

    public void PlayMusic(string musicTrack, float fadeOutTime = 2f, float fadeInTime = 1f)
    {
        foreach (var music in musicTracks)
        {
            if (music.musicName == musicTrack)
                StartCoroutine(PlayMusicRoutine(music, fadeOutTime, fadeInTime));
        }
    }

    private IEnumerator PlayMusicRoutine(MusicTrackSO musicTrack, float fadeOutTime, float fadeInTime)
    {
        if (_fadeOutMusicCoroutine != null)
            StopCoroutine(_fadeOutMusicCoroutine);
        if (_fadeInMusicCoroutine != null)
            StopCoroutine(_fadeInMusicCoroutine);
        if (!musicTrack || musicTrack.musicClip == currentAudioClip) yield break;
        currentAudioClip = musicTrack.musicClip;

        yield return _fadeOutMusicCoroutine = StartCoroutine(FadeOutMusic(fadeOutTime));
        yield return _fadeInMusicCoroutine = StartCoroutine(FadeInMusic(musicTrack, fadeInTime));
    }

    private IEnumerator FadeInMusic(MusicTrackSO musicTrack, float fadeInTime)
    {
        _musicAudioSource.clip = musicTrack.musicClip;
        _musicAudioSource.volume = musicTrack.musicVolume;
        _musicAudioSource.Play();

        yield return new WaitForSeconds(fadeInTime);
    }

    private IEnumerator FadeOutMusic(float fadeOutTime)
    {
        yield return new WaitForSeconds(fadeOutTime);
    }
    
    private void SetSoundsVolume(int volume)
    {
        const float muteDecibels = -80f;
        if (volume == 0)
        {
            soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {
            soundsMasterMixerGroup.audioMixer.SetFloat("SoundsVolume",
                Utils.LinearToDecibels(volume));
        }
    }

    public void PlaySoundEffect(string soundEffect)
    {
        foreach (var sound in soundEffects)
        {
            if (sound.soundEffectName == soundEffect)
                PlaySoundEffect(sound);
        }
    }

    public void PlaySoundEffect(SoundEffectSO soundEffect, bool loop = false)
    {
        var sound = Instantiate(soundEffect.soundPrefab, Vector3.zero, quaternion.identity).GetComponent<SoundEffect>();
        sound.transform.parent = transform;
        sound.SetSound(soundEffect);
        sound.gameObject.SetActive(true);
        sound.GetComponent<AudioSource>().Play();
        if (loop || soundEffect.loopSoundEffect)
            sound.GetComponent<AudioSource>().loop = true;
        else
            StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));
    }

    public void PlaySoundEffect(string soundEffect, bool loop = false)
    {
        foreach (var sound in soundEffects)
        {
            if (sound.soundEffectName == soundEffect)
                PlaySoundEffect(sound, loop);
        }
    }

    private static IEnumerator DisableSound(SoundEffect sound, float soundDuration)
    {
        yield return new WaitForSeconds(soundDuration);
        sound.gameObject.SetActive(false);
    }
}
