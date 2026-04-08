using UnityEngine;

public class GameAudioManager : MonoBehaviour {
    [SerializeField] private AudioClip run;
    [SerializeField] private AudioClip jupm;
    [SerializeField] private AudioClip changeLane;
    [SerializeField] private AudioClip optionSelect;
    [SerializeField] private AudioClip mainBgm;
    [SerializeField] private AudioClip startingScreenBgm;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    public void PlayJump() {
        sfxSource.PlayOneShot(jupm);
    }
    public void PlayRun() {
        sfxSource.PlayOneShot(run);
    }
    public void PlayChangelane() {
        sfxSource.PlayOneShot(changeLane);
    }
    public void PlayOptionSelect() {
        sfxSource.PlayOneShot(optionSelect);
    }

    public void PlayMainBGM() {
        bgmSource.clip = mainBgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayStartingBGM() {
        bgmSource.clip = startingScreenBgm;
        bgmSource.loop = false;
        bgmSource.Play();
    }

    public void StopSoundSource(AudioSource soundSource) {
        soundSource.Stop();
    }

}