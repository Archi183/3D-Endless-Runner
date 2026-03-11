using UnityEngine;

public class TileSpawner : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int startingTiles = 5;
    [SerializeField] private float tileLength = 30f;
    private float spawnZ = -10;

    private void Start() {
        for (int i=0; i<startingTiles; i++) {
            SpawnTile();
        }

    }

    public void SpawnTile() {
        Vector3 tileSpawnPosition = new Vector3(0, 0, spawnZ);
        Instantiate(tilePrefab, tileSpawnPosition, Quaternion.identity);
        spawnZ += tileLength;
    }

}
