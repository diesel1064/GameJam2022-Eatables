using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicTrack_", menuName = "Scriptable Objects/Sounds/New Music Track")]
public class MusicTrackSO : ScriptableObject
{
    [Header("Music Track Details")] 
    [Tooltip("The name for the music track")]
    public string musicName;

    [Tooltip("The audio clip fo the music track")]
    public AudioClip musicClip;

    [Tooltip("The volume for the music track")]
    [Range(0, 1)]
    public float musicVolume = 1f;
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        Utils.ValidateCheckEmptyString(this, nameof(musicName), musicName);
        Utils.ValidateCheckNullValue(this, nameof(musicClip), musicClip);
        Utils.ValidateCheckPositiveValue(this, nameof(musicVolume), musicVolume, true);
    }   
    #endif
}
