using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    float attackSpeed { get; set; }
    float damage { get; set; }
    float health { get; set; }
    float range { get; set; }
    GameObject gameObject { get; set; }
    void ApplyBuff(float damageBuff, float rateOfFireBuff);
    void ResetTowerEffects();
}