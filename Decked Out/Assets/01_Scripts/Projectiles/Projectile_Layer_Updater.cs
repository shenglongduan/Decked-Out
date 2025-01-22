using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Layer_Updater : MonoBehaviour
{
    [SerializeField] float _tickSpeed = 0.001f;

    SpriteRenderer _spriteRenderer;
    float _timer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        float orderInLayer = (gameObject.transform.position.y * 100);
        orderInLayer = -orderInLayer;
        _spriteRenderer.sortingOrder = (int)orderInLayer;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _tickSpeed)
        {
            _timer = 0;
            float orderInLayer = (gameObject.transform.position.y * 100);
            orderInLayer = -orderInLayer;
            _spriteRenderer.sortingOrder = (int)orderInLayer;
        }
    }
}
