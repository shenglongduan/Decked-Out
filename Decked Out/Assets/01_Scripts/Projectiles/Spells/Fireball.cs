using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    public float attackRange = 2f;    
    [SerializeField] private float damage;
    private AudioSource source;
    private Animator animator;

    private void Start()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        source = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponentInChildren<Animator>();
        audioManager.playSFXClip(AudioManager.SFXSound.Power_Fireball_Cast, source);
        Invoke("DealDamage", 0.5f);
        StartCoroutine(DestroyWhenAnimationComplete());
    }

    private void DealDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemyScript = collider.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                    enemyScript.setBurning();
                }
                KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();
                if (kaboom != null)
                {
                    kaboom.TakeDamage(damage);
                    kaboom.setBurning();
                }
                Apostate apostate = collider.GetComponent<Apostate>();
                if (apostate != null)
                {
                    apostate.TakeDamage(damage);
                    apostate.setBurning();

                }
                Necromancer necromancer = collider.GetComponent<Necromancer>();
                if (necromancer != null)
                {
                    necromancer.TakeDamage(damage);
                    necromancer.setBurning();
                }
                Mopey_Misters mopey_ = collider.GetComponent<Mopey_Misters>();
                if (mopey_ != null)
                {
                    mopey_.TakeDamage(damage);
                    mopey_.setBurning();
                }
                Cleric cleric = collider.GetComponent<Cleric>();
                if (cleric != null)
                {
                    cleric.TakeDamage(damage);
                    cleric.setBurning();
                }
                Aegis aegis = collider.GetComponent<Aegis>();
                if (aegis != null)
                {
                    aegis.TakeDamage(damage);
                    aegis.setBurning();
                }
            }
        }
        if(source.isPlaying == false)
        {
            Destroy(gameObject, 0.5f);
        }
    }
    private IEnumerator DestroyWhenAnimationComplete()
    {
        yield return null;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.normalizedTime < 1)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}