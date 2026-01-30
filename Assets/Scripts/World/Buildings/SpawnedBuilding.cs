// A building that can be put in a cell


using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnedBuilding : MonoBehaviour
{
    private Building _building;
    public Building building {
        get { return _building; }
        set { _building = value; }
    }

    private Cell _occupiedCell;
    public Cell occupiedCell {
        get { return _occupiedCell; }
        set { _occupiedCell = value; }
    }

    public HashSet<int> accessRoads = new HashSet<int>();

    [SerializeField] public MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;

    [Header("Effects")]
    [SerializeField] AudioPlayer audioPlayer;
    [SerializeField] ParticlePlayer smokeParticle;

    private static ObjectPool<AudioPlayer> audioPlayerPool;
    private static ObjectPool<ParticlePlayer> smokePool;

    public void OnBuild() {
        meshFilter.mesh = building.mesh;
        meshRenderer.material = building.material;

        building.Built(this);

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

    public void CheckAccessRoads() {
        accessRoads.Clear();

        foreach (Vector2Int dir in Data.directions) {
            Vector2Int pos = occupiedCell.position + dir;

            if (Utility.InBounds(pos)) {
                Cell cell = PlayerData.grid.Cells[pos.x, pos.y];

                if (cell.IsOccupied && cell.building.building is Road) {
                    accessRoads.Add(cell.roadNetworkId);
                }
            }
        }
    }
}