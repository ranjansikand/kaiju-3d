// Plays a sound


using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    public void Play(AudioClip audioClip, bool varyPitch = false) {
        audioSource.volume = Data.sfxVolume;
        float time = audioClip.length;

        if (varyPitch) audioSource.pitch = Random.Range(0.9f, 1.1f);
        else audioSource.pitch = 1f;

        audioSource.PlayOneShot(audioClip);
        Invoke(nameof(Deactivate), time * 1.1f);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
