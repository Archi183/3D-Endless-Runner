using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float speedGrowthFactor = 1.05f;
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float speedGrowthFactorRate = 100f;
    private float distance;
    private float currentSpeed;

    public float GetSpeed() {
        return currentSpeed;
    }

    private void Start() {
        currentSpeed = baseSpeed;
    }

    private void Update() {
        CalculateDistance();
        UpdateSpeed();
    }

    private void CalculateDistance() {
        distance += currentSpeed * Time.deltaTime;
    }

    private void UpdateSpeed() {
        float newSpeed = baseSpeed * Mathf.Pow(speedGrowthFactor, distance / speedGrowthFactorRate);
        currentSpeed = Mathf.Min(newSpeed, maxSpeed);
    }
}