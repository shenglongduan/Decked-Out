using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Projectile : MonoBehaviour
{
    public float waveSpeed = 10.0f;
    private float damage;
    private Transform target;
    private bool shouldRotate = true;
    private bool hasHit = false;
    private bool canMove = false;
    [SerializeField] private float force;
    [SerializeField] private float duration;
    private static Dictionary<GameObject, int> targetHits = new Dictionary<GameObject, int>();
    void Start()
    {
        StartCoroutine(EnableMovementAfterDelay(0.8f)); 

    }
    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true; 
    }

    private void Update()
    {
        if (!canMove) return;
        if (target == null || target.gameObject == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        Vector2 directionToTarget = (targetPosition - currentPosition).normalized;
        float stopDistance = 0.7f;  
        Vector2 offsetPosition = targetPosition - (directionToTarget * stopDistance);

        transform.position = Vector2.MoveTowards(currentPosition, offsetPosition, waveSpeed * Time.deltaTime);
        Vector2 direction = targetPosition - currentPosition;
        if (shouldRotate)
        {
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (Vector2.Distance(currentPosition, offsetPosition) < stopDistance)
        {
            if (!hasHit) 
            {
                DealDamage(target.gameObject);
                PushEnemy(target.gameObject, direction);
                hasHit = true;
            }

            shouldRotate = false;
            Destroy(gameObject, 1.5f);
        }
    
}
    private void TrackHitAndCheckForInstaKill(GameObject enemy)
    {
        if (targetHits.ContainsKey(enemy))
        {
            targetHits[enemy]++;
            if (targetHits[enemy] == 5) 
            {
               
                targetHits[enemy] = 0; 
                Insta_Kill(enemy);
            }
        }
        else
        {
            targetHits.Add(enemy, 1);
        }
    }
    public void SetDamage(float value)
    {
        damage = value;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void PushEnemy(GameObject enemy, Vector2 direction)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.HandleWaveImpact(direction, duration, force);
        }
        KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.HandleWaveImpact(direction ,duration, force);
        }
        Apostate apostate = enemy.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.HandleWaveImpact(direction, duration, force);
            
        }
        Mopey_Misters mopey_ = enemy.GetComponent<Mopey_Misters>();
        if (mopey_ != null)
        {
            mopey_.HandleWaveImpact(direction, duration, force);

        }
        Cleric cleric = enemy.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.HandleWaveImpact(direction, duration, force);

        }
        Aegis aegis = enemy.GetComponent<Aegis>();
        if (aegis != null)
        {
            aegis.HandleWaveImpact(direction, duration, force);

        }

    }
    private void DealDamage(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
        KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.TakeDamage(damage);
        }
        Apostate apostate = enemy.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.TakeDamage(damage);
        }
        Necromancer necromancer = enemy.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.TakeDamage(damage);
          
        }
        Mopey_Misters mopey = enemy.GetComponent<Mopey_Misters>();
        if (mopey != null)
        {
            mopey.TakeDamage(damage);

        }
        Cleric cleric = enemy.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.TakeDamage(damage);

        }
        Aegis aegis = enemy.GetComponent<Aegis>();
        if (aegis != null)
        {
            aegis.TakeDamage(damage);

        }
        TrackHitAndCheckForInstaKill(enemy);
    }
    private void Insta_Kill(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.Insta_Kill();
        }
        Mopey_Misters mopey_ = enemy.GetComponent<Mopey_Misters>();
        if (mopey_ != null)
        {
            mopey_.Insta_Kill();
        }
        KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.Insta_Kill();
        }
        Apostate apostate = enemy.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.Insta_Kill();
        }
        Necromancer necromancer = enemy.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.Insta_Kill();

        }
        Cleric cleric = enemy.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.Insta_Kill();

        }
        Aegis aegis = enemy.GetComponent<Aegis>();
        if (aegis != null)
        {
            aegis.Insta_Kill();

        }
    }
}

