using UnityEngine;

public class Mortar_Ball : MonoBehaviour
{
    public float cannonSpeed = 10.0f;
    private float damage;
    private Vector2 fixedTargetPosition;
    public GameObject effect;
    [SerializeField] private float aoeRadius = 5.0f;
    private enum MovementPhase { Ascending, MovingHorizontal, Descending }
    private MovementPhase currentPhase = MovementPhase.Ascending;
    private const float maxYPosition = 12.0f;

    public void SetDamage(float value)
    {
        damage = value;
    }

    public void SetTarget(Transform target)
    {
        // Capture the target's position at the moment of firing
        if (target != null)
        {
            fixedTargetPosition = new Vector2(target.position.x, target.position.y);
        }
    }

    private void Update()
    {
        switch (currentPhase)
        {
            case MovementPhase.Ascending:
                Ascend();
                break;
            case MovementPhase.MovingHorizontal:
                MoveHorizontal();
                break;
            case MovementPhase.Descending:
                Descend();
                break;
        }

        CheckForImpact();
    }

    private void Ascend()
    {
        if (transform.position.y < maxYPosition)
        {
            transform.position += Vector3.up * cannonSpeed * Time.deltaTime;
        }
        else
        {
            currentPhase = MovementPhase.MovingHorizontal;
        }
    }

    private void MoveHorizontal()
    {
        Vector2 targetPosition = new Vector2(fixedTargetPosition.x, maxYPosition);
        if (transform.position.x != fixedTargetPosition.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, cannonSpeed * Time.deltaTime);
        }
        else
        {
            currentPhase = MovementPhase.Descending;
        }
    }

    private void Descend()
    {
        if (transform.position.y > fixedTargetPosition.y)
        {
            transform.position += Vector3.down * cannonSpeed * Time.deltaTime;
        }
        else
        {
            // Adjust the final position to be exactly at the fixed target's position.
            transform.position = new Vector3(fixedTargetPosition.x, fixedTargetPosition.y, transform.position.z);
            TriggerImpactEffect();
        }
    }

    private void CheckForImpact()
    {
        if (Vector2.Distance(transform.position, fixedTargetPosition) < 0.1f)
        {
            TriggerImpactEffect();
        }
    }

    private void TriggerImpactEffect()
    {
        Vector3 explosionPosition = transform.position + Vector3.up * 2.0f;
        GameObject Explosion = Instantiate(effect, explosionPosition, Quaternion.identity);
        DealDamage(transform.position);
        Destroy(Explosion, 0.7f);
        Destroy(gameObject);
    }
    private void DealDamage(Vector2 explosionPoint)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosionPoint, aoeRadius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemyScript = hitCollider.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
            KaboomEnemy kaboom = hitCollider.GetComponent<KaboomEnemy>();
            if (kaboom != null)
            {
                kaboom.TakeDamage(damage);
            }
            Apostate apostate = hitCollider.GetComponent<Apostate>();
            if (apostate != null)
            {
                apostate.TakeDamage(damage);
            }
            Necromancer necromancer = hitCollider.GetComponent<Necromancer>();
            if (necromancer != null)
            {
                necromancer.TakeDamage(damage);
            }
            Cleric cleric = hitCollider.GetComponent<Cleric>();
            if (cleric != null)
            {
                cleric.TakeDamage(damage);
            }
            Aegis aegis = hitCollider.GetComponent<Aegis>();
            if (aegis != null)
            {
                aegis.TakeDamage(damage);

            }
        }
    }
}

