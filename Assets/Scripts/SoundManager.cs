using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioSource> musicSources; // �پ��� ���� �ҽ� ����Ʈ
    public AudioSource btnSource;

    private void Awake()
    {
        // �̱��� �������� �ߺ� ���� ����
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
        LoadVolumeSettings(); // ���� �ε�� ������ ���� ������ �ٽ� ����

        if (scene.name == "Dust")
        {
            PlayMusic(1); 
        }
    }

    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicSources.Count)
        {
            StopMusic(); // ���� ��� ���� ������ ����
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



