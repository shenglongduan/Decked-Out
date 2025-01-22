using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    public float attackRange = 2f;
    private AudioSource source;
    private Animator animator;

    private void Start()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        source = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponentInChildren<Animator>();
        audioManager.playSFXClip(AudioManager.SFXSound.Power_Freeze_Cast, source);
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
                    enemyScript.ApplyTotalFreeze();
                }
                KaboomEnemy kaboom = collider.GetComponent<KaboomEnemy>();
                if (kaboom != null)
                {
                    kaboom.ApplyTotalFreeze();
                }
                Apostate apostate = collider.GetComponent<Apostate>();
                if (apostate != null)
                {
                    apostate.ApplyTotalFreeze();

                }
                Necromancer necromancer = collider.GetComponent<Necromancer>();
                if (necromancer != null)
                {
                    necromancer.ApplyTotalFreeze();
                }
                Cleric cleric = collider.GetComponent<Cleric>();
                if (cleric != null)
                {
                    cleric.ApplyTotalFreeze();
                }
                Mopey_Misters mopey_ = collider.GetComponent<Mopey_Misters>();
                if (mopey_ != null)
                {
                    mopey_.ApplyTotalFreeze();
                }
                Aegis aegis = collider.GetComponent<Aegis>();
                if (aegis != null)
                {
                    aegis.ApplyTotalFreeze();
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
