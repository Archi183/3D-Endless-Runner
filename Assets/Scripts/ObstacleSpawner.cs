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
    private List<ObstacleType[,]> patternLibrary;
    private Dictionary<ObstacleType, List<ObstacleSO>> categorizedPool = new Dictionary<ObstacleType, List<ObstacleSO>>();
    private void Start() {
        InitializePool();
        patternLibrary = InitializePatterns();
        SpawnWithSelectedPattern();
    }

    private List<ObstacleType[,]> InitializePatterns() {
        List<ObstacleType[,]> patternLibrary = new List<ObstacleType[,]> {

            // MEDIUM PATTERNS
            // M1: J0J / 0B0 / 000
            new ObstacleType[,]
            {
                { ObstacleType.jump,  ObstacleType.empty, ObstacleType.jump },
                { ObstacleType.empty, ObstacleType.block, ObstacleType.empty },
                { ObstacleType.empty, ObstacleType.empty, ObstacleType.empty }
            },

            // M2: B00 / 0J0 / 00B
            new ObstacleType[,]
            {
                { ObstacleType.block, ObstacleType.empty, ObstacleType.empty },
                { ObstacleType.empty, ObstacleType.jump,  ObstacleType.empty },
                { ObstacleType.empty, ObstacleType.empty, ObstacleType.block }
            },

            // M3: 0J0 / B0B / 000
            new ObstacleType[,]
            {
                { ObstacleType.empty, ObstacleType.jump,  ObstacleType.empty },
                { ObstacleType.block, ObstacleType.empty, ObstacleType.block },
                { ObstacleType.empty, ObstacleType.empty, ObstacleType.empty }
            },

            // M4: B0B / 0J0 / B00
            new ObstacleType[,]
            {
                { ObstacleType.block, ObstacleType.empty, ObstacleType.block },
                { ObstacleType.empty, ObstacleType.jump,  ObstacleType.empty },
                { ObstacleType.block, ObstacleType.empty, ObstacleType.empty }
            },


            // HARD PATTERNS
            // H1: B0B / 0B0 / 000
            new ObstacleType[,]
            {
                { ObstacleType.block, ObstacleType.empty, ObstacleType.block },
                { ObstacleType.empty, ObstacleType.block, ObstacleType.empty },
                { ObstacleType.empty, ObstacleType.empty, ObstacleType.empty }
            },

            // H2: JBJ / B0B / 000
            new ObstacleType[,]
            {
                { ObstacleType.jump,  ObstacleType.block, ObstacleType.jump },
                { ObstacleType.block, ObstacleType.empty, ObstacleType.block },
                { ObstacleType.empty, ObstacleType.empty, ObstacleType.empty }
            },

            // H3: BJB / 0B0 / 000
            new ObstacleType[,]
            {
                { ObstacleType.block, ObstacleType.jump,  ObstacleType.block },
                { ObstacleType.empty, ObstacleType.block, ObstacleType.empty },
                { ObstacleType.empty, ObstacleType.empty, ObstacleType.empty }
            },

            // H4: B0B / 0J0 / B0B
            new ObstacleType[,]
            {
                { ObstacleType.block, ObstacleType.empty, ObstacleType.block },
                { ObstacleType.empty, ObstacleType.jump,  ObstacleType.empty },
                { ObstacleType.block, ObstacleType.empty, ObstacleType.block }
            }
        };
        return patternLibrary;
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

    private void SpawnWithSelectedPattern() {
        if (patternLibrary==null || patternLibrary.Count==0) return;
        int libraryIndex = UnityEngine.Random.Range(0, patternLibrary.Count);
        ObstacleType[,] spawningPattern = patternLibrary[libraryIndex];
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