using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Tower
{
    public Vector3 postion;
    public float health;
    public TowerCollection towerType;
}

public class TowerString
{
    public string postionX;
    public string postionY;
    public string postionZ;
    public string health;
    public string towerType;
}

public enum TowerCollection
{
    Archer_Tower, //Archer Tower
    Attraction_Tower, //Attraction Tower
    Cannon, //Cannon Tower
    Sniper_Tower, //Sniper Tower
    Frost_Tower, //Frost Tower
    Earthquake_Tower, //Earthquake Tower
    Wave_Tower, //Wave Tower
    Balista_Tower, //Ballista Tower
    Buff_Tower, //Buff Tower
    Poison_Tower, //Poison Tower
    Mystery_Tower, //Mystery Tower
    Organ_Tower, //Organ Gun Tower
    Flamethrower, //Fire Tower
    Mortar_Tower, //Mortar Tower
    Electric_Tower // Eletric Tower
}

public class MidGameSaveSystem : MonoBehaviour
{
    private string path = Application.streamingAssetsPath + "/MidGameSave.txt";

    private int TotalTowerCount;
    [HideInInspector] public float castleHealth = 0;
    [HideInInspector] public int waveCount = 0;
    [HideInInspector] public int killCount = 0;
    [HideInInspector] public int gemCount = 0;
    [HideInInspector] public TowerCardSO[] cardInPlay;
    [HideInInspector] public Tower[] towers;

    [SerializeField] private GameObject[] towersObj;
    private TowerString[] towersString;
    private int towerCollectionCount = 0;
    private string[] allOutPutString;


