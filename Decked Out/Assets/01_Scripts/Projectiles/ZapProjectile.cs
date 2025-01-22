using UnityEngine;

public class ZapProjectile : MonoBehaviour
{
    public float Speed = 10.0f;
    private float damage;
    private Transform target;
    public GameObject effect;

    private void Update()
    {
        if (target == null || target.gameObject == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, Speed * Time.deltaTime);

        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            GameObject deathEffect = Instantiate(effect, transform.position, Quaternion.identity);
            DealDamage(target.gameObject);
            Destroy(deathEffect, 0.5f);
            Destroy(gameObject);
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

    private void DealDamage(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
            enemyScript.Zap();
            enemyScript.ResetZapFlag();
        }
        KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.TakeDamage(damage);
            kaboom.Zap();
            kaboom.ResetZapFlag();

        }
        Apostate apostate = enemy.GetComponent<Apostate>();
        if (apostate != null)
        {  
            apostate.TakeDamage(damage);
            apostate.Zap();
            apostate.ResetZapFlag();

        }
        Mopey_Misters mopey_ = enemy.GetComponent<Mopey_Misters>();
        if (mopey_ != null)
        {
            mopey_.TakeDamage(damage);
            mopey_.Zap();
            mopey_.ResetZapFlag();

        }
        Necromancer necromancer = enemy.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.TakeDamage(damage);
            necromancer.Zap();
            necromancer.ResetZapFlag();
        }
        Cleric cleric = enemy.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.TakeDamage(damage);
            cleric.Zap();
            cleric.ResetZapFlag();
        }
        Aegis aegis = enemy.GetComponent<Aegis>();
        if (aegis != null)
        {
            aegis.TakeDamage(damage);
            aegis.Zap();
            aegis.ResetZapFlag();
        }
    }
}