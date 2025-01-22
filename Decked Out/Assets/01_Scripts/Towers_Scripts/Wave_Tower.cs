using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Tower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject Wave_Slash_Prefab;
    [SerializeField] private float Damage;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float Health = 2;
    public GameObject effect;
    private GameObject buffed;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    private float initialRateOfFire;
    private bool canAttack = true;
    private bool hasBeenBuffed = false;
    private Animator animator;
    public AudioSource audioSource;

    private float _waveTowerAnimLength = 1.0f;

    private List<GameObject> recentlyShotEnemies = new List<GameObject>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        FindAndShootTarget();
    }

    private void Start()
    {
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("WaveTower_Animation"))
            {
                _waveTowerAnimLength = clip.length;
                break;
            }
        }
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
            List<GameObject> newTargets = new List<GameObject>();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    if (!recentlyShotEnemies.Contains(collider.gameObject))
                    {
                        newTargets.Add(collider.gameObject);
                    }
                }
            }

            GameObject target = null;
            if (newTargets.Count > 0)
            {
                target = newTargets[0];
                recentlyShotEnemies.Add(target);
            }
            else if (recentlyShotEnemies.Count > 0)
            {
                target = recentlyShotEnemies[0];
                recentlyShotEnemies.RemoveAt(0);
            }

            if (target != null)
            {
                ShootWave(target.transform);
            }
        }
    }

    private void ShootWave(Transform target)
    {
        animator.SetBool("IsShooting", true);
        // audioSource.Play();
        GameObject Slash_Prefab = Instantiate(Wave_Slash_Prefab, transform.position, Quaternion.identity);
        Wave_Projectile waveScript = Slash_Prefab.GetComponent<Wave_Projectile>();
        waveScript.SetTarget(target);

        canAttack = false;
        waveScript.SetDamage(Damage);
        StartCoroutine(DeactivateAnimation());
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator DeactivateAnimation()
    {
        yield return new WaitForSeconds(_waveTowerAnimLength);
        animator.SetBool("IsShooting", false);
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
