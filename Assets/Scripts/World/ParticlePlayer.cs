using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem ps;

    public void Play() {
        float time = ps.main.duration + ps.main.startLifetime.constantMax;
        ps.Play();

        Invoke(nameof(Deactivate), time * 1.1f);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
