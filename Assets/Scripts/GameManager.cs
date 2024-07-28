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
    public float startSurviveTime = 10f; // ���� ���� �ð� 10��
    private float surviveTime;
    private bool isGamePaused = false;

    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // �� �ε� �̺�Ʈ �߰�
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
        yield return new WaitForSecondsRealtime(delay); // �ǽð����� 5�� ���
        Time.timeScale = 1f; // �� ��ȯ �� Ÿ�ӽ����� ����
        //SceneManager.LoadScene("Upgrade"); // ���׷��̵� ������ �̵�
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
        Time.timeScale = 1f; // �⺻ ���� �ð����� ����
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
        yield return new WaitForSeconds(0.1f); // ª�� ���� �ð�

        // ���ο� ������ �ʿ��� ������Ʈ�� ��ư�� �ٽ� ã��
        Options = Utils.FindInactiveObjectByName("Options");
        StartB = Utils.FindInactiveObjectByName("Start")?.GetComponent<Button>();
        OptionOn = Utils.FindInactiveObjectByName("OptionB")?.GetComponent<Button>();
        OptionXsign = Utils.FindInactiveObjectByName("XB")?.GetComponent<Button>();

        InitializeButtons();
    }

    void InitializeButtons()
    {
        // ��ư�� null�� �ƴ� ��쿡�� ������ �߰�
        if (OptionOn != null) OptionOn.onClick.AddListener(OptionsOn);
        if (OptionXsign != null) OptionXsign.onClick.AddListener(Xsign);
        if (StartB != null) StartB.onClick.AddListener(StartButton);
    }

    public void GameOver()
    {
        isGameover = true;
        PauseGame();
        CanvasManager.instance?.ShowGameOverText(true); // ���� ���� �ؽ�Ʈ Ȱ��ȭ
        StartCoroutine(LoadMainSceneAfterDelay(5f)); // 5�� �Ŀ� ���� ������ �̵�
    }

    IEnumerator LoadMainSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // �ǽð����� 5�� ���
        Time.timeScale = 1f; // �� ��ȯ �� Ÿ�ӽ����� ����
        CanvasManager.instance?.ShowGameOverText(false); // ���� ���� �ؽ�Ʈ ��Ȱ��ȭ
        SceneManager.LoadScene("Main"); // ���� ������ �̵�
        SetCursorVisible(true); // �� ��ȯ �� ���콺 Ŀ�� ���̱�
    }    
}
