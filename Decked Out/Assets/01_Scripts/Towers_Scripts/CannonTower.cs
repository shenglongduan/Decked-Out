

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CannonTower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject CannonBall;
    [SerializeField] private float Damage;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    public GameObject effect;
    private GameObject buffed;
    private float initialRateOfFire;
    private bool canAttack = true;
    private bool hasBeenBuffed = false;
    private Animator animator;
    private AudioSource audioSource;

    private float _cannonTowerAnimLength = 1.0f;

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
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioManager.SetSFXClip(AudioManager.SFXSound.Tower_Cannon_Shoot);
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("Cannon_Tower"))
            {
                _cannonTowerAnimLength = clip.length;
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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    ShootCannon(collider.transform);
                    break;
                }
            }
        }
    }

    private void ShootCannon(Transform target)
    {
        audioSource.Play();
        animator.SetBool("IsShooting", true);
        canAttack = false;

        StartCoroutine(ShootCannonBall(target));
        StartCoroutine(DeactivateAnimation());
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator ShootCannonBall(Transform target)
    {
        float offset = 0.25f;
        yield return new WaitForSeconds(_cannonTowerAnimLength - offset);
        GameObject cannonBall = Instantiate(CannonBall, transform.position, Quaternion.identity);
        CannonBall cannonBallScript = cannonBall.GetComponent<CannonBall>();
        cannonBallScript.SetTarget(target);
        cannonBallScript.SetDamage(Damage);
    }

    private IEnumerator DeactivateAnimation()
    {
        yield return new WaitForSeconds(_cannonTowerAnimLength);
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
