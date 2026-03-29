using UnityEngine;

public class HouseSpawner : MonoBehaviour {
    [SerializeField] private GameObject sideHouse;
    [SerializeField] private GameObject sideBackWall;
    [SerializeField] private Transform[] houseSpawnPointsLeft;
    [SerializeField] private Transform[] houseSpawnPointsRight;
    [SerializeField] private Transform[] wallSpawnPoints;
    [SerializeField] private Transform houseParent;

    private void SpawnHouseLeft() {
        foreach (Transform spawnPoints in houseSpawnPointsLeft){

            Quaternion rotation = Quaternion.Euler(0, 90f, 0);

            GameObject obj = Instantiate(sideHouse, spawnPoints.position, rotation);
            obj.transform.SetParent(houseParent, true);            
        }
    }
    private void SpawnHouseRight() {
        foreach (Transform spawnPoints in houseSpawnPointsRight){

            Quaternion rotation = Quaternion.Euler(0, -90f, 0);

            GameObject obj = Instantiate(sideHouse, spawnPoints.position, rotation);
            obj.transform.SetParent(houseParent, true);            
        }
    }

    private void SpawnWalls() {
        foreach (Transform spawnPoints in wallSpawnPoints){

            Quaternion rotation = Quaternion.Euler(0, 0, 0);

            GameObject obj = Instantiate(sideBackWall, spawnPoints.position, rotation);
            obj.transform.SetParent(houseParent, true);
        }
        
    }

    private void Start() {
        SpawnHouseLeft();
        SpawnHouseRight();
        SpawnWalls();
    }

}
