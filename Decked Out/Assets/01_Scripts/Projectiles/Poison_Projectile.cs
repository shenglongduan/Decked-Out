using UnityEngine;

public class Poison_Projectile : MonoBehaviour
{
    public float arrowSpeed = 10.0f;
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
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, arrowSpeed * Time.deltaTime);


        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

  
}

