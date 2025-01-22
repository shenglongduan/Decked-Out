using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBobbingAnimation : MonoBehaviour
{     

    [SerializeField] float _squishFactor;
    [SerializeField] float _squishSpeed;
    [SerializeField] float _rotate;
    [SerializeField] float _rotateSpeed;

    private GameLoader _loader;
    float _squish;
    float _negSquish;
    float _negRotate;
    bool _isExpanding;
    bool _isRotating;
    float _s;
    float _r;
    Vector3 _orginalScale;
    Quaternion _orginalRotation;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
        _orginalScale = transform.localScale;
    }
    public void Initialize()
    {
        _negRotate = _rotate * -1;
        _squish = 1 + _squishFactor;
        _negSquish = 1 - _squishFactor;
    }
    private void Update()
    {
        if (_isExpanding)
        {
            _s += _squishSpeed * Time.deltaTime;
            if (_s >1f)
            {
                _s = 1;
                _isExpanding = false;
            }
        }       
        else if (!_isExpanding)
        {
            _s -= _squishSpeed * Time.deltaTime;
            if (_s < 0f)
            {
                _s = 0f;
                _isExpanding = true;
            }
        }
       
        float scaleFactor = Mathf.Lerp(_squish, _negSquish, _s);
        Vector3 currentScale = _orginalScale;
        currentScale.y *= scaleFactor;
        transform.localScale = currentScale;

        float angle = Mathf.Sin(Time.time * _rotateSpeed) * _rotate;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
