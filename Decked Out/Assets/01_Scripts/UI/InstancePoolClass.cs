using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PooledIndicator
{
    public GameObject gameObject;
    public EnemyIndicator script;
    public Transform transForm;

    public PooledIndicator(GameObject gameObject)
    {
        this.script = gameObject.GetComponent<EnemyIndicator>();
        this.transForm = gameObject.GetComponent<Transform>();
        this.gameObject = gameObject;

        if (this.script == null)
        {
            Debug.LogError("EnemyIndicator script not found on the GameObject.");
        }
        if (this.transForm == null)
        {
            Debug.LogError("RectTransform component not found on the GameObject.");
        }
    }
}
