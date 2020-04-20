using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource efxSource;
    public AudioSource efxSource2;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    public int efxIndex = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

    }

    public void PlaySingle(AudioClip clip)
    {
        if (efxIndex == 0) {
            efxSource.clip = clip;
            efxSource.Play();
        }
        else
        {
            efxSource2.clip = clip;
            efxSource2.Play();
        }

        efxIndex = efxIndex == 0 ? 1 : 0;
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];

        efxSource.Play();
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void MuteSFX()
    {
        efxSource.volume = 0f;
        efxSource2.volume = 0f;
    }

    public void UnMuteSFX()
    {
        efxSource.volume = 1f;
        efxSource2.volume = 1f;
    }

}
