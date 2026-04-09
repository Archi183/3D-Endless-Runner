using UnityEngine;
using System;

public enum GameState {
    waitingToStart,
    playing,
    gameOver
}

public class GameManager : MonoBehaviour {
    [SerializeField] private GameAudioManager gameAudioManager;
    [SerializeField] private GameInputManager inputManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float speedGrowthFactor = 1.05f;
    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float speedGrowthFactorRate = 100f;
    private float distance;
    private float currentSpeed;
    [SerializeField] private float runInterval = 1f;
    public GameState gameState = GameState.waitingToStart;


    private void Start() {
        currentSpeed = baseSpeed;
        gameState = GameState.playing;
        StartCoroutine(gameAudioManager.PlayBGM(gameState));
        StartCoroutine(gameAudioManager.PlaySoftWind(gameState));
        StartCoroutine(gameAudioManager.PlayRun(runInterval));
    }

    private void OnEnable() {
        playerController.onLaneChange += PlayerController_OnLaneChange;
        inputManager.jump += InputManager_jump_gameManager;
    }
    private void OnDisable() {
        playerController.onLaneChange -= PlayerController_OnLaneChange;
        inputManager.jump -= InputManager_jump_gameManager;
    }

    
    private void PlayerController_OnLaneChange(object sender, EventArgs e) {
        gameAudioManager.PlayChangelane();
    }

    private void InputManager_jump_gameManager(object sender, EventArgs e) {
        if(!playerController.IsOnGround()) return;
        gameAudioManager.PlayJump();
    }

    private void Update() {
        CalculateDistance();
        UpdateSpeed();
        Debug.Log(playerController.IsOnGround());
    }

    private void CalculateDistance() {
        distance += currentSpeed * Time.deltaTime;
    }

    private void UpdateSpeed() {
        float newSpeed = baseSpeed * Mathf.Pow(speedGrowthFactor, distance / speedGrowthFactorRate);
        currentSpeed = Mathf.Min(newSpeed, maxSpeed);
    }

    public float GetSpeed() {
        return currentSpeed;
    }



}