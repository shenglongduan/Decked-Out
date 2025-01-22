using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    [SerializeField] Image _baseImage;
    [SerializeField] Image _enemyImage;

    Camera _mainCamera;
    RectTransform _rectTransform;
    Vector3 _enemyPosition;
    bool _assigned = false;

    public void Initialize(Camera camera, Vector3 cameraBounds)
    {
        _mainCamera = camera;
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_assigned)
        {
            CalcuateIndicatorPosition();
        }
    }

    void CalcuateIndicatorPosition()
    {
        if (_assigned)
        {
            Debug.Log("Calculating Indicator Position and Activating");
            if (_mainCamera == null)
            {
                Debug.LogError("Camera not assigned.");
                return;
            }
            if (_rectTransform == null)
            {
                Debug.LogError("Transform not assigned.");
            }

            if (_enemyImage.sprite == null)
            {
                Debug.LogError("Enemy sprite is not set.");
                return;
            }
            Vector3 enemyPosition = _mainCamera.WorldToViewportPoint(_enemyPosition);
            Debug.Log("Enemy World to Viewport Point :" + enemyPosition);

            if (enemyPosition.x < 0 || enemyPosition.x > 1 || enemyPosition.y < 0 || enemyPosition.y > 1)
            {
                gameObject.SetActive(true);

                enemyPosition.x = Mathf.Clamp(enemyPosition.x, 0.05f, 0.95f);
                enemyPosition.y = Mathf.Clamp(enemyPosition.y, 0.05f, 0.95f);

                Vector3 indicatorPosition = _mainCamera.ViewportToScreenPoint(enemyPosition);
                Debug.Log("Indicator Position Viewport to Screenpoint: " +  indicatorPosition);

                _rectTransform.position = indicatorPosition;
            }
            else
            {
                gameObject.SetActive(false);
                Debug.Log("Deactiving Indicator");
            }
        }
    }

    public void UpdateIndicator(Sprite enemySprite, float scalingFactor, Vector3 enemyPosition)
    {
        if (_enemyImage == null)
        {
            Debug.LogError("Enemy Image is not assigned.");
            return;
        }
        Debug.Log("Updating Indicator");
        _assigned = true;
        _enemyImage.sprite = enemySprite;
        _enemyPosition = enemyPosition;
        //_transform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);
        CalcuateIndicatorPosition();
    }
    public void ReturnAndClearIndicator()
    {
        _assigned = false;
        _enemyImage.sprite = null;
        _enemyPosition = Vector3.zero;
        _rectTransform.localScale = Vector3.one;
    }
}
