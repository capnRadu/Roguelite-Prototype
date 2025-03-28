using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private GameObject healthFillBar;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthFillBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1, 1);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
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
}
