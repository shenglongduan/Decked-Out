using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();

    public GameObject enemyPrefab;
    public GameObject KaboomPrefab;
    public GameObject GolemPrefab;
    public GameObject Apostate_Prefab;
    public GameObject necromancer;
    public GameObject aegis;
    public GameObject cleric;
    public GameObject Mopey_prefab;
    public GameObject Zealots_prefab;
    public GameObject Mistake_Prefab;
    public float unitSquareSize = 10.0f;
    public float TowersLeft = 6;
    public bool kaboomEnemy = false;
    public Slider healthSliderPrefab;
    public TMP_Text towersLeftText;
    public bool collisionOccurred = false;

    private int enemiesSpawned = 0;
    private EnemyKillTracker _killTracker;
    private Coroutine spawningCoroutine;
    public CardRandoEngine cardRandoEngine;
    private Button startButton;
    public int towersPlaced = 0;
    public int currentWave = 0;
    public CardHandling deck_Building;
    private GameSpeedManager _gameSpeedManager;
    private NewWavePanelManager _newWavePanel;

    public static WaveManager Instance { get; private set; }

    private void Start()
    {

           Initialize();

    }

    public WaveManager Initialize()
    {
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        deck_Building = FindObjectOfType<CardHandling>();
        _killTracker = FindObjectOfType<EnemyKillTracker>();
        _gameSpeedManager = FindObjectOfType<GameSpeedManager>();
        _newWavePanel = FindObjectOfType<NewWavePanelManager>();
        return this;
    }

    public void StartWaves()
    {
        ToggleStartButton(false);
        _gameSpeedManager.ActivateControlPanel();
        spawningCoroutine = StartCoroutine(StartWave());
    }

    public void SetStartButton(Button button)
    {
        startButton = button;
        startButton.onClick.AddListener(StartWaves);
        ToggleStartButton(true);
    }

    private IEnumerator StartWave()
    {
        int numberOfEnemies = waves[currentWave].numberOfEnemies;
        _killTracker.NumbersOfEnemiesInWave(numberOfEnemies);
        enemiesSpawned = 0;

        for (int i = 0; i < _killTracker._enemiesInWave; i++)
        {
            if (enemiesSpawned >= numberOfEnemies)
            {
                Debug.LogError("Trying to spawn more enemies than allowed");
                break;
            }
            else
            {
                SpawnEnemyBasedOnPercentage(waves[currentWave].enemySpawnInfos);
                enemiesSpawned++;
                yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemies);
            }
        }
    }

    private void SpawnEnemyBasedOnPercentage(List<EnemySpawnInfo> enemySpawnInfos)
    {
        float rand = Random.value;
        float cumulative = 0f;

        foreach (var enemyInfo in enemySpawnInfos)
        {
            cumulative += enemyInfo.spawnPercentage;
            if (rand < cumulative)
            {
                SpawnEnemyByType(enemyInfo.enemyType);
                break;
            }
        }
    }

    private void SpawnEnemyByType(string enemyType)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy;
        switch (enemyType)
        {
            case "Acolyte":
                newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Enemy>().maxHealth);
                Debug.Log("Spawned Acolyte");
                break;
            case "Kaboom":
                newEnemy = Instantiate(KaboomPrefab, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<KaboomEnemy>().maxHealth);
                Debug.Log("Spawned Kaboom");
                break;
            case "Golem":
                newEnemy = Instantiate(GolemPrefab, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Enemy>().maxHealth);
                Debug.Log("Spawned Golem");
                break;
            case "Apostate":
                newEnemy = Instantiate(Apostate_Prefab, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Apostate>().maxHealth);
                Debug.Log("Spawned Apostate");
                break;
            case "Necromancer":
                newEnemy = Instantiate(necromancer, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Necromancer>().maxHealth);
                Debug.Log("Spawned Necromancer");
                break;
            case "Aegis":
                newEnemy = Instantiate(aegis, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Aegis>().maxHealth);
                Debug.Log("Spawned Aegis");
                break;
            case "Mopey_Misters":
                newEnemy = Instantiate(Mopey_prefab, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Mopey_Misters>().maxHealth);
                Debug.Log("Spawned Mopey");
                break;
            case "Zoom_Zealots":
                newEnemy = Instantiate(Zealots_prefab, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<ZoomZealots>().maxHealth);
                Debug.Log("Spawned Zoom Zealots");
                break;
            case "Cleric":
                newEnemy = Instantiate(cleric, spawnPosition, Quaternion.identity);
                //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Cleric>().maxHealth);
                Debug.Log("Spawned Cleric");
                break;
        }
    }
    public void IncrementTowersPlaced()
    {
        towersPlaced++;
        TowersLeft--;
    }

    private void SetupHealthSlider(GameObject enemy, float maxHealth)
    {
        Slider newHealthSlider = Instantiate(healthSliderPrefab);
        Vector3 sliderPosition = Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, 100.0f, 0));
        newHealthSlider.transform.position = sliderPosition;
        newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        newHealthSlider.maxValue = maxHealth;
        //enemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int randomSide = UnityEngine.Random.Range(0, 4);
        bool spawnInside = UnityEngine.Random.value < 0.3f;

        float insetFactor = spawnInside ? 0.1f : 0.0f;

        float maxInset = unitSquareSize * insetFactor;
        float minEdgeOffset = unitSquareSize / 1 - maxInset;

        float randomX = 0;
        float randomY = 0;

        switch (randomSide)
        {
            case 0:
                randomX = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                randomY = spawnInside ? UnityEngine.Random.Range(minEdgeOffset, unitSquareSize / 2) : unitSquareSize / 2;
                break;
            case 1:
                randomX = spawnInside ? UnityEngine.Random.Range(unitSquareSize / 2 - maxInset, unitSquareSize / 2) : unitSquareSize / 2;
                randomY = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                break;
            case 2:
                randomX = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                randomY = spawnInside ? UnityEngine.Random.Range(-unitSquareSize / 2, -minEdgeOffset) : -unitSquareSize / 2;
                break;
            case 3:
                randomX = spawnInside ? UnityEngine.Random.Range(-unitSquareSize / 2, -unitSquareSize / 2 + maxInset) : -unitSquareSize / 2;
                randomY = UnityEngine.Random.Range(-unitSquareSize / 2 + maxInset, unitSquareSize / 2 - maxInset);
                break;
        }

        return new Vector3(randomX, randomY, 0);
    }

    public void AddEnemyToCurrentWave(string enemyType, Vector3 spawnPosition)
    {
        GameObject newEnemySpawn = null;
        //Slider newHealthSlider = Instantiate(healthSliderPrefab);

   

        switch (enemyType)
        {
       
            case "Kaboom":
                IncrementEnemyCount();
                newEnemySpawn = Instantiate(KaboomPrefab, spawnPosition, Quaternion.identity);
                //newHealthSlider.maxValue = newEnemySpawn.GetComponent<KaboomEnemy>().maxHealth;
                break;
            case "Golem":
                IncrementEnemyCount();
                newEnemySpawn = Instantiate(GolemPrefab, spawnPosition, Quaternion.identity);
                //newHealthSlider.maxValue = newEnemySpawn.GetComponent<Enemy>().maxHealth;
                break;
            case "Apostate":
                IncrementEnemyCount();
                newEnemySpawn = Instantiate(Apostate_Prefab, spawnPosition, Quaternion.identity);
                //newHealthSlider.maxValue = newEnemySpawn.GetComponent<Apostate>().maxHealth;
                break;
            case "Aegis":
                IncrementEnemyCount();
                newEnemySpawn = Instantiate(aegis, spawnPosition, Quaternion.identity);
                //newHealthSlider.maxValue = newEnemySpawn.GetComponent<Aegis>().maxHealth;
                break;
            case "Cleric":
                IncrementEnemyCount();
                newEnemySpawn = Instantiate(cleric, spawnPosition, Quaternion.identity);
                //newHealthSlider.maxValue = newEnemySpawn.GetComponent<Cleric>().maxHealth;
                break;
        }

        if (newEnemySpawn != null)
        {
            //sliderPosition = Camera.main.WorldToScreenPoint(newEnemySpawn.transform.position + new Vector3(0, 100.0f, 0));
            //newHealthSlider.transform.position = sliderPosition;
            //newHealthSlider.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            //newEnemy.GetComponent<Enemy>().SetHealthSlider(newHealthSlider);

        }
    }

    public void Spawn_mistakes(Vector3 spawnPosition)
    {
        Vector3 spawnOffset = Random.insideUnitCircle * 0.5f;
        GameObject newEnemy = Instantiate(Mistake_Prefab, spawnPosition + spawnOffset, Quaternion.identity);
        //SetupHealthSlider(newEnemy, newEnemy.GetComponent<Enemy>().maxHealth);
    }

    public void IncrementEnemyCount()
    {
        waves[currentWave].numberOfEnemies++;
        _killTracker.NumbersOfEnemiesInWave(waves[currentWave].numberOfEnemies);
    }

    public void AllEnemiesInWaveDestroyed()
    {
        UpdateTowerHealth();
        DestroyTowers();
        ToggleStartButton(true);
        _gameSpeedManager.DeactiveControlPanel();
        towersPlaced = 0;
        TowersLeft = 5;
        currentWave++;
        _newWavePanel.NewWave(currentWave);
    }
    public void StopWave()
    {
        towersPlaced = 0;
        TowersLeft = 5;
        //currentWave = 0;
        ToggleStartButton(true);
        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }

        DestroyAllGameObjectsWithTag("Enemy");
        DestroyAllGameObjectsWithTag("Health");
        //DestroyAllGameObjectsWithTag("Tower");
        //DestroyAllGameObjectsWithTag("Placed");
        //DestroyAllGameObjectsWithTag("Empty");
        //DestroyAllGameObjectsWithTag("Buffer");
        //DestroyAllGameObjectsWithTag("buffed_icon");
    }

    private void DestroyAllGameObjectsWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    public void SetCollision(bool tf)
    {
        collisionOccurred = tf;
    }

    private void DestroyTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            ITower towerScript = tower.GetComponent<ITower>();
            if (towerScript != null && towerScript.health <= 0)
            {
                Destroy(tower);
            }
        }
        GameObject[] PlacedTowers = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject PlacedTower in PlacedTowers)
        {
            ITower towerScript = PlacedTower.GetComponent<ITower>();
            if (towerScript != null && towerScript.health <= 0)
            {
                Destroy(PlacedTower);
            }
        }
        GameObject[] Empties = GameObject.FindGameObjectsWithTag("Empty");
        foreach (GameObject empty in Empties)
        {
            ITower towerScript = empty.GetComponent<ITower>();
            if (towerScript != null && towerScript.health <= 0)
            {
                Destroy(empty);
                collisionOccurred = false;
            }
        }
        GameObject[] buffer = GameObject.FindGameObjectsWithTag("Buffer");
        foreach (GameObject buffers in buffer)
        {
            IBuffTower towerScript = buffers.GetComponent<IBuffTower>();
            if (towerScript != null && towerScript.health <= 0)
            {
                Destroy(buffers);
            }
        }
    }

    private void UpdateTowerHealth()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject tower in towers)
        {
            ITower towerScript = tower.GetComponent<ITower>();
            if (towerScript != null)
            {
                towerScript.health--;
            }
        }
        GameObject[] PlacedTowers = GameObject.FindGameObjectsWithTag("Placed");
        foreach (GameObject PlacedTower in PlacedTowers)
        {
            ITower towerScript = PlacedTower.GetComponent<ITower>();
            if (towerScript != null)
            {
                towerScript.health--;
            }
        }
        GameObject[] Empties = GameObject.FindGameObjectsWithTag("Empty");
        foreach (GameObject empty in Empties)
        {
            ITower towerScript = empty.GetComponent<ITower>();
            if (towerScript != null)
            {
                towerScript.health--;
            }
        }
        GameObject[] buffer = GameObject.FindGameObjectsWithTag("Buffer");
        foreach (GameObject buffers in buffer)
        {
            IBuffTower towerScript = buffers.GetComponent<IBuffTower>();
            if (towerScript != null)
            {
                towerScript.health--;
            }
        }
    }
    public int GetCurrentEnemyCount()
    {
        int totalEnemies = 0;
        foreach (Wave wave in waves)
        {
            totalEnemies += wave.numberOfEnemies;
        }
        return totalEnemies;
    }
    private void ToggleStartButton(bool isEnabled)
    {
        startButton.interactable = isEnabled;
        startButton.gameObject.SetActive(isEnabled);
    }
}
