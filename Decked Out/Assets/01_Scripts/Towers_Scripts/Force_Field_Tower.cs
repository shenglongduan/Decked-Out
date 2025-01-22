
using UnityEngine;

using System.Collections;

public class Force_Field_Tower : MonoBehaviour, ITower
{
    public float attackRange;
    public GameObject force_field_Prefab;
    [SerializeField] private float Damage;
    [SerializeField] private float RateOfFire;
    [SerializeField] private float Health = 2;
    private GameObject towerGameObject;
    private SpriteRenderer spriteRenderer;
    private GameObject instantiatedForceFieldPrefab;
    private float initialDamage;
    private float initialRateOfFire;
    public GameObject effect;
    private GameObject buffed;
    private bool hasBeenBuffed = false;
    public AudioSource audioSource;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Update()
    {


    }
    private void Start()
    {
        float towerRange = attackRange;
        Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
        force_field_Prefab.transform.localScale = towerRangeScaling / 4;
        initialDamage = Damage;
        initialRateOfFire = RateOfFire;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Vector3 positionWithOffset = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        instantiatedForceFieldPrefab = Instantiate(force_field_Prefab, positionWithOffset, Quaternion.identity);
    }
    private void OnDestroy()
    {
        if (instantiatedForceFieldPrefab != null)
        {
            Destroy(instantiatedForceFieldPrefab);
        }
    }
    public IEnumerator StartFlickerEffect()
    {
        SpriteRenderer sr = instantiatedForceFieldPrefab.GetComponentInChildren<SpriteRenderer>();
        Collider2D col = instantiatedForceFieldPrefab.GetComponent<Collider2D>();

        // Check if components are not null
        if (sr == null || col == null) yield break;

        // Flicker effect
        for (int i = 0; i < 5; i++) 
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f); // Flicker speed
        }

        yield return new WaitForSeconds(2); // Wait for 2 seconds

        
        sr.enabled = true; 
        col.isTrigger = true; 

        // Lower the opacity
        Color tempColor = sr.color;
        tempColor.a = 0.5f; 
        sr.color = tempColor;
    }
    public void ResetFieldPrefabChanges()
    {
        if (instantiatedForceFieldPrefab == null) return;

        SpriteRenderer sr = instantiatedForceFieldPrefab.GetComponentInChildren<SpriteRenderer>();
        Collider2D col = instantiatedForceFieldPrefab.GetComponent<Collider2D>();


        if (sr != null && col != null)
        {

            Color tempColor = sr.color;
            tempColor.a = 1f; 
            sr.color = tempColor;

            col.isTrigger = false;
        }
    }
    public float damage
    {
        get { return Damage; }
        set { Damage = value; }
    }
    public float attackSpeed
    {
        get { return RateOfFire; }
        set { RateOfFire = value; }
    }
    public float range
    {
        get { return attackRange; }
        set { attackRange = value; }
    }
    GameObject ITower.gameObject
    {
        get { return towerGameObject; }
        set { towerGameObject = value; }
    }
    public float health
    {
        get { return Health; }
        set { Health = value; }
    }
    public void ApplyBuff(float damageBuff, float rateOfFireBuff)
    {
        if (!hasBeenBuffed && !gameObject.CompareTag("Empty"))
        {
            Damage += damageBuff;
            RateOfFire *= rateOfFireBuff;
            if (RateOfFire < 0.1f)
            {
                RateOfFire = 0.1f;
            }
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null && health != 0)
            {
                buffed = Instantiate(effect, transform.position, Quaternion.identity);
            }
            hasBeenBuffed = true;

        }
    }

    public void ResetTowerEffects()
    {
        Damage = initialDamage;
        RateOfFire = initialRateOfFire;
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color defaultColor = Color.white;
            spriteRenderer.color = defaultColor;
        }
        Destroy(buffed);
        hasBeenBuffed = false;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }


}
