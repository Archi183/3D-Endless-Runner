using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleSO", menuName = "Scriptable Objects/Obstacle")]
public class ObstacleSO : ScriptableObject {
    public string itemName;
    public GameObject preFab;
    public ObstacleType obstacleType;
    public bool canRotate;
}