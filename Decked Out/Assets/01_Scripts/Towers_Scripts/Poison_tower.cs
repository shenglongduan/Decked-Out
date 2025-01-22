

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Poison_tower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject poision_prefab;
    [SerializeField] private float Damage;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    private float initialRateOfFire;
    public GameObject effect;
    private GameObject buffed;
    private bool canAttack = true;
    private List<GameObject> recentlyShotEnemies = new List<GameObject>();
    private bool hasBeenBuffed = false;
    public AudioSource audioSource;
    public AudioManager audioManager;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        FindAndShootTarget();
        if (health == 0)
        {
            spriteRenderer.color = Color.red;
        }
    }
    private void Start()
    {
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public float damage
    {
        get { return Damage; }
        set { Damage = value; }
    }
    public float attackSpeed
    {
        get { return RateOfFire; }
        set { RateOfFire = value; }
    }
    public float range
    {
        get { return attackRange; }
        set { attackRange = value; }
    }
    GameObject ITower.gameObject
    {
        get { return towerGameObject; }
        set { towerGameObject = value; }
    }
    public float health
    {
        get { return Health; }
        set { Health = value; }
    }
    public void ApplyBuff(float damageBuff, float rateOfFireBuff)
    {
        if (!hasBeenBuffed && !gameObject.CompareTag("Empty"))
        {
            Damage += damageBuff;
            RateOfFire *= rateOfFireBuff;
            if (RateOfFire < 0.1f)
            {
                RateOfFire = 0.1f;
            }
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null && health != 0)
            {
                buffed = Instantiate(effect, transform.position, Quaternion.identity);
            }
            hasBeenBuffed = true;

        }
    }
    private void FindAndShootTarget()
    {
        if (canAttack)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy") && !recentlyShotEnemies.Contains(collider.gameObject))
                {
                    recentlyShotEnemies.Add(collider.gameObject);
                    ShootArrow(collider.transform);
                    break;
                }
            }
        }
    }

    private void ShootArrow(Transform target)
    {
        AudioManager.Instance.playSFXClip(AudioManager.SFXSound.Tower_Poison_Shot, gameObject.GetComponent<AudioSource>());
        GameObject arrow = Instantiate(poision_prefab, transform.position, Quaternion.identity);
        Poison_Projectile arrowScript = arrow.GetComponent<Poison_Projectile>();
        arrowScript.SetTarget(target);

        canAttack = false;
        StartCoroutine(AttackCooldown());

    }
    public void ResetTowerEffects()
    {
        Damage = initialDamage;
        RateOfFire = initialRateOfFire;
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color defaultColor = Color.white;
            spriteRenderer.color = defaultColor;
        }
        Destroy(buffed);
        hasBeenBuffed = false;
    }

    public float GetAttackRange()
    {
        return attackRange;
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
    private void OnDestroy()
    {
        if (buffed != null)
        {
            Destroy(buffed);
        }
    }
}
