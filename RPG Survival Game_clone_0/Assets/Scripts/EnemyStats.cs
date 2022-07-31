using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    Rigidbody rb ;
    public Canvas canvas;
    public EnemyHealthBar enemyHealthBar;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;
    Animator anim;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        anim = GetComponent<Animator>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);

    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        anim.Play("Damage");

        enemyHealthBar.SetCurrentHealth(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            anim.Play("Death");
            canvas.gameObject.SetActive(false);
        }
    }
}
