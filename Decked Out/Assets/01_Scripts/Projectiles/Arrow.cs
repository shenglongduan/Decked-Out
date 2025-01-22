using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed = 10.0f;
    private float damage;
    private Transform target;

    private void Update()
    {
        if (target == null || target.gameObject == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, arrowSpeed * Time.deltaTime);

        Vector2 direction = targetPosition - currentPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            DealDamage(target.gameObject);
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
    }
}

