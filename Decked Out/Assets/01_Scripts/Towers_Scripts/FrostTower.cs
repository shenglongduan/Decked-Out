using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTower : MonoBehaviour, ITower
{
    public float attackRange = 5.0f;
    private float Damage = 10.0f;
    private float RateOfFire = 1.0f;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private float initialDamage;
    public GameObject effect;
    private GameObject buffed;
    private float initialRateOfFire;
    private bool hasBeenBuffed = false;
    private AudioSource audioSource;
    private Animator animator;

    private float _iceTowerAnimLength = 1.0f;
    private bool canAttack = true;

    private void Start()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioManager.SetSFXClip(AudioManager.SFXSound.Tower_Frost_Freeze);
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("IceTower_Animation"))
            {
                _iceTowerAnimLength = clip.length;
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {
        if (canAttack)
        {
            FindAndFreezeTarget();
        }
    }

    private void FindAndFreezeTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                FreezeTarget(collider.gameObject);
                break;
            }
        }
    }

    private void FreezeTarget(GameObject target)
    {
        if (canAttack)
        {
            canAttack = false;
            ApplyFreeze(target);
            StartCoroutine(PlayAnimationAndCooldown());
        }
    }

    private void ApplyFreeze(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        KaboomEnemy kaboom = target.GetComponent<KaboomEnemy>();
        Mopey_Misters mopey = target.GetComponent<Mopey_Misters>();
        Apostate apostate = target.GetComponent<Apostate>();
        Cleric cleric = target.GetComponent<Cleric>();

        if (enemy != null)
        {
            enemy.ApplyFreeze(0.3f);
        }
        if (kaboom != null)
        {
            kaboom.ApplyFreeze(0.3f);
        }
        if (apostate != null)
        {
            apostate.ApplyFreeze(0.3f);
        }
        if (cleric != null)
        {
            cleric.ApplyFreeze(0.3f);
        }
        if (mopey != null)
        {
            mopey.ApplyFreeze(0.3f);
        }
        Aegis aegis = target.GetComponent<Aegis>();
        if (aegis != null)
        {
            aegis.TakeDamage(damage);
        }
        audioSource.Play();
    }

    private IEnumerator PlayAnimationAndCooldown()
    {
        animator.SetBool("IsShooting", true);
        yield return new WaitForSeconds(_iceTowerAnimLength);
        animator.SetBool("IsShooting", false);
        yield return new WaitForSeconds(RateOfFire);
        canAttack = true;
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

    public float health
    {
        get { return Health; }
        set { Health = value; }
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
            if (spriteRenderer != null && Health != 0)
            {
                buffed = Instantiate(effect, transform.position, Quaternion.identity);
            }
            hasBeenBuffed = true;
        }
    }

    public void ResetTowerEffects()
    {
        Damage = initialDamage;
        RateOfFire = initialRateOfFire;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
        if (buffed != null)
        {
            Destroy(buffed);
        }
        hasBeenBuffed = false;
    }

    private void OnDestroy()
    {
        if (buffed != null)
        {
            Destroy(buffed);
        }
    }
}
