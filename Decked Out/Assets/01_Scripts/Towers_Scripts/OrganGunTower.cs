using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganGunTower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject SmallBulletPrefab;
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
    private AudioSource audioSource;
    private AudioManager audioManager;

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
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public float range
    {
        get { return attackRange; }
        set { attackRange = value; }
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
                if (collider.CompareTag("Enemy"))
                {
                    int range = Random.Range(1, 3);
                    if(range == 1)
                    {
                        audioSource.clip = audioManager.SetSFXClip(AudioManager.SFXSound.Tower_OrganShot_Var1);
                    }
                    else if(range == 2)
                    {
                        audioSource.clip = audioManager.SetSFXClip(AudioManager.SFXSound.Tower_OrganShot_Var2);
                    }
                    else
                    {
                        audioSource.clip = audioManager.SetSFXClip(AudioManager.SFXSound.Tower_OrganShot_Var3);
                    }
                    audioSource.Play();
                    ShootInAnyDirection();
                    break;
                }
            }
        }
    }

    private void ShootInAnyDirection()
    {
        Vector2[] directions = new Vector2[]
        {
        Vector2.up, Vector2.down, Vector2.left, Vector2.right,
        new Vector2(-1, 1).normalized, new Vector2(1, 1).normalized,
        new Vector2(-1, -1).normalized, new Vector2(1, -1).normalized
        };
        foreach (Vector2 direction in directions)
        {
            GameObject smallBullet = Instantiate(SmallBulletPrefab, transform.position, Quaternion.identity);
            SmallBullet bulletScript = smallBullet.GetComponent<SmallBullet>();
            bulletScript.SetDamage(Damage);
            bulletScript.SetDirection(direction);
            bulletScript.SetAttackRange(attackRange);
            smallBullet.transform.SetParent(transform);
            //Debug.Log("Creating bullet with damage: " + Damage + ", direction: " + direction + ", and attack range: " + attackRange);
        }

        canAttack = false;
        StartCoroutine(AttackCooldown());
    }

    public void ResetTowerEffects()
    {
        Damage = initialDamage;
        RateOfFire = initialRateOfFire;
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
