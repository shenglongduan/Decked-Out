using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartJiggle : MonoBehaviour
{
    [SerializeField] GameObject _castleSprite;
    [SerializeField] float _jiggleDuration = 0.25f;
    [Range(0, 1)]
    [SerializeField] float[] _jiggleMagnitudes;
    [SerializeField] AudioClip _hitSound;


    Castle _castleScript;
    AudioSource _audioSource;
    Vector3 _originalPOS;
    private GameLoader _loader;

    float _jiggleMagnitude = 0.1f;
    float _currentHealth;
    float _lastHealth;
    float _maxHealth;

    float _jiggleTimer;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        _castleScript =  FindObjectOfType<Castle>();
        _maxHealth = _castleScript.maxHealth;
        _currentHealth = _castleScript.health;
        _lastHealth = _currentHealth;
        _audioSource = GetComponent<AudioSource>();
    }
    public void StartJiggle(float health)
    {
        _lastHealth = _currentHealth;      
        float difference = Mathf.Abs(_lastHealth - health);
        int multipleOfFive = Mathf.FloorToInt(difference / 5);

        switch (multipleOfFive)
        {
            case 0:
                _jiggleMagnitude = _jiggleMagnitudes[0];                
                break;
            case 1:
                _jiggleMagnitude = _jiggleMagnitudes[1];
                break;
            case 2:
                _jiggleMagnitude = _jiggleMagnitudes[2];
                break;
            case 3:
                _jiggleMagnitude = _jiggleMagnitudes[3];
                break;
            case 4:
                _jiggleMagnitude = _jiggleMagnitudes[4];
                break;
            case 5:
                _jiggleMagnitude = _jiggleMagnitudes[5];
                break;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            Vibrate(_jiggleMagnitude);
            Debug.Log("Vibrating");
        }
        else if (Application.isEditor)
        {
            Debug.Log("Would be Vibrating");
        }

        if (_jiggleTimer <= 0)
            
        {
            _jiggleTimer = _jiggleDuration; 
        }

        _currentHealth = health;
        _audioSource.volume = _jiggleMagnitude * 4;
        _audioSource.PlayOneShot(_hitSound);

    }
    private void Vibrate(float duration)
    {
        long miliseconds = (long) (duration * 1000);
        Debug.LogFormat(miliseconds.ToString());
        VibratorManager.Vibrate(miliseconds);
    }
    private void Update()
    {
        if (_jiggleTimer > 0)
        {
            _jiggleTimer -= Time.deltaTime;
            float jiggleFactorX = Mathf.Sin(Time.time * Mathf.PI * 2 + Random.Range(-.75f, .75f)) * _jiggleMagnitude;
            float jiggleFactorY = Mathf.Sin(Time.time * Mathf.PI * 2 + Random.Range(-.75f, .75f)) * _jiggleMagnitude;            
            _castleSprite.transform.localPosition = _originalPOS + new Vector3(jiggleFactorX, jiggleFactorY, 0);
        }
        else
        {
            _castleSprite.transform.localPosition = _originalPOS;
        }
    }

}
