using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicatorLocalManager : MonoBehaviour
{
    [SerializeField] Sprite _headSprite;
    [SerializeField] float _indicatorScallingFactor;

    PooledIndicator _indicatorPooledObject;
    EnemyIndicator _indicatorScript;
    private void Awake()
    {
        _indicatorPooledObject = EnemyIndicatorPool.Instance.GetIndicator();
        UpdateIndicator();
    }
    private void OnDestroy()
    {
        if (_indicatorPooledObject != null && _indicatorScript != null)
        {
            _indicatorScript.ReturnAndClearIndicator();
            EnemyIndicatorPool.Instance.ReturnIndicator(_indicatorPooledObject);
            _indicatorScript = null;
        }
    }

    private void UpdateIndicator()
    {
        _indicatorScript = _indicatorPooledObject.script;
        if (_indicatorScript != null && _indicatorScallingFactor != 0)
        {
            _indicatorScript.UpdateIndicator(_headSprite, _indicatorScallingFactor, gameObject.transform.position);
            Debug.Log("Calling Indicator to update.");
        }
        else
        {
            Debug.Log("Update Indicator not called from local manager.");
        }
    }
}
