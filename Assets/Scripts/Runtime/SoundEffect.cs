using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class SoundEffect : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (_audioSource.clip != null)
        {
            _audioSource.Play();
        }
    }

    private void OnDisable()
    {
        _audioSource.Stop();
    }

    public void SetSound(SoundEffectSO soundEffect)
    {
        _audioSource.pitch = Random.Range(soundEffect.soundEffectPitchRandomVariationMin,
            soundEffect.soundEffectPitchRandomVariationMax);
        _audioSource.volume = soundEffect.soundEffectVolume;
        _audioSource.clip = soundEffect.soundEffectClip;
    }
}
