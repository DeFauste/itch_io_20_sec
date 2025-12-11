using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip loopClip;
    public AudioClip eventClip;

    public float crossfadeDuration = 2f;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    private bool isCrossfading = false;
    private float crossfadeTimer = 0f;
    private AudioSource currentSource;
    private AudioSource nextSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        audioSource1.playOnAwake = false;
        audioSource2.playOnAwake = false;
        audioSource1.loop = true;
        audioSource2.loop = false;

        if (loopClip != null)
        {
            audioSource1.clip = loopClip;
            audioSource1.volume = 1f;
            audioSource1.Play();
            currentSource = audioSource1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrossfading)
        {
            crossfadeTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(crossfadeTimer / crossfadeDuration);

            currentSource.volume = 1f - progress;
            nextSource.volume = progress;

            if (progress >= 1f)
            {
                isCrossfading = false;
                currentSource.Stop();
                currentSource = nextSource;
            }
        }

        if (currentSource == audioSource2 && !audioSource2.isPlaying && !isCrossfading)
        {
            ReturnToLoop();
        }
    }

    public void TriggerEvent()
    {
        if (eventClip == null)
        {
            return;
        }

        StartCrossfade(eventClip, false);
    }

    public void ReturnToLoop()
    {
        if (loopClip == null)
        {
            return;
        }

        StartCrossfade(loopClip, true);
    }

    private void StartCrossfade(AudioClip targetClip, bool shouldLoop)
    {
        nextSource = (currentSource == audioSource1) ? audioSource2 : audioSource1;

        nextSource.clip = targetClip;
        nextSource.loop = shouldLoop;
        nextSource.volume = 0f;
        nextSource.Play();

        isCrossfading = true;
        crossfadeTimer = 0f;
    }
}
