using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedTowerStatHandler : MonoBehaviour
{

    [SerializeField] TowerCardSO _towerSO;
    [SerializeField] float _holdDuration = 2f;

    CardRandoEngine _cardRandoEngine;
    Collider2D _coll;
    float _holdTimer;
    bool _holding;

    private void Awake()
    {
        _cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        _coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (_holding)
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= _holdDuration )
            {
                _cardRandoEngine.ButtonStats(_towerSO);
                StopHolding();
            }
        }
    }
    private void StartHolding()
    {
        _holding = true;
        Debug.Log(gameObject.name + " is being held.");
    }
    private void StopHolding()
    {
        _holding= false;
        _holdTimer = 0f;
        Debug.Log(gameObject.name + " is no longer being held.");
    }

    private void OnMouseEnter()
    {
        StartHolding();
    }
    private void OnMouseExit()
    {
        StopHolding();
    }
}
