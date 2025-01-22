using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuffTower : MonoBehaviour, IBuffTower
{
    public float buffRange;
    public float damageBuff;
    public float rateOfFireBuff;
    private List<ITower> towersInRange = new List<ITower>();

    [SerializeField] private float Health;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, buffRange);
    }
    public float health
    {
        get { return Health; }
        set { Health = value; }

    }
    public float range
    {
        get { return buffRange; }
        set { buffRange = value; }
    }
    private void Start()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.playSFXClip(AudioManager.SFXSound.Tower_Buff_Powerup, gameObject.GetComponent<AudioSource>());
    }
    private void Update()
    {
        towersInRange = FindTowersInRange();

        foreach (ITower tower in towersInRange)
        {
            tower.ApplyBuff(damageBuff, rateOfFireBuff);
        }
    }
    private void OnDestroy()
    {
        foreach (ITower tower in towersInRange)
        {
            tower.ResetTowerEffects();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        ITower towerScript = other.GetComponent<ITower>();
        if (towerScript != null)
        {
            towersInRange.Remove(towerScript); 
            towerScript.ResetTowerEffects();
        }
    }
    private List<ITower> FindTowersInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buffRange);

        return colliders
            .Select(collider => collider.GetComponent<ITower>())
            .Where(tower => tower != null)
            .ToList();
    }
}