using UnityEngine;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
    private TileSpawner tileSpawner;    
    [SerializeField] private float tileDeleteTime = 4f;

    public void Init(TileSpawner spawner) {
        this.tileSpawner = spawner;
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        tileSpawner.SpawnTile();
        Destroy(transform.parent.gameObject, tileDeleteTime);
    }

}