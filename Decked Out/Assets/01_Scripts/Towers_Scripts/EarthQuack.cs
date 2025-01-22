using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuack : MonoBehaviour, ITower
{
    public float attackRange = 5.0f;
    [SerializeField] private float Damage;
    private float RateOfFire = 1.0f;
    public GameObject effect;
    public GameObject buff_prefab;
    private GameObject buffed;

    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    private float initialRateOfFire;
    private bool hasBeenBuffed = false;
    private AudioManager audioManager;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        StartCoroutine(DamageOverTime());
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();
                Apostate apostate = collider.GetComponent<Apostate>();
                Aegis aegis = collider.GetComponent<Aegis>();
                Mopey_Misters mopey = collider.GetComponent<Mopey_Misters>();
                Cleric cleric = collider.GetComponent<Cleric>();

                if (enemy != null)
                {
                    enemy.ApplyFreeze(0.5f);
                }
                if (kaboom != null)
                {
                    kaboom.ApplyFreeze(0.5f);
                }
                if (apostate != null)
                {
                    apostate.ApplyFreeze(0.5f);
                }
                if (cleric != null)
                {
                    cleric.ApplyFreeze(0.5f);
                }
                if (mopey != null)
                {
                    mopey.ApplyFreeze(0.5f);
                }
                if (aegis != null)
                {
                    aegis.ApplyFreeze(0.5f);
                }
            }
        }
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
    private IEnumerator DamageOverTime()
    {
        while (true) 
        {

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();
                    Mopey_Misters _mopey = collider.GetComponent<Mopey_Misters>();
                    Apostate apostate = collider.GetComponent<Apostate>();
                    Necromancer necromancer = collider.GetComponent<Necromancer>();
                    Aegis aegis = collider.GetComponent<Aegis>();
                    Cleric cleric = collider.GetComponent<Cleric>();

                    audioManager.playSFXClip(AudioManager.SFXSound.Tower_Earthquake_Impact, gameObject.GetComponent<AudioSource>());

                    if (enemy != null)
                    {
                        enemy.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                    if (kaboom != null)
                    {
                        kaboom.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }

                    if (apostate != null)
                    {
                        apostate.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                    if (necromancer != null)
                    {
                        necromancer.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                    if (aegis != null)
                    {
                        aegis.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                    if (cleric != null)
                    {
                        cleric.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                    if (_mopey != null)
                    {
                        _mopey.TakeDamage(Damage);
                        GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
                        Destroy(deathEffect, 0.5f);
                    }
                }
            }
            yield return new WaitForSeconds(2.0f);
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
    GameObject ITower.gameObject
    {
        get { return towerGameObject; }
        set { towerGameObject = value; }
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
                buffed = Instantiate(buff_prefab, transform.position, Quaternion.identity);
            }
            hasBeenBuffed = true;
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
