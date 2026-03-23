using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public enum ObstacleType {
    empty,
    jump,
    slide,
    block
}

public class ObstacleSpawner : MonoBehaviour {
    [SerializeField] private ObstacleSO[] obstaclePool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform obstacleParent;

    private Dictionary<ObstacleType, List<ObstacleSO>> categorizedPool = new Dictionary<ObstacleType, List<ObstacleSO>>();
    private void Start() {
        InitializePool();
        SpawnWithPattern();
    }

    private void InitializePool() {
        categorizedPool = obstaclePool
            .GroupBy(so => so.obstacleType)
            .ToDictionary(g => g.Key, g => g.ToList());

    }

    private void SpawnObstacle(int index, ObstacleType targetType) {
        Transform spawnPoint = spawnPoints[index];
        if (!categorizedPool.TryGetValue(targetType, out var validObstacles)) return;
        if (validObstacles.Count == 0) return;
        int obstacleIndex = UnityEngine.Random.Range(0, validObstacles.Count);
        ObstacleSO selectedSo = validObstacles[obstacleIndex];
        Quaternion rotation = Quaternion.identity;
        if (selectedSo.canRotate) {
            rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 4) * 90f, 0);
        }
        GameObject obj = Instantiate(selectedSo.preFab, spawnPoint.position, rotation);
        obj.transform.SetParent(obstacleParent, true); 
    }

    private void SpawnWithPattern() {
        ObstacleType[,] spawningPattern = new ObstacleType[,] {
            { ObstacleType.jump, ObstacleType.empty, ObstacleType.jump },
            { ObstacleType.jump, ObstacleType.empty, ObstacleType.empty },
            { ObstacleType.empty, ObstacleType.empty, ObstacleType.jump }
        };
        int rows = spawningPattern.GetLength(0);
        int lanes = spawningPattern.GetLength(1);
        for (int i=0; i<rows; i++) {
            for (int j=0; j<lanes; j++) {
                if (spawningPattern[i,j] != ObstacleType.empty) {
                    int oneDIndex = i * lanes + j;
                    SpawnObstacle(oneDIndex, spawningPattern[i,j]);
                }
            }
        }
    }

}