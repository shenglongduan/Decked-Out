using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatformHalo : MonoBehaviour
{
    [SerializeField] float _fadeDuration;
    [Range(0.1f, 1f)]
    [SerializeField] float _maxAlpha = 1f;
    [Range(0.1f, 1f)]
    [SerializeField] float _minAlpha = 0.75f;
    [SerializeField] float _dragScale;
    [SerializeField] float _scaleDuration;

    float _scaleUpTimer;
    float _scaleDownTimer;
    float _currentScale = 1f;
    float _alpha;
    SpriteRenderer _spriteRenderer;
    bool _isDragging;
    bool _wasDragging;
    float _startScale = 1f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _alpha = _spriteRenderer.color.a;
        _currentScale = 1f; // Set initial scale to 1
        _spriteRenderer.transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
    }

    public void IsDragging(bool isDragging)
    {
        _isDragging = isDragging;
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
        // Assuming you have a bool to track when the dragging state changes
        if (_wasDragging != _isDragging)
        {
            // Capture the current scale as the starting point for the lerp
            _startScale = _currentScale;
            // Reset the appropriate timer based on the new state
            if (_isDragging)
            {
                _scaleDownTimer = 0.0f; // Start counting up for scale up
            }
            else
            {
                _scaleUpTimer = 0.0f; // Start counting down for scale down
            }
            _wasDragging = _isDragging;
        }

        if (_isDragging)
        {
            if (_currentScale < _dragScale)
            {
                _scaleUpTimer += Time.deltaTime;
                // Calculate t based on the captured start scale
                float t = Mathf.Clamp01(_scaleUpTimer / _scaleDuration);
                _currentScale = Mathf.Lerp(_startScale, _dragScale, t);
            }
        }
        else
        {
            if (_currentScale > 1.0f)
            {
                _scaleDownTimer += Time.deltaTime;
                // Calculate t based on the captured start scale
                float t = Mathf.Clamp01(_scaleDownTimer / _scaleDuration);
                _currentScale = Mathf.Lerp(_startScale, 1, t);
            }
        }

        // Apply the scaling
        _spriteRenderer.transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
    }
}
