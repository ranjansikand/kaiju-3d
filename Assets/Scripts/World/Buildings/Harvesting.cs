// A Building that harvests resources


using System.Collections;
using UnityEngine;

public class Harvesting : Building
{
    [SerializeField] CellType harvestCells = CellType.Forest;

    public override void Built(SpawnedBuilding spawnedBuilding) {
        base.Built(spawnedBuilding);

        PlayerData.player.StartCoroutine(HarvestRoutine(spawnedBuilding));
    }

    IEnumerator HarvestRoutine(SpawnedBuilding spawnedBuilding) {
        while (true) {
            Cell cell = CheckNearbyTiles(spawnedBuilding.occupiedCell);

            if (cell != null && spawnedBuilding.accessRoads.Count > 0) {
                Resources harvest = cell.Harvest();

                if (harvest != 0) {
                    PlayerData.inventory = PlayerData.inventory + harvest;
                }
            }

            yield return Data.oneSecondDelay;
        }
    }

    Cell CheckNearbyTiles(Cell start) {
        foreach (Vector2Int dir in Data.directions) {
            Vector2Int position = start.position + dir;
            Cell celltoCheck = PlayerData.grid.Cells[position.x, position.y];

            if (celltoCheck.cellType == harvestCells) return celltoCheck;
        }

        return null;
    }
}
