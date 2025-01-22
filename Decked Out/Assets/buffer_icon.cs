using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffer_icon : MonoBehaviour, IBuffTower
{
   private float buffRange;
   private float Health;

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
}

