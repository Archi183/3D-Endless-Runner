using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour {
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform obstacleParent;

    [SerializeField] private int minObstacles = 0;
    [SerializeField] private int maxObstacles = 6;

    private void Start() {
        Spawn();
    }

    public void Spawn() {
        int obstacleCount = Random.Range(minObstacles, maxObstacles + 1);

        List<int> used = new List<int>();

        for (int i = 0; i < obstacleCount; i++) {
            int index;

            do {
                index = Random.Range(0, spawnPoints.Length);
            } while (used.Contains(index));

            used.Add(index);

            Transform spawnPoint = spawnPoints[index];

            Vector3 offset = new Vector3(0, 0, Random.Range(-0.1f, 0.1f));

            int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject obstacle = obstaclePrefabs[obstacleIndex];

            Quaternion rotation = Quaternion.identity;

            if (!(obstacleIndex == 3 || obstacleIndex == 4)) {
                rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90f, 0);
            }
            GameObject obj = Instantiate(obstacle, spawnPoint.position + offset, rotation);
            obj.transform.SetParent(obstacleParent, true); 
        }
    }

}