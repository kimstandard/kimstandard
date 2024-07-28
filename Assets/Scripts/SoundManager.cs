using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioSource> musicSources; // 다양한 음악 소스 리스트
    public AudioSource btnSource;

    private void Awake()
    {
        // 싱글톤 패턴으로 중복 생성 방지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadVolumeSettings(); // 씬이 로드될 때마다 볼륨 설정을 다시 적용

        if (scene.name == "Dust")
        {
            PlayMusic(1); 
        }
    }

    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicSources.Count)
        {
            StopMusic(); // 현재 재생 중인 음악을 중지
            musicSources[index].Play();
        }
        else
        {
            Debug.LogWarning("Invalid music index");
        }
    }

    public void StopMusic()
    {
        foreach (var source in musicSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        foreach (var source in musicSources)
        {
            source.volume = volume;
        }
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetButtonVolume(float volume)
    {
        if (btnSource != null)
        {
            btnSource.volume = volume;
            PlayerPrefs.SetFloat("ButtonVolume", volume);
            PlayerPrefs.Save();
        }
    }

    public void ONSfx()
    {
        if (btnSource != null)
        {
            btnSource.Play();
        }
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            SetMusicVolume(musicVolume);
        }

        if (PlayerPrefs.HasKey("ButtonVolume"))
        {
            float buttonVolume = PlayerPrefs.GetFloat("ButtonVolume");
            btnSource.volume = buttonVolume;
        }
    }
}



