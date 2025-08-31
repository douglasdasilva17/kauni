using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource soundFXPrefab;
    AudioSource currentAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundFX(AudioClip audioClip, Transform spawnTransform, float volume, float pitch = 1f)
    {
        AudioSource audioSource = Instantiate(soundFXPrefab, spawnTransform.position, Quaternion.identity) as AudioSource;

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFX(AudioClip[] audioClip, Transform parent, float volume, float pitch = 1f)
    {
        AudioSource audioSource = Instantiate(soundFXPrefab, parent.position, Quaternion.identity) as AudioSource;

        audioSource.reverbZoneMix = 0;
        audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFX(AudioClip[] audioClip, Transform spawnTransform, float volume, float pitch = 1f, float delay = 1f)
    {
        StartCoroutine(PlaySoundDelayed(audioClip, spawnTransform, volume, delay));
    }

    private IEnumerator PlaySoundDelayed(AudioClip[] audioClip, Transform spawnTransform, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);

        AudioSource audioSource = Instantiate(soundFXPrefab, spawnTransform.position, Quaternion.identity) as AudioSource;

        audioSource.reverbZoneMix = 0;
        audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
