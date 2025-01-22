using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
   [SerializeField] public float range = 5f;
   [SerializeField] public float damage = 10f;
    private bool canAttack = true;
    private List<Enemy> enemyTargets = new List<Enemy>();
    private List<KaboomEnemy> kaboomTargets = new List<KaboomEnemy>();
    private List<Apostate> apostateTargets = new List<Apostate>();
    private List<Necromancer> nercoTargets = new List<Necromancer>();
    private void Start()
    {
        Destroy(gameObject, 5f); // Destroy after 5 seconds
    }

    private void Update()
    {
        FindAndShootTarget();
    }
    private void FindAndShootTarget()
    {
        if (canAttack)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    DamageOverTime(collider.gameObject);
                    break;
                }
            }
        }
    }
    private IEnumerator AttackCooldown()
    {
   
        while (!canAttack)
        {
            yield return new WaitForSeconds(0.5f);
            canAttack = true;
        }
    }

    private void DamageOverTime(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
            enemyScript.SetPoisoning(true);
            enemyTargets.Add(enemyScript);
        }
        KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.TakeDamage(damage);
            kaboom.SetPoisoning(true);
            kaboomTargets.Add(kaboom);
        }
        Apostate apostate = enemy.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.TakeDamage(damage);
            apostate.SetPoisoning(true);
            apostateTargets.Add(apostate);
        }
        Necromancer necromancer = enemy.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.TakeDamage(damage);
            necromancer.SetPoisoning(true);
            nercoTargets.Add(necromancer);
        }
        canAttack = false;
        StartCoroutine(AttackCooldown());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (enemyTargets.Count > 0)
            {
                foreach (Enemy enemy in enemyTargets)
                {
                    enemy.SetPoisoning(false);
                }
                enemyTargets.Clear();
            }
            if (kaboomTargets.Count > 0)
            {
                foreach (KaboomEnemy kaboom in kaboomTargets)
                {
                    kaboom.SetPoisoning(false);
                }
                kaboomTargets.Clear();
            }
            if (apostateTargets.Count > 0)
            {
                foreach (Apostate apostate in apostateTargets)
                {
                    apostate.SetPoisoning(false);
                }
                apostateTargets.Clear();
            }
            if (nercoTargets.Count > 0)
            {
                foreach (Necromancer necromancer in nercoTargets)
                {
                    necromancer.SetPoisoning(false);
                }
                nercoTargets.Clear();
            }
            StopAllCoroutines(); 
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}