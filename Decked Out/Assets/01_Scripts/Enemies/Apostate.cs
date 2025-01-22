using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Apostate : MonoBehaviour
{
    public UnityEngine.Transform targetCastle;
    public float moveSpeed = 1f;
    private float original_speed;
    public float damage = 10.0f;
    public float maxHealth;
    public float currentHealth;
    //public Slider healthSlider;
    public GameObject zapPrefab;
    public bool isBurning = false;
    private bool hasBeenZapped = false;
    private float damageTimer = 1.0f;
    public bool isFrozen = false;
    public int TotalFreezeTime = 3;
    public bool isTotalFrozen = false;
    private float timeSinceLastDamage = 0.0f;
    public AudioClip deathSound;
    public GameObject deathEffectPrefab;
    private EnemyDeathSoundHandling deathSoundHandling;
    private EnemyKillTracker _killTracker;
    [SerializeField] private CircleCollider2D circleCollider;
    public float detectionRadius;
    private HashSet<GameObject> previouslyDetected = new HashSet<GameObject>();
    float _yPos;
    SpriteRenderer _spriteRenderer;
    EnemyHealthFlash healthFlash;

    //Attraction tower 

    private Transform originalTarget;
    public bool isAttracted;
    public bool isPoisoned;

    //Wave_Tower
    public bool isBeingPushed = false;

    bool _isDead = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = damageTimer;
        original_speed = moveSpeed;
        deathSoundHandling = GetComponent<EnemyDeathSoundHandling>();
        deathSoundHandling.enemyDeathSound = deathSound;
        _killTracker = GameObject.FindObjectOfType<EnemyKillTracker>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        healthFlash = GetComponent<EnemyHealthFlash>();
    }
    public void Insta_Kill()
    {
        Die();
    }
    private void Update()
    {
        HashSet<GameObject> currentlyDetected = new HashSet<GameObject>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Placed"))
            {
                currentlyDetected.Add(hit.gameObject);
                // If not previously detected, disable scripts
                if (!previouslyDetected.Contains(hit.gameObject))
                {
                    SetScriptsEnabled(hit.gameObject, false);
                }
            }
            if (hit.CompareTag("Tower"))
            {
                currentlyDetected.Add(hit.gameObject);
                // If not previously detected, disable scripts
                if (!previouslyDetected.Contains(hit.gameObject))
                {
                    SetScriptsEnabled(hit.gameObject, false);
                }
            }
            if (hit.CompareTag("Buffer"))
            {
                currentlyDetected.Add(hit.gameObject);
                // If not previously detected, disable scripts
                if (!previouslyDetected.Contains(hit.gameObject))
                {
                    SetScriptsEnabled(hit.gameObject, false);
                }
            }
        }

        foreach (var obj in previouslyDetected)
        {
            if (obj != null && !currentlyDetected.Contains(obj)) 
            {
                SetScriptsEnabled(obj, true);
            }
        }

        previouslyDetected = currentlyDetected;
    
        if (targetCastle != null)
        {
            Vector2 moveDirection = (targetCastle.position + new Vector3(0f, -1f, 0) - transform.position).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            //if (healthSlider != null)
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

    void OnDestroy()
    {
        foreach (var obj in previouslyDetected)
        {
            if (obj != null) // Check if the object hasn't been destroyed
            {
                SetScriptsEnabled(obj, true);
            }
        }
    }
    void SetScriptsEnabled(GameObject obj, bool enabled)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = enabled;
        }
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

    private IEnumerator ResetAttracted()
    {
        yield return new WaitForSeconds(5);
        targetCastle = originalTarget;
        isAttracted = false;
    }
    private void Die()
    {
        _isDead = true;
        if (_killTracker != null)
        {
            _killTracker.EnemyKilled();
        }
        //deathSoundHandling.PlayDeathSound();

        //Destroy(healthSlider.gameObject);
        GameObject deathEffect_prefab = Instantiate(deathEffectPrefab, transform.position, transform.rotation);
        Destroy(deathEffect_prefab, 10f);
        Destroy(gameObject);

     
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
        if (collision.gameObject.CompareTag("Placed"))
        {
            MonoBehaviour[] scripts = collision.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false; 
            }
        }

        if (circleCollider == null) return;

        if (collision.gameObject.CompareTag("Field"))
        {
            circleCollider.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Placed"))
        {
            MonoBehaviour[] scripts = collision.gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true; 
            }
        }
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
        moveSpeed = original_speed;
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
        moveSpeed = original_speed;
    }

    public void ApplySpeedUp(float precentage)
    {
        moveSpeed *= precentage;
    }
    public void RemoveSpeedUp()
    {
        moveSpeed = original_speed;
    }
}