    private void Awake()
    {
        GameObject[] saveSystem = GameObject.FindGameObjectsWithTag("MidGameSaveSystem");

        if (saveSystem.Length > 1)
        {
            Destroy(gameObject);
        }

        foreach (TowerCollection card in System.Enum.GetValues(typeof(TowerCollection)))
        {
            towerCollectionCount++;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void WriteText()
    {
        StreamWriter writer = new StreamWriter(path, true);

        for(int i = 0; i < TotalTowerCount; i++)
        {
            writer.WriteLine(towers[i].postion.x.ToString());
            writer.WriteLine(towers[i].postion.y.ToString());
            writer.WriteLine(towers[i].postion.z.ToString());
            writer.WriteLine(towers[i].health.ToString());
            writer.WriteLine(towers[i].towerType.ToString());
            writer.WriteLine("");
        }

        writer.Close();
    }

    public void ReadText()
    {
        StringReader reader = new StringReader(path);

        towersString = new TowerString[PlayerPrefs.GetInt("MidGameTotalTowerCount")];

        for(int i = 0; i < towersString.Length; i++)
        {
            towersString[i].postionX = reader.ReadLine();
            towersString[i].postionY = reader.ReadLine();
            towersString[i].postionZ = reader.ReadLine();
            towersString[i].health = reader.ReadLine();
            towersString[i].towerType = reader.ReadLine();
            reader.ReadLine();
        }

        reader.Close();

        for (int i = 0; i < towersString.Length; i++)
        {
            towers[i].postion.x = float.Parse(towersString[i].postionX);
            towers[i].postion.y = float.Parse(towersString[i].postionY);
            towers[i].postion.z = float.Parse(towersString[i].postionZ);
            towers[i].health = int.Parse(towersString[i].health);
            towers[i].towerType = (TowerCollection)System.Enum.Parse(typeof(TowerCollection), towersString[i].towerType);
        }
    }

    public void SetMidGameCastleHealth(float count)
    {
        PlayerPrefs.SetFloat("MidGameCastleHealth", count);
    }
    public float GetMidGameCastleHealth()
    {
        if (PlayerPrefs.HasKey("MidGameCastleHealth"))
        {
            return PlayerPrefs.GetFloat("MidGameCastleHealth");
        }
        return 0;
    }

    public void SetMidGameWaveCount(int count)
    {
        PlayerPrefs.SetInt("MidGameWaveCount", count);
    }
    public int GetMidGameWaveCounth()
    {
        if (PlayerPrefs.HasKey("MidGameWaveCount"))
        {
            return PlayerPrefs.GetInt("MidGameWaveCount");
        }
        return 0;
    }

    public void SetMidGameKillCount(int count)
    {
        PlayerPrefs.SetInt("MidGameKillCount", count);
    }
    public int GetMidGameKillCount()
    {
        if (PlayerPrefs.HasKey("MidGameCastleHealth"))
        {
            return PlayerPrefs.GetInt("MidGameCastleHealth");
        }
        return 0;
    }

    public void SetMidGameGemCount(int count)
    {
        PlayerPrefs.SetInt("MidGameGemCount", count);
    }
    public int GetMidGameGemCount()
    {
        if (PlayerPrefs.HasKey("MidGameGemCount"))
        {
            return PlayerPrefs.GetInt("MidGameGemCount");
        }
        return 0;
    }

    public void SetTowers()
    {
        GameObject[] bufferTowerOBj = GameObject.FindGameObjectsWithTag("Buffer");
        GameObject[] inGameTowersObj = GameObject.FindGameObjectsWithTag("Tower");

        towers = new Tower[inGameTowersObj.Length + bufferTowerOBj.Length];
        TotalTowerCount = towers.Length;
        PlayerPrefs.SetFloat("MidGameTotalTowerCount", TotalTowerCount);
        int count = 0;

        foreach (GameObject tower in bufferTowerOBj)
        {
            towers[count].postion = tower.transform.position;

            BuffTower buffTower = tower.GetComponent<BuffTower>();
            EarthQuack earthQuack = tower.GetComponent<EarthQuack>();
            FrostTower frostTower = tower.GetComponent<FrostTower>();

            if (buffTower != null)
            {
                towers[count].health = buffTower.health;
                towers[count].towerType = TowerCollection.Buff_Tower;
            }
            else if (earthQuack != null)
            {
                towers[count].health = earthQuack.health;
                towers[count].towerType = TowerCollection.Earthquake_Tower;
            }
            else if (frostTower != null)
            {
                towers[count].health = frostTower.health;
                towers[count].towerType = TowerCollection.Frost_Tower;
            }
            count++;
        }

        foreach (GameObject tower in inGameTowersObj)
        {
            towers[count].postion = tower.transform.position;

            SniperTower sniperTower = tower.GetComponent<SniperTower>();
            ArcherTower archerTower = tower.GetComponent<ArcherTower>();
            FlamethrowerTower flamethrowerTower = tower.GetComponent<FlamethrowerTower>();
            ElectricTower electricTower = tower.GetComponent<ElectricTower>();
            AttractionTower attractionTower = tower.GetComponent<AttractionTower>();
            CannonTower cannonTower = tower.GetComponent<CannonTower>();
            Mortar_Tower mortar_Tower = tower.GetComponent<Mortar_Tower>();
            Wave_Tower wave_Tower = tower.GetComponent<Wave_Tower>();
            Poison_tower poison_Tower = tower.GetComponent<Poison_tower>();
            Ballista_Tower ballista_Tower = tower.GetComponent<Ballista_Tower>();
            Force_Field_Tower force_Field_Tower = tower.GetComponent<Force_Field_Tower>();
            OrganGunTower organGunTower = tower.GetComponent<OrganGunTower>();

            if (archerTower != null)
            {
                towers[count].health = archerTower.health;
                towers[count].towerType = TowerCollection.Archer_Tower;
            }
            else if (flamethrowerTower != null)
            {
                towers[count].health = flamethrowerTower.health;
                towers[count].towerType = TowerCollection.Flamethrower;
            }
            else if (electricTower != null)
            {
                towers[count].health = electricTower.health;
                towers[count].towerType = TowerCollection.Electric_Tower;
            }
            else if (attractionTower != null)
            {
                towers[count].health = attractionTower.health;
                towers[count].towerType = TowerCollection.Attraction_Tower;
            }
            else if (cannonTower != null)
            {
                towers[count].health = cannonTower.health;
                towers[count].towerType = TowerCollection.Cannon;
            }
            else if (wave_Tower != null)
            {
                towers[count].health = wave_Tower.health;
                towers[count].towerType = TowerCollection.Wave_Tower;
            }
            else if (ballista_Tower != null)
            {
                towers[count].health = ballista_Tower.health;
                towers[count].towerType = TowerCollection.Balista_Tower;
            }
            else if (poison_Tower != null)
            {
                towers[count].health = poison_Tower.health;
                towers[count].towerType = TowerCollection.Poison_Tower;
            }
            else if (mortar_Tower != null)
            {
                towers[count].health = mortar_Tower.health;
                towers[count].towerType = TowerCollection.Mortar_Tower;
            }
            else if (sniperTower != null)
            {
                towers[count].health = sniperTower.health;
                towers[count].towerType = TowerCollection.Sniper_Tower;
            }
            else if (force_Field_Tower != null)
            {
                towers[count].health = force_Field_Tower.health;
                towers[count].towerType = TowerCollection.Organ_Tower;
            }
            count++;
        }
    }
    public void SpawnTower()
    {
        foreach (Tower tower in towers)
        {
            if (tower.towerType == TowerCollection.Archer_Tower)
            {
                GameObject obj = Instantiate(towersObj[0]);
                obj.transform.position = tower.postion;
                obj.GetComponent<ArcherTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Frost_Tower)
            {
                GameObject obj = Instantiate(towersObj[1]);
                obj.transform.position = tower.postion;
                obj.GetComponent<FrostTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Buff_Tower)
            {
                GameObject obj = Instantiate(towersObj[2]);
                obj.transform.position = tower.postion;
                obj.GetComponent<BuffTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Flamethrower)
            {
                GameObject obj = Instantiate(towersObj[3]);
                obj.transform.position = tower.postion;
                obj.GetComponent<FlamethrowerTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Electric_Tower)
            {
                GameObject obj = Instantiate(towersObj[4]);
                obj.transform.position = tower.postion;
                obj.GetComponent<ElectricTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Earthquake_Tower)
            {
                GameObject obj = Instantiate(towersObj[5]);
                obj.transform.position = tower.postion;
                obj.GetComponent<EarthQuack>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Attraction_Tower)
            {
                GameObject obj = Instantiate(towersObj[6]);
                obj.transform.position = tower.postion;
                obj.GetComponent<AttractionTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Cannon)
            {
                GameObject obj = Instantiate(towersObj[7]);
                obj.transform.position = tower.postion;
                obj.GetComponent<CannonTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Wave_Tower)
            {
                GameObject obj = Instantiate(towersObj[8]);
                obj.transform.position = tower.postion;
                obj.GetComponent<Wave_Tower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Balista_Tower)
            {
                GameObject obj = Instantiate(towersObj[9]);
                obj.transform.position = tower.postion;
                obj.GetComponent<Ballista_Tower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Poison_Tower)
            {
                GameObject obj = Instantiate(towersObj[10]);
                obj.transform.position = tower.postion;
                obj.GetComponent<Poison_tower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Mortar_Tower)
            {
                GameObject obj = Instantiate(towersObj[11]);
                obj.transform.position = tower.postion;
                obj.GetComponent<Mortar_Tower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Sniper_Tower)
            {
                GameObject obj = Instantiate(towersObj[12]);
                obj.transform.position = tower.postion;
                obj.GetComponent<SniperTower>().health = tower.health;
            }
            else if (tower.towerType == TowerCollection.Organ_Tower)
            {
                GameObject obj = Instantiate(towersObj[14]);
                obj.transform.position = tower.postion;
                obj.GetComponent<OrganGunTower>().health = tower.health;
            }
        }
    }

    public void SetCardInPlay(TowerCardSO[] card)
    {
        cardInPlay = card;
    }

    public string[] GetTowerCollectionString()
    {
        allOutPutString = new string[towerCollectionCount];
        int count = 0;

        foreach (TowerCollection card in System.Enum.GetValues(typeof(TowerCollection)))
        {
            allOutPutString[count] = card.ToString();
        }

        return allOutPutString;
    }
}
