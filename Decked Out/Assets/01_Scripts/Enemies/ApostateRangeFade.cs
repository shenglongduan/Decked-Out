using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApostateRangeFade : MonoBehaviour
{
    [SerializeField] float _fadeDuration;
    [Range(0.1f, 1f)]
    [SerializeField] float _maxAlpha = 1f;
    [Range(0.1f, 1f)]
    [SerializeField] float _minAlpha = 0.75f;

    float _alpha;
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _alpha = _spriteRenderer.color.a;
    }

    private void Update()
    {
        if (_spriteRenderer != null)
        {            
            float alphaSIN = Mathf.Sin((Time.time / _fadeDuration) * Mathf.PI * 2);
            float t = (alphaSIN + 1) / 2;
            _alpha = Mathf.Lerp(_maxAlpha, _minAlpha, t);
            Color newColor = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _alpha);
            _spriteRenderer.color = newColor;
        }
    }
}
