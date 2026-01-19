// A building that can be put in a cell


using UnityEngine;
using DG.Tweening;

public class SpawnedBuilding : MonoBehaviour
{
    private Building _building;
    public Building building {
        get { return _building; }
        set { 
            _building = value;
        }
    }
    public Cell occupiedCell;

    [SerializeField] public MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Effects")]
    [SerializeField] AudioPlayer audioPlayer;
    [SerializeField] ParticlePlayer smokeParticle;

    private static ObjectPool<AudioPlayer> audioPlayerPool;
    private static ObjectPool<ParticlePlayer> smokePool;

    public void OnBuild() {
        meshFilter.mesh = _building.mesh;
        meshRenderer.material = _building.material;
        
        _building.Built(this);

        if (audioPlayerPool == null) {
            audioPlayerPool = new ObjectPool<AudioPlayer>(audioPlayer, PlayerData.player.transform);
            smokePool = new ObjectPool<ParticlePlayer>(smokeParticle, PlayerData.player.transform);
        }

        // Spawn effects
        AudioPlayer audio = audioPlayerPool.Get();
        ParticlePlayer smoke = smokePool.Get();

        smoke.transform.position = audio.transform.position = transform.position;

        audio.Play(building.placementSound, true);
        smoke.Play();

        // Tween Animation
        Vector3 baseScale = transform.localScale;
        transform.localScale = new Vector3(0.5f, 0.01f, 0.5f);

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOScale(baseScale * 1.125f, 0.125f))
           .Append(transform.DOScale(baseScale * 0.875f, 0.25f))
           .Append(transform.DOScale(baseScale, 0.5f));
    }
}