using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicatorPool : MonoBehaviour
{
    public static EnemyIndicatorPool Instance { get; private set; }

    [SerializeField] private GameObject _indicatorPrefab;
    [SerializeField] private int _poolSize = 50;

    [Header("Variables to Passthrough")]
    [SerializeField] Transform _canvasTransform;
    [SerializeField] Camera _camera;

    private Queue<PooledIndicator> _indicatorQueue = new Queue<PooledIndicator>();
    private Vector3 _cameraBounds;

    void Awake()
    {
        if (_canvasTransform == null)
        {
            Debug.LogError("Canvas Transform not assigned.");
            return;
        }
        else
        {
            Instance = this;
            _cameraBounds = new Vector3(Screen.width, Screen.height, 0);
            InitializePool();
        }
    }


    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject indicator = Instantiate(_indicatorPrefab);
            PooledIndicator pooledIndicator = new PooledIndicator(indicator);
            if (pooledIndicator.transForm != null)
            {
                pooledIndicator.transForm.SetParent(_canvasTransform, false);
            }
            if (pooledIndicator.script != null)
            {
                pooledIndicator.script.Initialize(_camera, _cameraBounds);
            }
            pooledIndicator.gameObject.SetActive(false);
            _indicatorQueue.Enqueue(pooledIndicator);
        }
    }

    public PooledIndicator GetIndicator()
    {
        if (_indicatorQueue.Count > 0)
        {
            PooledIndicator indicator = _indicatorQueue.Dequeue();
            indicator.gameObject.SetActive(true);
            return indicator;
        }
        else
        {
            //Spawning in new indicators if pool size not big enough
            //GameObject gameObject = Instantiate(_indicatorPrefab);
            //PooledIndicator indicator = new PooledIndicator(gameObject);
            //indicator.gameObject.SetActive(true);
            //return indicator;

            Debug.LogWarning("No inactive indicators in pool.");
            return null;
        }
    }

    public void ReturnIndicator(PooledIndicator indicator)
    {
        indicator.gameObject.SetActive(false);
        _indicatorQueue.Enqueue(indicator);
    }
}
