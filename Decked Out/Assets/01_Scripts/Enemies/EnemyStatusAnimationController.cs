using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusAnimationController : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] GameObject _burnEffect;
    [SerializeField] GameObject _slowEffect;
    [SerializeField] GameObject _charmEffect;
    [SerializeField] GameObject _poisonEffect;

    [Header("Enemy Script")]
    [SerializeField] Enemy _enemyScript;
    [SerializeField] Apostate _apostateScript;
    [SerializeField] KaboomEnemy _kaboomScript;
    [SerializeField] Necromancer _necromancerScript;
    [SerializeField] Mopey_Misters Mopey_Script;
    [SerializeField] ZoomZealots Zoom_Zealtos;
    [SerializeField] Aegis _AegisScript;
    [SerializeField] Cleric cleric;

    [Header("Sprite Modifers")]
    [SerializeField] SpriteRenderer _mainSpriteRenderer;
    [SerializeField] Color _burningColour;
    [SerializeField] Color _chilledColour;
    [SerializeField] Color _poisonedColour;
    [SerializeField] Color _attrachedColour;

    bool _basic = false;
    bool _kaboom = false;
    bool _apostate = false;
    bool _mopey = false;
    bool _necromancer = false;
    bool _zoom = false;
    bool _Aegis = false;
    bool _cleric = false;
    float _yPos;
    float _upscaleDuration = 0.5f;
    float _downscaleDuration = 0.25f;

    SpriteRenderer _burnRenderer;
    SpriteRenderer _slowRenderer;
    SpriteRenderer _charmRenderer;
    SpriteRenderer _poisonRenderer;

    [Header("Scalling")]
    [SerializeField] float _burnScaleValue;
    [SerializeField] float _slowScaleValue;
    [SerializeField] float _charmScaleValue;
    [SerializeField] float _poisonScaleValue;

    Vector3 _burnScale;
    Vector3 _slowScale;
    Vector3 _charmScale;
    Vector3 _poisonScale;


    int frozenStateFrames;
    int frozenStateFrameThreshold = 10;
    int clearStatusFrames;
    int clearStatusFrameThreshold = 10;

    private void Start()
    {
        if (_enemyScript != null)
        {
            _basic = true;
            Debug.Log("Basic Enemy Found");
        }
        else if (_kaboomScript != null)
        {
            _kaboom = true;
            Debug.Log("Kaboom Enemy Found");
        }
        else if (_apostateScript != null)
        {
            _apostate = true;
            Debug.Log("Apostate Found");
        }
        else if (_necromancerScript != null)
        {
            _necromancer = true;
            Debug.Log("Nercomancer Found");
        }
        else if (_AegisScript != null)
        {
            _Aegis = true;
            Debug.Log("Nercomancer Found");
        }
        else if (cleric != null)
        {
            _cleric = true;
            Debug.Log("Nercomancer Found");
        }
        else if (Mopey_Script != null)
        {
            _mopey = true;
            Debug.Log("Nercomancer Found");
        }
        else if (Zoom_Zealtos != null)
        {
            _zoom = true;
            Debug.Log("Nercomancer Found");
        }
        else
        {
            Debug.LogError("No type of enemy script found.");
        }

       _burnRenderer = _burnEffect.GetComponent<SpriteRenderer>();
       ScaleDownAndDisable(_burnEffect, _burnScale);
       _slowRenderer = _slowEffect.GetComponent<SpriteRenderer>();
       ScaleDownAndDisable(_slowEffect, _slowScale);
       _charmRenderer = _charmEffect.GetComponent<SpriteRenderer>();
       ScaleDownAndDisable(_charmEffect, _charmScale);
       _poisonRenderer = _poisonEffect.GetComponent<SpriteRenderer>();
       ScaleDownAndDisable(_poisonEffect, _poisonScale);

        _burnScale = Vector3.one * _burnScaleValue;
        _charmScale = Vector3.one * _charmScaleValue;
        _poisonScale = Vector3.one * _poisonScaleValue;
        _slowScale = Vector3.one * _slowScaleValue;
    }
    private void Update()
    {
        if (_basic)
        {
            if (_enemyScript.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (_enemyScript.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);                    
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!_enemyScript.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (_enemyScript.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!_enemyScript.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }
                
            }
            if (_enemyScript.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!_enemyScript.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (_enemyScript.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!_enemyScript.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!_enemyScript.isBurning && !_enemyScript.isPoisoned && !_enemyScript.isFrozen && !_enemyScript.isAttracted)
            {
                EnemyColour(Color.white);
            }
           
        }
        else if (_kaboom)
        {
            if (_kaboomScript.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (_kaboomScript.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!_kaboomScript.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (_kaboomScript.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!_kaboomScript.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }

            }
            if (_kaboomScript.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!_kaboomScript.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (_kaboomScript.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!_kaboomScript.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!_kaboomScript.isBurning && !_kaboomScript.isPoisoned && !_kaboomScript.isFrozen && !_kaboomScript.isAttracted)
            {
                clearStatusFrames++;
                if (clearStatusFrames >= clearStatusFrameThreshold)
                {
                    EnemyColour(Color.white);
                    clearStatusFrames = 0;
                }
            }

        }
        else if (_apostate)
        {
            if (_apostateScript.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (_apostateScript.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!_apostateScript.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (_apostateScript.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!_apostateScript.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }

            }
            if (_apostateScript.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!_apostateScript.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (_apostateScript.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!_apostateScript.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!_apostateScript.isBurning && !_apostateScript.isPoisoned && !_apostateScript.isFrozen && !_apostateScript.isAttracted)
            {
                clearStatusFrames++;
                if (clearStatusFrames >= clearStatusFrameThreshold)
                {
                    EnemyColour(Color.white);
                    clearStatusFrames = 0;
                }
            }
        }
        else if (_mopey)
        {
            if (Mopey_Script.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (Mopey_Script.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!Mopey_Script.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (Mopey_Script.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!Mopey_Script.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }

            }
            if (Mopey_Script.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!Mopey_Script.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (Mopey_Script.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!Mopey_Script.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!Mopey_Script.isBurning && !Mopey_Script.isPoisoned && !Mopey_Script.isFrozen && !Mopey_Script.isAttracted)
            {
                clearStatusFrames++;
                if (clearStatusFrames >= clearStatusFrameThreshold)
                {
                    EnemyColour(Color.white);
                    clearStatusFrames = 0;
                }
            }
        }
        else if (_zoom)
        {
            if (Zoom_Zealtos.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (Zoom_Zealtos.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!Zoom_Zealtos.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (Zoom_Zealtos.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!Zoom_Zealtos.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }

            }
            if (Zoom_Zealtos.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!Zoom_Zealtos.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (Zoom_Zealtos.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!Zoom_Zealtos.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!Zoom_Zealtos.isBurning && !Zoom_Zealtos.isPoisoned && !Zoom_Zealtos.isFrozen && !Zoom_Zealtos.isAttracted)
            {
                clearStatusFrames++;
                if (clearStatusFrames >= clearStatusFrameThreshold)
                {
                    EnemyColour(Color.white);
                    clearStatusFrames = 0;
                }
            }
        }
        else if (_necromancer)
        {
            if (_necromancerScript.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (_necromancerScript.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!_necromancerScript.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (_necromancerScript.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!_necromancerScript.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }
            }
            if (_necromancerScript.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!_necromancerScript.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (_necromancerScript.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!_necromancerScript.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!_necromancerScript.isBurning && !_necromancerScript.isPoisoned && !_necromancerScript.isFrozen && !_necromancerScript.isAttracted)
            {
                clearStatusFrames++;
                if (clearStatusFrames >= clearStatusFrameThreshold)
                {
                    EnemyColour(Color.white);
                    clearStatusFrames = 0;
                }
            }
        }
        else if (_Aegis)
        {
            if (_AegisScript.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (_AegisScript.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!_AegisScript.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (_AegisScript.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!_AegisScript.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }
            }
            if (_AegisScript.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!_AegisScript.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (_AegisScript.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!_AegisScript.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!_AegisScript.isBurning && !_AegisScript.isPoisoned && !_AegisScript.isFrozen && !_AegisScript.isAttracted)
            {
                clearStatusFrames++;
                if (clearStatusFrames >= clearStatusFrameThreshold)
                {
                    EnemyColour(Color.white);
                    clearStatusFrames = 0;
                }
            }
        }
        else if (cleric)
        {
            if (cleric.currentHealth <= 0)
            {
                _burnEffect.SetActive(false);
                _slowEffect.SetActive(false);
                _poisonEffect.SetActive(false);
                _charmEffect.SetActive(false);
            }
            if (cleric.isBurning)
            {
                EnemyColour(_burningColour);
                if (_burnEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_burnRenderer);
                }
                else if (_burnEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_burnEffect, _burnScale);
                    UpdateSortingOrder(_burnRenderer);
                }
            }
            else if (!cleric.isBurning && _burnEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_burnEffect, _burnScale);
            }
            if (cleric.isFrozen)
            {
                EnemyColour(_chilledColour);
                if (_slowEffect.activeInHierarchy == true)
                {
                    UpdateSortingOrder(_slowRenderer);
                }
                else if (_slowEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_slowEffect, _slowScale);
                    UpdateSortingOrder(_slowRenderer);
                }
            }
            else if (!cleric.isFrozen && _slowEffect.activeInHierarchy)
            {
                frozenStateFrames++;
                if (frozenStateFrames >= frozenStateFrameThreshold)
                {
                    ScaleDownAndDisable(_slowEffect, _slowScale);
                    frozenStateFrames = 0;
                }
            }
            if (cleric.isPoisoned)
            {
                EnemyColour(_poisonedColour);
                if (_poisonEffect.activeInHierarchy)
                {
                    UpdateSortingOrder(_poisonRenderer);
                }
                else if (_poisonEffect.activeInHierarchy != true)
                {
                    ScaleUpAndEnable(_poisonEffect, _poisonScale);
                    UpdateSortingOrder(_poisonRenderer);
                }
            }
            else if (!cleric.isPoisoned && _poisonEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_poisonEffect, _poisonScale);
            }
            if (cleric.isAttracted)
            {
                EnemyColour(_attrachedColour);
                ScaleUpAndEnable(_charmEffect, _charmScale);
                UpdateSortingOrder(_charmRenderer);
            }
            else if (!cleric.isAttracted && _charmEffect.activeInHierarchy)
            {
                ScaleDownAndDisable(_charmEffect, _charmScale);
            }
            else if (!cleric.isBurning && !cleric.isPoisoned && !cleric.isFrozen && !cleric.isAttracted)
            {
                clearStatusFrames++;
                if (clearStatusFrames >= clearStatusFrameThreshold)
                {
                    EnemyColour(Color.white);
                    clearStatusFrames = 0;
                }
            }
        }
    }

    private void EnemyColour(Color color)
    {
        _mainSpriteRenderer.color = color;
    }

    private void ScaleDownAndDisable(GameObject effectObject, Vector3 effectScale)
    {
        StartCoroutine(ScaleDownCoroutine(effectObject, effectScale));
    }
    private IEnumerator ScaleDownCoroutine(GameObject effectObject, Vector3 effectScale)
    {
        Vector3 originalScale = effectScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0;

        while (elapsedTime < _downscaleDuration)
        {
            float ratio = elapsedTime / _downscaleDuration;
            effectObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, ratio);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectObject.transform.localScale = targetScale;
        effectObject.SetActive(false);
    }

    private void ScaleUpAndEnable(GameObject effectObject, Vector3 scale)
    {
        effectObject.SetActive(true);
        StartCoroutine(ScaleUpCoroutine(effectObject, scale));
    }
    private IEnumerator ScaleUpCoroutine(GameObject effectObject, Vector3 targetScale)
    {
        Vector3 originalScale = effectObject.transform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < _upscaleDuration)
        {
            float ratio = elapsedTime / _upscaleDuration;
            effectObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, ratio);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        effectObject.transform.localScale = targetScale;
    }

    private void UpdateSortingOrder(SpriteRenderer effectRenderer)
    {
        _yPos = transform.position.y;
        _yPos = -_yPos;
        effectRenderer.sortingOrder = (int)(_yPos * 100) + 2;
    }

}
