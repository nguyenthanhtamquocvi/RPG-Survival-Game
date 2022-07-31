using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Stats")]
    [HideInInspector] public HealthBar healthBar;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    [Header("Stamina Stats")]
    [HideInInspector] public StaminaBar staminaBar;
    public int staminaLevel = 10;
    public int maxStamina;
    public int currentStamina;



    AnimationHandler animationHandler;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        animationHandler = GetComponentInChildren<AnimationHandler>();
        animationHandler.Initialaze();
    }

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }


    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animationHandler.PlayTargetAnimation("Damage", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animationHandler.PlayTargetAnimation("Death", true);
        }
    }

    public void TakeStamina(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }
}
