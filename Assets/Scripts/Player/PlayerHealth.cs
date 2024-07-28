using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBarImage; // HP �� �̹���
    public float maxHealth = 100f; // �ִ� HP
    private float currentHealth;

    private GameManager gameManager;    

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        // GameManager ã�� (���� �� �ϳ��� GameManager�� �ִٰ� ����)
        gameManager = FindObjectOfType<GameManager>();        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(20f); // ���� �浹 �� 10�� �������� ����
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

        // ü���� 0�� �Ǹ� GameManager�� GameOver �Լ� ȣ��
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
