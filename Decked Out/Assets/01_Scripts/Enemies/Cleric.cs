using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cleric : MonoBehaviour
{
    public UnityEngine.Transform targetCastle;
    public float moveSpeed = 1f;
    private float original_moveSpeed;
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
    public GameObject deathEffectPrefab;
    private float timeSinceLastDamage = 0.0f;
    public AudioClip deathSound;
    private EnemyDeathSoundHandling deathSoundHandling;
    private EnemyKillTracker _killTracker;
    [SerializeField] private CircleCollider2D circleCollider;
    float _yPos;
    SpriteRenderer _spriteRenderer;

    [Header("Healing Configuration")]
    public float healingRange = 5f; // The range within which enemies can be healed
    public float healAmount = 25f; // The amount of health to restore
    //Attraction tower 

    private Transform originalTarget;
    public bool isAttracted;

    public bool isPoisoned;

    //Wave_Tower
    public bool isBeingPushed = false;
    EnemyDeathAnimation _enemyDeathAnimation;
    EnemyHealthFlash _healthFlash;
    CapsuleCollider2D _capsuleCollider;
    bool _isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        timeSinceLastDamage = damageTimer;
        original_moveSpeed = moveSpeed;
        deathSoundHandling = GetComponent<EnemyDeathSoundHandling>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathSoundHandling.enemyDeathSound = deathSound;
        _killTracker = GameObject.FindObjectOfType<EnemyKillTracker>();
        _enemyDeathAnimation = GetComponent<EnemyDeathAnimation>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _healthFlash = GetComponent<EnemyHealthFlash>();
        InvokeRepeating("HealNearbyEnemies", 3.0f, 3.0f);
    }
    void HealNearbyEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, healingRange);
        foreach (Collider2D enemy in enemiesInRange)
        {
         
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.currentHealth += healAmount;
                    enemyScript.UpdateEnemyHealthUI(); 
                }
                KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
                if (kaboom != null)
                {
                    kaboom.currentHealth += healAmount;
                    kaboom.UpdateEnemyHealthUI();
                }
                Apostate apostate = enemy.GetComponent<Apostate>();
                if (apostate != null)
                {
                    apostate.currentHealth += healAmount;
                    apostate.UpdateEnemyHealthUI();
                }
            Mopey_Misters _Mopey = enemy.GetComponent<Mopey_Misters>();
            if (_Mopey != null)
            {
                _Mopey.currentHealth += healAmount;
                _Mopey.UpdateEnemyHealthUI();
            }
            Necromancer necromancer = enemy.GetComponent<Necromancer>();
                if (necromancer != null)
                {
                    necromancer.currentHealth += healAmount;
                    necromancer.UpdateEnemyHealthUI();
                }
            Aegis aegis = enemy.GetComponent<Aegis>();
            if (aegis != null)
            {
                aegis.currentHealth += healAmount;
                aegis.UpdateEnemyHealthUI();
            }

        }
    }
    private void Update()
    {
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
                TakeDamage(10.0f);
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
    private IEnumerator reset_Field()
    {
        yield return new WaitForSeconds(2);
    }
    private void Die()
    {
        _isDead = true;
        if (_killTracker != null)
        {
            _killTracker.EnemyKilled();
        }
        deathSoundHandling.PlayDeathSound();

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
        if (circleCollider == null)
        {

            // if (collision.gameObject.CompareTag("Field"))
            // {
            //     Field force_Field = collision.gameObject.GetComponent<Field>();
            //     force_Field.StartFlickerEffect();
            //     StartCoroutine(reset_Field());
            //     force_Field.ResetFieldPrefabChanges();
            // }

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
    private void UpdateEnemyHealthUI()
    {
        //healthSlider.value = currentHealth;
        _healthFlash.TakeDamage(currentHealth);

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
        moveSpeed = original_moveSpeed;
    }
    public void Insta_Kill()
    {
        Die();
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

    public void ApplySpeedUp(float precentage)
    {
        moveSpeed *= precentage;
    }
    public void RemoveSpeedUp()
    {
        moveSpeed = original_moveSpeed;
    }
}