using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using TMPro;

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
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject onPlayingUI;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI highestScoreText;

    private float distance;
    private float currentSpeed;
    private int score = 0;
    private int highestScore;
    private bool isPaused = true;
    [SerializeField] private float runInterval = 1f;
    public GameState gameState = GameState.waitingToStart;
    public static GameManager Instance {get; private set;}

    

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
       
    }

    private void Start() {
        scoreText = onPlayingUI.transform.Find("Score (TMP)").GetComponent<TextMeshProUGUI>();
        highestScoreText = onPlayingUI.transform.Find("HighestScore (TMP)").GetComponent<TextMeshProUGUI>();
        PlayingUI(true);
        LoadHighestScore();
        UpdateHighestScoreUI();
        currentSpeed = baseSpeed;
        gameState = GameState.playing;
        isPaused = false;
        StartCoroutine(gameAudioManager.PlayBGM(gameState));
        StartCoroutine(gameAudioManager.PlaySoftWind(gameState));
        StartCoroutine(gameAudioManager.PlayRun(runInterval));
    }

    private void Update() {
        CalculateDistance();
        UpdateSpeed();
        UpdateScore();
        if (highestScore < score) {
            highestScore = score;
        }
    }

    private void OnEnable() {
        playerController.onLaneChange += PlayerController_OnLaneChange;
        inputManager.jump += InputManager_jump_gameManager;
        inputManager.pause += InputManager_CheckPause;
    }

    private void OnDisable() {
        playerController.onLaneChange -= PlayerController_OnLaneChange;
        inputManager.jump -= InputManager_jump_gameManager;
        inputManager.pause -= InputManager_CheckPause;
    }

    private void PlayerController_OnLaneChange(object sender, EventArgs e) {
        gameAudioManager.PlayChangelane();
    }

    private void InputManager_jump_gameManager(object sender, EventArgs e) {
        if(!playerController.IsOnGround()) return;
        gameAudioManager.PlayJump();
    }

    private void InputManager_CheckPause(object sender, EventArgs e) {
        if (isPaused) Resume();
        else Pause();
    }

    private void CalculateDistance() {
        distance += currentSpeed * Time.deltaTime;
    }

    private void UpdateSpeed() {
        float newSpeed = baseSpeed * Mathf.Pow(speedGrowthFactor, distance / speedGrowthFactorRate);
        currentSpeed = Mathf.Min(newSpeed, maxSpeed);
    }

    private void UpdateScore() {
        score = Mathf.Clamp((int)distance, 0, 99999999);
        
        scoreText.text = "Score: " + score.ToString();

        if (score > highestScore) {
            highestScore = score;
            UpdateHighestScoreUI();
        }
    }

    private void UpdateHighestScoreUI() {
        highestScoreText.text = "Best: " + highestScore.ToString();
    }

    private void SaveHighestScore() {
        PlayerPrefs.SetInt("HighestPlayerScore", highestScore);
    
        PlayerPrefs.Save(); 
    }

    private void LoadHighestScore() {
        highestScore = PlayerPrefs.GetInt("HighestPlayerScore", 0);
        
    }

    private void PlayingUI(bool value) {
        onPlayingUI.SetActive(value);
    }

    public void GameOver() {
        if (gameState != GameState.gameOver) {
            PlayingUI(false);
            gameState = GameState.gameOver;
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            gameAudioManager.PlayGameOver();
            gameAudioManager.SetGameAudioPaused(isPaused);
            if (score > highestScore) {
                highestScore = score;
            }
            SaveHighestScore();
        }
    }

    public void Resume() {
        PlayingUI(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        gameAudioManager.SetGameAudioPaused(isPaused);
    }

    public void Pause() {
        PlayingUI(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        gameAudioManager.SetGameAudioPaused(isPaused);
    }

    public void Restart() {
        PlayingUI(true);
        LoadHighestScore();
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameAudioManager.SetGameAudioPaused(isPaused);
    }

    public float GetSpeed() {
        return currentSpeed;
    }

}