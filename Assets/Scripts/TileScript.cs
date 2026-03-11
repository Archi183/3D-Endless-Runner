using UnityEngine;

public class TileScript : MonoBehaviour {
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform spawnLeft;
    [SerializeField] private Transform spawnMiddle;
    [SerializeField] private Transform spawnRight;
    
    private TileSpawner tileSpawner;
    [SerializeField] private float tileDeleteTime = 2f;

    void Start() {
        tileSpawner = FindFirstObjectByType<TileSpawner>();

        SpawnObstacle();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        tileSpawner.SpawnTile();

        Destroy(transform.parent.gameObject, tileDeleteTime);
    }
    private void SpawnObstacle() {
        int lane = Random.Range(0, 3);

        Transform spawnPoint;

        if (lane == 0)
            spawnPoint = spawnLeft;
        else if (lane == 1)
            spawnPoint = spawnMiddle;
        else
            spawnPoint = spawnRight;

        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }
}