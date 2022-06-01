using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect", menuName="Scriptable Objects/Sounds/New Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    [Header("Sound Effect Details")]
    [Tooltip("The name for the sound effect")]
    public string soundEffectName;
    [Tooltip("The prefab for the sound effect")]
    public GameObject soundPrefab;
    [Tooltip("The audio clip for the sound effect")]
    public AudioClip soundEffectClip;
    [Tooltip("The minimum pitch variation for the sound effect. A random pitch variation will be generated between the minimum and maximum values. A random pitch variation makes sound effects sound more natural.")]
    [Range(0.1f, 1.5f)] public float soundEffectPitchRandomVariationMin = 0.8f;
    [Tooltip("The minimum pitch variation for the sound effect. A random pitch variation will be generated between the minimum and maximum values. A random pitch variation makes sound effects sound more natural.")]
    [Range(0.1f, 1.5f)] public float soundEffectPitchRandomVariationMax = 1.2f;
    [Tooltip("The sound effect volume")]
    [Range(0f, 1f)] public float soundEffectVolume = 1f;
    [Tooltip("Is the Sound Effect supposed to loop?")]
    public bool loopSoundEffect = false;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        Utils.ValidateCheckEmptyString(this, nameof(soundEffectName), soundEffectName);
        Utils.ValidateCheckNullValue(this, nameof(soundPrefab), soundPrefab);
        Utils.ValidateCheckNullValue(this, nameof(soundEffectClip), soundEffectClip);
        Utils.ValidateCheckPositiveRange(this, nameof(soundEffectPitchRandomVariationMin),
            soundEffectPitchRandomVariationMin, nameof(soundEffectPitchRandomVariationMax),
            soundEffectPitchRandomVariationMax, false);
        Utils.ValidateCheckPositiveValue(this, nameof(soundEffectVolume), soundEffectVolume, true);
    }
    #endif
}
