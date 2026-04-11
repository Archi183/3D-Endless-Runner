using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingGameHandler : MonoBehaviour {
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip startingScreenBgm;
    [SerializeField] private AudioClip optionSelect;
    [SerializeField] private AudioClip optionSelectReverse;

    private void Start() {
        PlayStartingBGM();
    }

    private void PlayStartingBGM() {
        bgmSource.volume = 0.5f;
        bgmSource.clip = startingScreenBgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayOptionSelect() {
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(optionSelect);
    }

    public void PlayOptionSelectReverse() {
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(optionSelectReverse);
    }
    
    public void StartMainGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }

}