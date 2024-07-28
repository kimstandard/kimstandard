using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBarImage; // HP 바 이미지
    public float maxHealth = 100f; // 최대 HP
    private float currentHealth;

    private GameManager gameManager;    

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        // GameManager 찾기 (씬에 단 하나의 GameManager가 있다고 가정)
        gameManager = FindObjectOfType<GameManager>();        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(20f); // 적과 충돌 시 10의 데미지를 받음
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthBar();

        // 체력이 0이 되면 GameManager의 GameOver 함수 호출
        if (currentHealth <= 0)
        {
            gameManager.GameOver();
        }
    }

    void UpdateHealthBar()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }
}
