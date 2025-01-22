using UnityEngine;

public class SmallBullet : MonoBehaviour
{
    public float SmallBulletSpeed;
    private float damage;
    private Vector2 moveDirection;
    private float attackRange;

    private void Update()
    {
        transform.Translate(moveDirection * SmallBulletSpeed * Time.deltaTime, Space.World);
        if (Vector2.Distance(transform.position, transform.parent.position) > attackRange)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    public void SetAttackRange(float range)
    {
        attackRange = range;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            DealDamage(other.gameObject);
            Debug.Log("Creating bullet with damage");
        }
    }

    private void DealDamage(GameObject enemy)
    {
        Debug.Log("Dealing damage to target: " + enemy.name);
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
        Destroy(gameObject);
    }
}
