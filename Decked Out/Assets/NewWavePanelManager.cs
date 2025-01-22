using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWavePanelManager : MonoBehaviour
{
    [Header("Images/Sprites")]
    [SerializeField] Image[] _digitImages;
    [SerializeField] Sprite[] _digitSprites;
    [SerializeField] Image[] _imagesToFade;

    [Header("Movement")]
    [SerializeField] float _floatToFadeWaitTime;
    [SerializeField] float _floatInDuration;
    [SerializeField] float _fadeOutDuration;
    [SerializeField] GameObject _leftObject;
    [SerializeField] Transform _leftTarget;
    [SerializeField] GameObject _rightObject;
    [SerializeField] Transform _rightTarget;

    List<Color> _colorOfImagesToFade = new List<Color>();
    Vector3 _leftStartPosition;
    Vector3 _rightStartPosition;
    float _floatInTimer;
    float _fadeOutTimer;
    bool _floatingIn;
    bool _fadingOut;
    int _waveNumber;
    bool _updated;

    private void Awake()
    {
        UpdateWaveDisplay(0);
        NewWave(_waveNumber);
        _leftStartPosition = _leftObject.transform.position;
        _rightStartPosition = _rightObject.transform.position;
        for (int i = 0; i < _imagesToFade.Length; i++)
        {
            _colorOfImagesToFade.Add(_imagesToFade[i].color);
        }
    }

    public void NewWave(int waveNumber)
    {
        _waveNumber = waveNumber + 1;
        _floatingIn = true;
    }

    private void Update()
    {
        if (_floatingIn)
        {
            if (!_updated)
            {
                UpdateWaveDisplay(_waveNumber);
                _updated = true;
            }
            _floatInTimer += Time.deltaTime;
            float t = _floatInTimer / _floatInDuration;
            t = Mathf.Clamp01(t);

            _leftObject.transform.position = Vector3.Lerp(_leftStartPosition, _leftTarget.position, t);
            _rightObject.transform.position = Vector3.Lerp(_rightStartPosition, _rightTarget.position, t);

            if (t >= 1f)
            {
                _floatingIn = false;
                _floatInTimer = 0f;
                StartCoroutine(FloatToFadeWait(_floatToFadeWaitTime));
            }
        }
        if (_fadingOut)
        {
            _fadeOutTimer += Time.deltaTime;
            float f = _fadeOutTimer / _fadeOutDuration;
            f = Mathf.Clamp01(f);
            for (int i = 0; i < _imagesToFade.Length; i++)
            {
                if (_imagesToFade[i].gameObject.activeInHierarchy)
                {
                    float newAlpha = Mathf.Lerp(_colorOfImagesToFade[i].a, 0f, f);
                    Color newColor = new Color(_colorOfImagesToFade[i].r, _colorOfImagesToFade[i].g, _colorOfImagesToFade[i].b, newAlpha);
                    _imagesToFade[i].color = newColor;
                }
            }
            if (f >= 1f)
            {
                _fadingOut = false;
                _fadeOutTimer = 0f;
                _leftObject.transform.position = _leftStartPosition;
                _rightObject.transform.position = _rightStartPosition;
                for (int i = 0; i < _imagesToFade.Length; i++)
                {
                    _imagesToFade[i].color = new Color(_imagesToFade[i].color.r, _imagesToFade[i].color.g, _imagesToFade[i].color.b, 1);
                }
                _updated = false;
            }
        }
    }


    private void UpdateWaveDisplay(int waveNumber)
    {
        string waveString = waveNumber.ToString();
        for (int i = 0; i < _digitImages.Length; i++)
        {
            if (i < waveString.Length)
            {
                int digit = int.Parse(waveString[waveString.Length - 1 - i].ToString());
                _digitImages[_digitImages.Length - 1 - i].sprite = _digitSprites[digit];
                _digitImages[_digitImages.Length - 1 - i].enabled = true; // Ensure the image is enabled
            }
            else
            {
                _digitImages[_digitImages.Length - 1 - i].enabled = false; // Hide the image if not used
            }
        }
    }

    IEnumerator FloatToFadeWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _fadingOut = true;
    }
}
