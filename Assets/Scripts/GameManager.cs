using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject Options;
    public Button StartB;
    public Button OptionOn;
    public Button OptionXsign;

    

    bool isGameover;
    public float startSurviveTime = 10f; // 시작 생존 시간 10초
    private float surviveTime;
    private bool isGamePaused = false;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 이벤트 추가
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeButtons();
        surviveTime = startSurviveTime;
        isGameover = false;
    }

    void Update()
    {
        if (!isGameover && SceneManager.GetActiveScene().name != "Main" && SceneManager.GetActiveScene().name == "Dust")
        {
            surviveTime -= Time.deltaTime;
            if (surviveTime < 0)
            {
                surviveTime = 0;
            }
            TimeSpan timeSpan = TimeSpan.FromSeconds(surviveTime);
            CanvasManager.instance.TimeText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

            if (surviveTime <= 0f)
            {
                //ClearText.gameObject.SetActive(true);
                StartCoroutine(LoadUpgradeSceneAfterDelay(5f));
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Options.activeSelf)
            {
                Xsign();
            }
            else
            {
                OptionsOn();
            }
        }
    }

    IEnumerator LoadUpgradeSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 실시간으로 5초 대기
        Time.timeScale = 1f; // 씬 전환 전 타임스케일 복구
        //SceneManager.LoadScene("Upgrade"); // 업그레이드 씬으로 이동
    }

    void StartButton()
    {
        LoadingScene.LoadScene("Dust");
    }

    void OptionsOn()
    {
        Options.SetActive(true);
        PauseGame();        
        FirstPersonController.instance.SetMouseControl(false);
        SetCursorVisible(true);
    }

    void Xsign()
    {
        Options.SetActive(false);
        ResumeGame();        
        FirstPersonController.instance.SetMouseControl(true);

        if (SceneManager.GetActiveScene().name != "Main")
        {
            SetCursorVisible(false);
        }
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f; // 기본 게임 시간으로 복구
    }

    public void SetCursorVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindUIElementsWithDelay());

        if (scene.name == "Dust")
        {
            CanvasManager.instance?.BackGround.gameObject.SetActive(false);
            CanvasManager.instance?.OptionButton.gameObject.SetActive(false);
            CanvasManager.instance?.StartButton.gameObject.SetActive(false);
            CanvasManager.instance?.TimeText.gameObject.SetActive(true);
            CanvasManager.instance?.currentBullet.gameObject.SetActive(true);            
        }
    }

    private IEnumerator FindUIElementsWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // 짧은 지연 시간

        // 새로운 씬에서 필요한 오브젝트와 버튼을 다시 찾음
        Options = Utils.FindInactiveObjectByName("Options");
        StartB = Utils.FindInactiveObjectByName("Start")?.GetComponent<Button>();
        OptionOn = Utils.FindInactiveObjectByName("OptionB")?.GetComponent<Button>();
        OptionXsign = Utils.FindInactiveObjectByName("XB")?.GetComponent<Button>();

        InitializeButtons();
    }

    void InitializeButtons()
    {
        // 버튼이 null이 아닌 경우에만 리스너 추가
        if (OptionOn != null) OptionOn.onClick.AddListener(OptionsOn);
        if (OptionXsign != null) OptionXsign.onClick.AddListener(Xsign);
        if (StartB != null) StartB.onClick.AddListener(StartButton);
    }

    public void GameOver()
    {
        isGameover = true;
        PauseGame();
        CanvasManager.instance?.ShowGameOverText(true); // 게임 오버 텍스트 활성화
        StartCoroutine(LoadMainSceneAfterDelay(5f)); // 5초 후에 메인 씬으로 이동
    }

    IEnumerator LoadMainSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 실시간으로 5초 대기
        Time.timeScale = 1f; // 씬 전환 전 타임스케일 복구
        CanvasManager.instance?.ShowGameOverText(false); // 게임 오버 텍스트 비활성화
        SceneManager.LoadScene("Main"); // 메인 씬으로 이동
        SetCursorVisible(true); // 씬 전환 후 마우스 커서 보이기
    }    
}
