using UnityEngine;

public class Ballista_Arrow : MonoBehaviour
{
    public float arrowSpeed = 10.0f;
    private float damage;
    private Transform target;
    private Vector2 moveDirection;
    private void Start()
    {
      
        moveDirection = transform.right;
        Destroy(gameObject, 5f); 

        Vector2 direction = moveDirection; 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void Update()
    {
        transform.position += (Vector3)moveDirection * arrowSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            DealDamage(collision.gameObject);
        }
    }

    public void SetDamage(float value)
    {
        damage = value;
    }

    public void SetTarget(Transform newTarget)
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(newTarget.position.x, newTarget.position.y);
        moveDirection = (targetPosition - currentPosition).normalized;

        // Optionally, update the arrow's rotation to face the target at this point
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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

