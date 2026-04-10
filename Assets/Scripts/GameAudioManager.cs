using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;

public class GameAudioManager : MonoBehaviour {
    [SerializeField] private float fadeInDuration = 5f;
    [SerializeField] private float bgmTargetVol = .2f;
    [SerializeField] private AudioClip run;
    [SerializeField] private AudioClip jupm;
    [SerializeField] private AudioClip changeLane;
    [SerializeField] private AudioClip optionSelect;
    [SerializeField] private AudioClip softWindLarge;
    [SerializeField] private AudioClip softWindMid;
    [SerializeField] private AudioClip softWindSmall;
    [SerializeField] private AudioClip mainBgm;
    [SerializeField] private AudioClip startingScreenBgm;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource windSource;
    [SerializeField] private AudioSource runSource;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameManager gameManager;

    private void Start() {
        runSource.volume = 0.35f;
    }

    public void PlayJump() {
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(jupm);
    }
    public void Run() {
        runSource.PlayOneShot(run);
    }
    public void PlayChangelane() {
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(changeLane);
    }
    public void PlayOptionSelect() {
        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(optionSelect);
    }

    public void MainBGM() {
        bgmSource.clip = mainBgm;
        bgmSource.loop = false;
        bgmSource.Play();
    }

    public void StartingBGM() {
        bgmSource.clip = startingScreenBgm;
        bgmSource.loop = false;
        bgmSource.Play();
    }

    public void SetGameAudioPaused(bool isPaused) {
        if (isPaused) {
            bgmSource.Pause();
            windSource.Pause();
            runSource.Pause();
        } else {
            bgmSource.UnPause();
            windSource.UnPause();
            runSource.UnPause();
        }
    }

    public void PlaySoftWindLarge() {
        windSource.clip = softWindLarge;
        windSource.loop = false;
        windSource.volume = 0.5f;
        windSource.Play();
    }
    public void PlaySoftWindMid() {
        windSource.clip = softWindMid;
        windSource.loop = false;
        windSource.volume = 0.5f;
        windSource.Play();
    }
    public void PlaySoftWindSmall() {
        windSource.clip = softWindSmall;
        windSource.loop = false;
        windSource.volume = 0.5f;
        windSource.Play();
    }

    public IEnumerator PlaySoftWind(GameState gameState) {
        while (gameState == GameState.playing) {
            float waitRandom = UnityEngine.Random.Range(5f, 20f);

            yield return new WaitForSeconds(waitRandom);
            if (waitRandom >= 5f & waitRandom < 10f) PlaySoftWindSmall();
            else if (waitRandom >= 10f & waitRandom <= 15f) PlaySoftWindMid();
            else PlaySoftWindLarge();

            yield return new WaitWhile(() => windSource.isPlaying);

        }
    }

    public IEnumerator PlayBGM(GameState gameState) {
        while (gameState == GameState.playing) {
            yield return new WaitForSeconds(30f);

            MainBGM();
            StartCoroutine(FadeIn(bgmSource, fadeInDuration, bgmTargetVol));

            yield return new WaitWhile(() => bgmSource.isPlaying);

            yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 121f));
        }
    }

    public IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume) {
        float currentTime = 0;
        audioSource.volume = 0;

        while (currentTime < duration) {

            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public IEnumerator PlayRun(float runInterval) {
        float checkInterval = 0.1f;
        while (true) {
            if (gameManager.gameState == GameState.playing && playerController.IsOnGround()) {
                Run();
                yield return new WaitForSeconds(runInterval);
            }
            else {
                yield return new WaitForSeconds(checkInterval);
            }
        }
    }

}