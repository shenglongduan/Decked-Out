using System.Collections;
using UnityEngine;

public class Chill : MonoBehaviour
{
    public float attackRange = 2f;    
    [SerializeField] private float damage;
    private AudioSource source;
    private Animator animator;
    private void Start()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        source = gameObject.GetComponent<AudioSource>();
        audioManager.playSFXClip(AudioManager.SFXSound.Power_Freeze_Cast, source);
        animator = gameObject.GetComponentInChildren<Animator>();
        Invoke("DealDamage", 0.9f);
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
                    enemyScript.ApplyFreeze(0.3f);
                }
                KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();
                if (kaboom != null)
                {
                    kaboom.TakeDamage(damage);
                    kaboom.ApplyFreeze(0.3f);
                }
                Apostate apostate = collider.GetComponent<Apostate>();
                if (apostate != null)
                {
                    apostate.TakeDamage(damage);
                    apostate.ApplyFreeze(0.3f);

                }
                Necromancer necromancer = collider.GetComponent<Necromancer>();
                if (necromancer != null)
                {
                    necromancer.TakeDamage(damage);
                    necromancer.ApplyFreeze(0.3f);
                }
                Cleric cleric = collider.GetComponent<Cleric>();
                if (cleric != null)
                {
                    cleric.TakeDamage(damage);
                    cleric.ApplyFreeze(0.3f);
                }
                Aegis aegis = collider.GetComponent<Aegis>();
                if (aegis != null)
                {
                    aegis.TakeDamage(damage);
                    aegis.ApplyFreeze(0.3f);
                }
                Mopey_Misters mopey = collider.GetComponent<Mopey_Misters>();
                if (mopey != null)
                {
                    mopey.TakeDamage(damage);
                    mopey.ApplyFreeze(0.3f);
                }
            }
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