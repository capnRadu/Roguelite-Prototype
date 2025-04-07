using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private GameObject healthFillBar;

    private void Awake()
    {
        if (gameObject.GetComponent<PlayerMovement>() != null && HealthTracker.Instance != null && HealthTracker.Instance.currentLoopHealth != 0) 
        {
            currentHealth = HealthTracker.Instance.currentLoopHealth;
            healthFillBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            currentHealth = maxHealth + (20f * EnemyTracker.Instance.GetBossHpMultiplier());
            maxHealth = currentHealth;
            healthFillBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
        }
        else
        {
            currentHealth = maxHealth;
        }
        
        Debug.Log("Health: " + currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthFillBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
            EnemyTracker.Instance.OnEnemyDeath(1);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    
    private IEnumerator FlashRed()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.material.color = Color.white;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
