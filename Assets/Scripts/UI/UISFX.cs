using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFX : MonoBehaviour
{
    private static AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public static void Play(AudioClip audioClip, bool varyPitch = false) {
        audioSource.volume = Data.sfxVolume;

        if (varyPitch) audioSource.pitch = Random.Range(0.9f, 1.1f);
        else audioSource.pitch = 1f;

        audioSource.PlayOneShot(audioClip);
    }
}
