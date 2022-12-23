using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [HideInInspector] public int maxHealth;
    [ReadOnly] public int currentHealth;

    [ReadOnly] public float defensePercentage;

    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void setHealth(int newHealth)
    {
        currentHealth = newHealth - (maxHealth - currentHealth);
        maxHealth = newHealth;

        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            gameObject.SetActive(false);
        }
    }

    public void setDefense(float newDefense) => defensePercentage = 1 - (newDefense / (newDefense + 100));

    public void TakeDamage(int damage)
    {
        currentHealth -= Mathf.RoundToInt(damage * defensePercentage);

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}