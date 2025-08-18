using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioClips
    {
        public AudioClip melee, range, money;
    }

    [Header("Sound")]
    public AudioSource audioSource;
    
    [Header("Clips")]
    public AudioClips audioClips;

    private Coroutine coroutine;
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip, float delay = 0, float pitch = 0, bool randomPitch = false, float spatialBlend = 0, float volume = 1)
    {
        coroutine = StartCoroutine(Play(clip, delay, pitch, randomPitch, spatialBlend, volume));
    }

    public void PlayRandomSound(AudioClip[] clips, float delay = 0, float pitch = 0, bool randomPitch = false, float spatialBlend = 0, float volume = 1)
    {
        var randomSoundIndex = Random.Range(0, clips.Length);
        PlaySound(clips[randomSoundIndex], delay, pitch, randomPitch, spatialBlend, volume);
    }

    private IEnumerator Play(AudioClip clip, float delay = 0, float pitch = 0, bool randomPitch = false, float spatialBlend = 0, float volume = 1)
    {
        if (delay > 0) yield return new WaitForSeconds(delay);

        audioSource.spatialBlend = spatialBlend;

        var pitchAdded = randomPitch ? Random.Range(-pitch, pitch) : pitch;
        audioSource.pitch = 1 + pitchAdded;

        audioSource.volume = volume;

        audioSource.PlayOneShot(clip);
        yield return null;
    }

    public void StopPlaying()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        audioSource.Stop();
    }
}