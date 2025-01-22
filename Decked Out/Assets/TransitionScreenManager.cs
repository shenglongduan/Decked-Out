using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreenManager : MonoBehaviour
{
    public static TransitionScreenManager Instance { get; private set; }
    
    [SerializeField] float _transitionDuration;
    [SerializeField] float _fadeOutDuration;

    [SerializeField] GameObject _leftParent;
    [SerializeField] Transform _leftTarget;
    [SerializeField] GameObject _northParent;
    [SerializeField] Transform _northTarget;
    [SerializeField] GameObject _rightParent;
    [SerializeField] Transform _rightTarget;
    [SerializeField] GameObject _southParent;
    [SerializeField] Transform _southTarget;
    [SerializeField]
    Image[] _images;

    List<Image> _childImages;
    List<Color> _colorOfChildImages;

    Vector3 _startPostionNorth;
    Vector3 _startPostionLeft;
    Vector3 _startPostionRight;
    Vector3 _startPositionSouth;

    bool _loading;
    bool _fading;
    string _previousScene;
    string _targetScene;
    float _transitionTimer = 0f;
    float _fadeOutTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
    private void Start()
    {
        _startPostionLeft = _leftParent.transform.position;
        _startPostionNorth = _northParent.transform.position; 
        _startPositionSouth = _southParent.transform.position;
        _startPostionRight = _rightParent.transform.position;
        _childImages = new List<Image>();
        _colorOfChildImages = new List<Color>();
        AddImages();
    }
    public void StartTranistion(string loadingSceneName)
    {
        _previousScene = SceneManager.GetActiveScene().name;
        _targetScene = loadingSceneName;
        _loading = true;
    }
    private void Update()
    {
        if (_loading)
        {
            _transitionTimer += Time.deltaTime;
            float t = _transitionTimer / _transitionDuration;
            t = Mathf.Clamp01(t);

            _leftParent.transform.position = Vector3.Lerp(_startPostionLeft, _leftTarget.position, t);
            _rightParent.transform.position = Vector3.Lerp(_startPostionRight, _rightTarget.position, t);
            _northParent.transform.position = Vector3.Lerp(_startPostionNorth, _northTarget.position, t);
            _southParent.transform.position = Vector3.Lerp(_startPositionSouth, _southTarget.position, t);

            if (t >= 1f)
            {
                _loading = false;
                _transitionTimer = 0f;
                StartCoroutine(LoadSceneAsync(_targetScene));
            }
        }
        if (_fading)
        {
            Debug.Log("Starting Fade Out");
            _fadeOutTimer += Time.deltaTime;
            float t = _fadeOutTimer / _fadeOutDuration;
            t = Mathf.Clamp01(t);
            for (int i = 0; i < _childImages.Count; i++)
            {
                float newAlpha = Mathf.Lerp(_colorOfChildImages[i].a, 0f, t);
                Color newColor = new Color(_colorOfChildImages[i].r, _colorOfChildImages[i].g, _colorOfChildImages[i].b, newAlpha);
                _childImages[i].color = newColor;
            }

            if (t >= 1f)
            {
                _fading = false;
                _fadeOutTimer = 0f;
                _leftParent.transform.position = _startPostionLeft;
                _northParent.transform.position = _startPostionNorth;
                _rightParent.transform.position = _startPostionRight;
                _southParent.transform.position = _startPositionSouth;
                for (int i = 0; i < _childImages.Count; i++)
                {
                    _childImages[i].color = new Color(_colorOfChildImages[i].r, _colorOfChildImages[i].g, _colorOfChildImages[i].b, 1);
                }
            }
        }
       
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (SceneManager.GetActiveScene().name == _targetScene)
        {
            _fading = true;
        }
        else
        {
            Debug.LogError("Loaded Scene is not target scene?");
        }

        //_fadeOutTimer = 0f;
    }

    private void AddImages()
    {
        foreach (Image image in _images)
        {
            _childImages.Add(image);
        }
        for (int i = 0; i < _childImages.Count; i++)
        {
            _colorOfChildImages.Add(_childImages[i].color);
        }
    }
}
