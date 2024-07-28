using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{

    public static CanvasManager instance;

    public Image BackGround;
    public Button OptionButton;
    public Button StartButton;
    public GameObject gameOverText;
    public TextMeshProUGUI TimeText;    
    public TextMeshProUGUI currentBullet;

    void Awake()
    {
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

    public void ShowGameOverText(bool show)
    {
        if (gameOverText != null)
        {
            gameOverText.SetActive(show);
        }
    }
}
