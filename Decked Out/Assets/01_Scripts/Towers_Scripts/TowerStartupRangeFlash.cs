using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStartupRangeFlash : MonoBehaviour
{
    [SerializeField] float _fadeDuration = 5f;
    [SerializeField] SpriteRenderer _rangeRenderer;

    ITower _towerScript;
    IBuffTower _buffTowerScript;
    bool _tower;
    bool _buffTower;

    private void Start()
    {
        if (TryGetComponent<ITower>(out _towerScript))
        {
            _tower = true;
            Debug.Log("Tower Script Found");
        }
        else if (TryGetComponent<IBuffTower>(out _buffTowerScript))
        {
            _buffTower = true;
            Debug.Log("Buff Tower Script Found");
        }
        else
        {
            Debug.LogError("No Tower Found - Check for Interface on Script");
        }
        StartCoroutine(FadeAlphaToZero());
    }
    
    IEnumerator FadeAlphaToZero()
    {
        float currentTime = 0f;
        Color endColor = new Color(_rangeRenderer.color.r, _rangeRenderer.color.g, _rangeRenderer.color.b, 0); // Target alpha is 0

        while (currentTime < _fadeDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / _fadeDuration);
            _rangeRenderer.color = new Color(_rangeRenderer.color.r, _rangeRenderer.color.g, _rangeRenderer.color.b, alpha);
            yield return null; // Wait until the next frame
        }

        // Ensure the alpha is set to 0 after the loop
        _rangeRenderer.color = endColor;
    }

}
