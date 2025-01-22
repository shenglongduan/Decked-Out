using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthFlash : MonoBehaviour
{
    [SerializeField] float _flashDuration = 0.15f;
    [SerializeField] Color _flashColour = Color.red;

    float _timer;
    SpriteRenderer _spriteRenderer;
    bool _tookDmg;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void TakeDamage(float currentHealth)
    {
       _tookDmg = true;
        if (currentHealth <= 0)
        {
            _tookDmg = false;
            _spriteRenderer.color = Color.white;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_tookDmg)
        {
            _tookDmg = false;
            _spriteRenderer.color = _flashColour;
            _timer = _flashDuration;
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _spriteRenderer.color = Color.white;
                _timer = 0;
            }
        }
    }
}
