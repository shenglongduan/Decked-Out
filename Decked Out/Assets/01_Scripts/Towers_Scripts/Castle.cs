using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Castle : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public float healthSliderCheat = 5f;

    public Slider healthSlider;
    public EndGameSplashManager endGame;
    [Range(0, 2500)]
    [SerializeField] long _deathVibrationDuration;
    private GameLoader _loader;
    private WaveManager wave;
    private int damage = 9999;

    HeartJiggle _heartJiggle;

    public float health { get { return currentHealth; } }

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    public void Initialize()
    {
        _heartJiggle = FindObjectOfType<HeartJiggle>();
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        wave = ServiceLocator.Get<WaveManager>();
        UpdateHealthUI();
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();
        _heartJiggle.StartJiggle(health);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        VibratorManager.Vibrate(_deathVibrationDuration);
        endGame.Death();
        //wave.StopWave();

    }
    public void ResetHealth()
    {
        currentHealth = 100;
        UpdateHealthUI();        
    }
    private void UpdateHealthUI()
    {
        if (currentHealth == 100)
        {
            healthSlider.value = currentHealth;
        }
        else if (currentHealth >= 99)
        {
            healthSlider.value = currentHealth - 10;
            healthSlider.value = Mathf.Clamp(healthSlider.value, 90, 100);
        }
        else if (currentHealth >= 89)
        {
            healthSlider.value = currentHealth - 8;
            healthSlider.value = Mathf.Clamp(healthSlider.value, 80, 90);
        }
        else if (currentHealth >= 79)
        {
            healthSlider.value = currentHealth - 6;
            healthSlider.value = Mathf.Clamp(healthSlider.value, 70, 80);
        }
        else if (currentHealth >= 69)
        {
            healthSlider.value = currentHealth - 4;
            healthSlider.value = Mathf.Clamp(healthSlider.value, 60, 70);
        }
        else if (currentHealth >= 59)
        {
            healthSlider.value = currentHealth - 2;
            healthSlider.value = Mathf.Clamp(healthSlider.value, 50, 60);
        }
        else if (currentHealth < 59)
        {
            healthSlider.value = currentHealth;
            healthSlider.value = Mathf.Clamp(healthSlider.value, 10, 50);
        }
        else if (currentHealth <= 10)
        {
            healthSlider.value = currentHealth + 2;
            healthSlider.value = Mathf.Clamp(healthSlider.value, 0, 10);
        }
    }

    

    public void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Wait(3, collision.gameObject));
    }

    private IEnumerator Wait(float time, GameObject enemy)
    {
       

            yield return new WaitForSeconds(time);
        if (enemy != null && enemy.gameObject != null)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                }
                KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
                if (kaboom != null)
                {
                    kaboom.TakeDamage(damage);
                }
                Apostate apostate = enemy.GetComponent<Apostate>();
                if (apostate != null)
                {
                    apostate.TakeDamage(damage);

                }
                Necromancer necromancer = enemy.GetComponent<Necromancer>();
                if (necromancer != null)
                {
                    necromancer.TakeDamage(damage);
                }
                Cleric cleric = enemy.GetComponent<Cleric>();
                if (cleric != null)
                {
                    cleric.TakeDamage(damage);
                }
                Aegis aegis = enemy.GetComponent<Aegis>();
                if (aegis != null)
                {
                    aegis.TakeDamage(damage);
                }
                Mopey_Misters mopey = enemy.GetComponent<Mopey_Misters>();
                if (mopey != null)
                {
                    mopey.TakeDamage(damage);
                }
            }
        }
    }
}
