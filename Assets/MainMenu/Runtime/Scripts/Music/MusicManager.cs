using System.Collections;
using UnityEngine;

public enum TrackType
{
    None,
    PressStartTrack,
    MenuThemeTrack
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioClip pressStartTrack;
    [SerializeField]
    private AudioClip menuThemeTrack;

    private TrackType currentTrack;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayTrack(TrackType.PressStartTrack);
    }

    public void PlayTrack(TrackType newTrack, float delay)
    {
        StartCoroutine(PlayTrackRoutine(newTrack, delay));
    }

    IEnumerator PlayTrackRoutine(TrackType newTrack, float delay)
    {
        yield return new WaitForSeconds(delay);

        PlayTrack(newTrack);
    }

    public void PlayTrack(TrackType newTrack)
    {
        if (newTrack == currentTrack)
            return;

        currentTrack = newTrack;

        switch (currentTrack)
        {
            case TrackType.None:
                musicSource.Stop();
                break;
            case TrackType.PressStartTrack:
                musicSource.clip = pressStartTrack;
                musicSource.Play();
                break;

            case TrackType.MenuThemeTrack:
                musicSource.clip = menuThemeTrack;
                musicSource.Play();
                break;
        }
    }
}
