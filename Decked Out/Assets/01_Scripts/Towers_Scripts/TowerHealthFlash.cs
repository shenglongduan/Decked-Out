using UnityEngine;

public class TowerHealthFlash : MonoBehaviour
{
    [SerializeField] Color _flashColour = Color.red;
    [SerializeField] float _healthThreshold = 1;

    float _timer;
    ITower _towerScript;
    IBuffTower _buffTowerScript;
    SpriteRenderer _spriteRenderer;
    bool _tower;
    bool _buffTower;

    private void Start()
    {
        if (TryGetComponent<ITower>(out _towerScript))
        {
            _tower = true;
            Debug.Log("Tower Script Found");
        }
        else if(TryGetComponent<IBuffTower>(out _buffTowerScript))
        {
            _buffTower = true;
            Debug.Log("Buff Tower Script Found");
        }
        else
        {
            Debug.LogError("No Tower Found - Check for Interface on Script");
        }
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (_tower || _buffTower)
        {
            if (_towerScript != null)
            {
                if (_towerScript.health <= _healthThreshold)
                {
                    if (GlobalFlashTimer.Instance.IsFlashing())
                    {
                        // Tower Flashed
                        _spriteRenderer.color = _flashColour;
                    }
                    else
                    {
                        // Reset to default color if not in flashing window
                        _spriteRenderer.color = Color.white;
                    }
                }
               
            }

            else if (_buffTowerScript != null && _buffTowerScript.health < _healthThreshold)
            {
                if (_buffTowerScript.health <= _healthThreshold)
                {
                    if (GlobalFlashTimer.Instance.IsFlashing())
                    {
                        // Tower Flashed
                        _spriteRenderer.color = _flashColour;
                    }
                    else
                    {
                        // Reset to default color if not in flashing window
                        _spriteRenderer.color = Color.white;
                    }
                }
                
            }

            else
            {
                _spriteRenderer.color = Color.white; // Ensure it is white if health is above threshold
            }
          
        }

        else
        {
            Debug.LogError("Tower Script Not Found");
        }
    }
}
