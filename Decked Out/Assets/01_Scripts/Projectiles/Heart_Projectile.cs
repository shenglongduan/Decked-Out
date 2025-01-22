using UnityEngine;

public class Heart_Projectile : MonoBehaviour
{
    public float HeartSpeed = 10.0f;
    private float damage;
    private Transform target;
    public GameObject Heart_Pop;


    //Attraction tower 

    private Transform attractionTower; 

    private void Update()
    {
        if (target == null || target.gameObject == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPosition = new Vector2(target.position.x, target.position.y);
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, HeartSpeed * Time.deltaTime);

        Vector2 direction = targetPosition - currentPosition;


        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            DealDamage(target.gameObject);
            GameObject Heart_Effect = Instantiate(Heart_Pop, transform.position, Quaternion.identity);
            DealDamage(target.gameObject);
            Destroy(Heart_Effect, 2f);
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
    public void SetTower(Transform tower)
    {
        attractionTower = tower;
    }
    private void DealDamage(GameObject enemy)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)

        {
            enemyScript.TakeDamage(damage);
            enemyScript.Attracted(attractionTower);
        }
        KaboomEnemy kaboom = enemy.GetComponent<KaboomEnemy>();
        if (kaboom != null)
        {
            kaboom.TakeDamage(damage);
            kaboom.Attracted(attractionTower);
        }
        Apostate apostate = enemy.GetComponent<Apostate>();
        if (apostate != null)
        {
            apostate.TakeDamage(damage);
            apostate.Attracted(attractionTower);
        }
        Mopey_Misters _mopey = enemy.GetComponent<Mopey_Misters>();
        if (_mopey != null)
        {
            _mopey.TakeDamage(damage);
            _mopey.Attracted(attractionTower);
        }
        Necromancer necromancer = enemy.GetComponent<Necromancer>();
        if (necromancer != null)
        {
            necromancer.TakeDamage(damage);
            necromancer.Attracted(attractionTower);
        }
        Cleric cleric = enemy.GetComponent<Cleric>();
        if (cleric != null)
        {
            cleric.TakeDamage(damage);
            cleric.Attracted(attractionTower);
        }
        Aegis aegis = enemy.GetComponent<Aegis>();
        if (aegis != null)
        {
            aegis.TakeDamage(damage);
            aegis.Attracted(attractionTower);
        }
    }
}

