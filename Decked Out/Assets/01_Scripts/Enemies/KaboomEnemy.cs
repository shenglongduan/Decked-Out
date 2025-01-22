using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KaboomEnemy : MonoBehaviour
{
    public UnityEngine.Transform targetCastle;
    public float moveSpeed = 1f;
    private float original_Speed;
    public float damage = 10.0f;
    public float maxHealth = 100.0f;
    public float currentHealth;
    //public Slider healthSlider;
    public GameObject zapPrefab;
    private bool hasBeenZapped = false;
    public bool isBurning = false;
    public GameObject effect;
    public GameObject deathEffectPrefab;
    private float damageTimer = 1.0f;
    public bool isFrozen = false;
    public int TotalFreezeTime = 3;
    public bool isTotalFrozen = false;
    private float timeSinceLastDamage = 0.0f;
    public AudioClip deathSound;
    private EnemyDeathSoundHandling deathSoundHandling;
    private EnemyKillTracker enemyKillTracker;
    private EnemyHealthFlash healthFlash;
    float _yPos;
    SpriteRenderer _spriteRenderer;

    //Attraction tower 

    private Transform originalTarget;
    public bool isAttracted;
    public bool isPoisoned;
    //Wave_Tower
    public bool isBeingPushed = false;
    bool _isDead = false;
    private void Start()
    {
        currentHealth = maxHealth;
        original_Speed = moveSpeed;
        timeSinceLastDamage = damageTimer;
        deathSoundHandling = GetComponent<EnemyDeathSoundHandling>();
        deathSoundHandling.enemyDeathSound = deathSound;
        enemyKillTracker = GameObject.FindObjectOfType<EnemyKillTracker>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        healthFlash = GetComponent<EnemyHealthFlash>();
    }

    private void Update()
    {
        if (targetCastle != null)
        {
            Vector2 moveDirection = (targetCastle.position + new Vector3(0f, -1f, 0) - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

           // if (healthSlider != null)
            {
                //Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
               // healthSlider.transform.position = screenPosition + new Vector2(0, 70.0f);
            }
        }
        if (isBurning)
        {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= damageTimer)
            {
                timeSinceLastDamage = 0.0f;
                TakeDamage(20.0f);
            }
        }

        UpdateSortingLayer();
    }
    private void UpdateSortingLayer()
    {
        _yPos = transform.position.y;
        _yPos = -_yPos;
        _spriteRenderer.sortingOrder = (int)(_yPos * 100);
    }
    public void Insta_Kill()
    {
        Die();
    }
    public void Attracted(Transform attractionTower)
    {
        if (!isAttracted)
        {
            originalTarget = targetCastle;
            targetCastle = attractionTower;
            isAttracted = true;
            StartCoroutine(ResetAttracted());
        }
    }

    public void HandleWaveImpact(Vector2 direction ,float duration, float distance)
    {
        if (!isBeingPushed)
        {
            Vector2 oppositeDirection = -direction.normalized;
            isBeingPushed = true;
            StartCoroutine(ManualPushback(oppositeDirection, duration, distance));
        }
    }
    public IEnumerator ManualPushback(Vector2 direction, float duration, float distance)
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition - direction.normalized * distance;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isBeingPushed = false;
    }
    private IEnumerator ResetAttracted()
    {
        yield return new WaitForSeconds(5);
        targetCastle = originalTarget;
        isAttracted = false;
    }
    private void DealAOEDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy otherEnemy = collider.GetComponent<Enemy>();
                if (otherEnemy != null)
                {
                    otherEnemy.TakeDamage(10);
                }
            }
        }
    }

    public bool ImmuneToDamage { get; set; }
    public bool IsShielded { get; set; }
    public void TakeDamage(float damage)
    {
        if (IsShielded)
        {
            // Destroy the shield
            IsShielded = false;
            ImmuneToDamage = false;
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    if (child.gameObject.CompareTag("Shield"))
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }
        else
        {
            // Apply damage to health if not shielded
            currentHealth -= damage;
            UpdateEnemyHealthUI();

            if (currentHealth <= 0 && !_isDead)
            {
                Die();
            }
        }
    }
    private void Die()
    {
        _isDead = true;
        deathSoundHandling.PlayDeathSound();
        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);

        if (enemyKillTracker != null)
        {
            enemyKillTracker.EnemyKilled();
        }

        DealAOEDamage();

        //Destroy(healthSlider.gameObject);
        GameObject deathEffect_prefab = Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        Destroy(deathEffect_prefab, 10f);
        Destroy(gameObject);
        Destroy(deathEffect, 4.0f);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Castle castle = collision.gameObject.GetComponent<Castle>();
            if (castle != null)
            {
                castle.TakeDamage(damage);
            }
            enemyKillTracker.EnemyDestroyed();
            GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
            //Destroy(healthSlider.gameObject);
            Destroy(gameObject);
            Destroy(deathEffect, 4.0f);
            
        }

    }

    public void UpdateEnemyHealthUI()
    {
        //healthSlider.value = currentHealth;
        healthFlash.TakeDamage(currentHealth);
    }

    public void SetHealthSlider(Slider slider)
    {
        //healthSlider = slider;
    }
    public void setBurning()
    {
        isBurning = true;
    }
    public void SetPoisoning(bool poisoning)
    {
        isPoisoned = poisoning;
    }
    public void ApplyFreeze(float precentage)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            moveSpeed *= precentage;
            StartCoroutine(DisableFreezeAfterDuration(3.0f));
        }
    }
    private IEnumerator DisableFreezeAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        moveSpeed = original_Speed;
    }
    public void Zap()
    {
        if (!hasBeenZapped)
        {
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 6f);

            foreach (Collider2D enemyCollider in nearbyEnemies)
            {
                if (enemyCollider.CompareTag("Enemy") && enemyCollider.gameObject != this.gameObject)
                {
                    GameObject zapPrefabInstance = Instantiate(zapPrefab, transform.position, Quaternion.identity);
                    ZapProjectile zapProjectile = zapPrefabInstance.GetComponent<ZapProjectile>();

                    if (zapProjectile != null)
                    {
                        zapProjectile.SetTarget(enemyCollider.transform);
                        zapProjectile.SetDamage(30f);
                    }
                }
            }
        }
    }
    public void ResetZapFlag()
    {
        hasBeenZapped = true;
    }

    public void ApplyTotalFreeze()
    {
        if (!isFrozen)
        {
            isTotalFrozen = true;
            moveSpeed = 0;
            StartCoroutine(DisableTotalFreezeAfterDuration(TotalFreezeTime));
        }
    }
    private IEnumerator DisableTotalFreezeAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isTotalFrozen = false;
        moveSpeed = original_Speed;
    }

    public void ApplySpeedUp(float precentage)
    {
        moveSpeed *= precentage;
    }
    public void RemoveSpeedUp()
    {
        moveSpeed = original_Speed;
    }
}