using UnityEngine;

public class TowerSelection : MonoBehaviour
{

    public bool isSelectingTower = false;
    public bool supportTower;
    public string towers;
    public string spells;


    [Header("Towers")]
    public GameObject ArcherTower;
    public GameObject CannonTower;
    public GameObject FlameTower; 
    public GameObject FrostTower;
    public GameObject BuffTower;
    public GameObject ElectricTower;
    public GameObject EathQuack;
    public GameObject attraction_Tower;
    public GameObject Wave_Tower;
    public GameObject Poison_Tower;
    public GameObject Ballista_Tower;
    public GameObject Mortar;
    public GameObject Mystery;
    public GameObject Sniper_Tower;
    public GameObject OrganGun_Tower;

    [Header("Spells")]
    public GameObject lightning;
    public GameObject fireball;
    public GameObject nuke;
    public GameObject bigBomb;
    public GameObject chill;
    public GameObject freeze;
    public GameObject BlackHole;
    public bool isSelectingSpell = false;


    public void SelectTower()
    {
        isSelectingTower = true;
        isSelectingSpell = false; 
    }

    public void SelectSpells()
    {
        isSelectingTower = true;
        isSelectingSpell = true;
    }
    public bool IsSelectingTower()
    {
        return isSelectingTower;
    }
    public bool IsSelectingSpell()
    {
        return isSelectingSpell;
    }
    public void SetSelectingTower(bool value)
    {
        isSelectingTower = value;
    }
    public void SetSelectingSpell(bool value)
    {
        isSelectingSpell = value;
    }
}