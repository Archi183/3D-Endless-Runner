using UnityEngine;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
    // [SerializeField] private GameObject[] obstaclePrefabs;
    // [SerializeField] private Transform[] spawnPoints;
    // [SerializeField] private Transform obstacleParent;
    private TileSpawner tileSpawner;    
    [SerializeField] private float tileDeleteTime = 2f;

    public void Init(TileSpawner spawner) {
        this.tileSpawner = spawner;
    }

    // void Start() {
    //     SpawnObstacle();
    // }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        tileSpawner.SpawnTile();
        Destroy(transform.parent.gameObject, tileDeleteTime);
    }

    // private void SpawnObstacle() {
    //     Debug.Log("Spawning tile");

    //     int obstacleCount = Random.Range(1, 4);

    //     List<int> used = new List<int>();

    //     for (int i = 0; i < obstacleCount; i++){
    //         int index;

    //         do {
    //             index = Random.Range(0, spawnPoints.Length);
    //         } while (used.Contains(index));

    //         used.Add(index);

    //         Transform spawnPoint = spawnPoints[index];

    //         Vector3 offset = new Vector3(Random.Range(-0.1f, 0.1f),0,Random.Range(-0.1f, 0.1f));

    //         int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
    //         GameObject obstacle = obstaclePrefabs[obstacleIndex];

    //         if (obstacleIndex == 3 || obstacleIndex == 4) {
    //             GameObject obj = Instantiate(obstacle, spawnPoint.position + offset, Quaternion.identity);
    //             obj.transform.SetParent(obstacleParent, true);     
    //         } else {
    //             Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90f, 0);
    //             GameObject obj = Instantiate(obstacle, spawnPoint.position + offset, rotation);
    //             obj.transform.SetParent(obstacleParent, true);      
    //         }
    //     }
    // }

}