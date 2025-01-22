using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElectricTower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject zapPrefab;
    [SerializeField] private float Damage;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float Health = 2;
    private float initialDamage;
    private float initialRateOfFire;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    public GameObject effect;
    private GameObject buffed;
    private bool canAttack = true;
    private bool hasBeenBuffed = false;
    private AudioSource audioSource;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void Start()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioManager.SetSFXClip(AudioManager.SFXSound.Tower_Electricity_Shot);
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        FindAndShootTarget();
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
                    ShootArrow(collider.transform);
                    break;
                }
            }
        }
    }

    private void ShootArrow(Transform target)
    {
        audioSource.Play();
        GameObject Zap = Instantiate(zapPrefab, transform.position, Quaternion.identity);
        ZapProjectile Script = Zap.GetComponent<ZapProjectile>();
        Script.SetTarget(target);

        canAttack = false;
        Script.SetDamage(Damage);
        StartCoroutine(AttackCooldown());
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
}
