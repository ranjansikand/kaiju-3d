// A building that can be put in a cell


using UnityEngine;

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

        AudioPlayer audio = audioPlayerPool.Get();
        ParticlePlayer smoke = smokePool.Get();

        smoke.transform.position = audio.transform.position = transform.position;

        audio.Play(building.placementSound, true);
        smoke.Play();
    }
}