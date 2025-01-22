using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Necromancer : MonoBehaviour
{
    public UnityEngine.Transform targetCastle;
    public float moveSpeed = 1f;
    private float original_moveSpeed;
    public float damage = 10.0f;
    private HashSet<GameObject> detectedEnemies = new HashSet<GameObject>();
    public float maxHealth;
    public float currentHealth;
    private float RateOfFire = 2;
    //public Slider healthSlider;
    public GameObject zapPrefab;
    public bool isBurning = false;
    private bool canAttack = true;
    private bool hasBeenZapped = false;
    public float detectionRadius;
    private float damageTimer = 1.0f;
    public bool isFrozen = false;
    public int TotalFreezeTime = 3;
    public bool isTotalFrozen = false;
    public GameObject deathEffectPrefab;
    private HashSet<GameObject> detectedEnemy = new HashSet<GameObject>();
    private float timeSinceLastDamage = 0.0f;
    public AudioClip deathSound;
    private EnemyDeathSoundHandling deathSoundHandling;
    private EnemyKillTracker _killTracker;
    private EnemyHealthFlash healthFlash;
    [SerializeField] private CircleCollider2D circleCollider;

    private float spawnCooldown = 3f; 
    private float lastSpawnTime;

    //Attraction tower 

    private Transform originalTarget;
    public bool isAttracted;
    public bool isPoisoned;

    //Wave_Tower
    public bool isBeingPushed = false;
    EnemyDeathAnimation _enemyDeathAnimation;
    CapsuleCollider2D _capsuleCollider;
    bool _isDead = false;
    float _yPos;
    SpriteRenderer _spriteRenderer;

    private WaveManager wave;
    private GameLoader _loader;
    private void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = damageTimer;
        original_moveSpeed = moveSpeed;
        deathSoundHandling = GetComponent<EnemyDeathSoundHandling>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
        deathSoundHandling.enemyDeathSound = deathSound;
        _killTracker = GameObject.FindObjectOfType<EnemyKillTracker>();
        _enemyDeathAnimation = GetComponent<EnemyDeathAnimation>();
        healthFlash = GetComponent<EnemyHealthFlash>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        StartCoroutine(DetectAndAddAcolyteCoroutine());
        lastSpawnTime = -spawnCooldown; // Initialize last spawn time
    }

    public void Initialize()
    {
        wave = ServiceLocator.Get<WaveManager>();
    }

    private IEnumerator DetectAndAddAcolyteCoroutine()
    {
        while (true)
        {
            DetectAndAddAcolyte();
            yield return new WaitForSeconds(2f);
        }
    }

    private void Update()
    {
        if (targetCastle != null)
        {
            Vector2 moveDirection = (targetCastle.position + new Vector3(0f, -1f, 0) - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

           // if (healthSlider != null)
            {
               // Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
               // healthSlider.transform.position = screenPosition + new Vector2(0, 70.0f);
            }
        }
        if (isBurning)
        {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= damageTimer)
            {
                timeSinceLastDamage = 0.0f;
                TakeDamage(10.0f);
            }
        }

        UpdateSortingLayer();
    }

    public void Insta_Kill()
    {
        Die();
    }

    private void UpdateSortingLayer()
    {
        _yPos = transform.position.y;
        _yPos = -_yPos;
        _spriteRenderer.sortingOrder = (int)(_yPos * 100);
    }

    public void HandleWaveImpact(Vector2 direction, float duration, float distance)
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

    private void DetectAndAddAcolyte()
    {
        if (Time.time - lastSpawnTime < spawnCooldown) return; // Check cooldown

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D collider in detectedObjects)
        {
            GameObject detectedObject = collider.gameObject;
            if (detectedEnemies.Contains(detectedObject))
            {
                continue; // Skip already detected enemies
            }

            // Add the object to the HashSet to prevent reprocessing
            detectedEnemies.Add(detectedObject);

            if (collider.CompareTag("Kaboom"))
            {
                wave.AddEnemyToCurrentWave("Kaboom", collider.transform.position);
            }
            else if (collider.CompareTag("Golem"))
            {
                wave.AddEnemyToCurrentWave("Golem", collider.transform.position);
            }
            else if (collider.CompareTag("Aegis"))
            {
                wave.AddEnemyToCurrentWave("Aegis", collider.transform.position);
            }
            else if (collider.CompareTag("Cleric"))
            {
                wave.AddEnemyToCurrentWave("Cleric", collider.transform.position);
            }
        }

        lastSpawnTime = Time.time; // Update last spawn time
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        float actualRateOfFire = RateOfFire;

        while (!canAttack)
        {
            yield return new WaitForSeconds(actualRateOfFire);
            canAttack = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
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

    public void SetPoisoning(bool poisoning)
    {
        isPoisoned = poisoning;
    }

    private IEnumerator ResetAttracted()
    {
        yield return new WaitForSeconds(5);
        targetCastle = originalTarget;
        isAttracted = false;
    }

    private IEnumerator reset_Field()
    {
        yield return new WaitForSeconds(2);
    }

    private void Die()
    {
        _isDead = true;
        moveSpeed = 0;
        _capsuleCollider.enabled = false;
        deathSoundHandling.PlayDeathSound();
        if (_killTracker != null)
        {
            _killTracker.EnemyKilled();
        }
        float deathAnimationDuration = _enemyDeathAnimation.PlayDeathAnimation();
        //healthSlider.gameObject.SetActive(false);
        //Destroy(healthSlider.gameObject, deathAnimationDuration);
        GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        Destroy(deathEffect, 10f);
        Destroy(gameObject, 0.4f);
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
            _killTracker.EnemyDestroyed();
            //Destroy(healthSlider.gameObject);
            Destroy(gameObject);
        }
        if (circleCollider == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Field"))
        {
            circleCollider.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (circleCollider == null) return;

        if (collision.gameObject.CompareTag("Field"))
        {
            circleCollider.enabled = false;
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

    public void ApplyFreeze(float percentage)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            moveSpeed *= percentage;
            StartCoroutine(DisableFreezeAfterDuration(3.0f));
        }
    }

    private IEnumerator DisableFreezeAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        moveSpeed = original_moveSpeed;
    }

    public void Zap()
    {
        if (!hasBeenZapped)
        {
            Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);

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
        moveSpeed = original_moveSpeed;
    }

    public void ApplySpeedUp(float percentage)
    {
        moveSpeed *= percentage;
    }

    public void RemoveSpeedUp()
    {
        moveSpeed = original_moveSpeed;
    }
}