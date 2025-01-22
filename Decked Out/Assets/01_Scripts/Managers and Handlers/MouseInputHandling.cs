using UnityEngine;


public class MouseInputHandling : MonoBehaviour
{
    [Header("Range Colours")]
    public Color notPlaceableColour;
    public Color onIslandColour;
    public Sprite placeableRange;
    public Sprite onPlatformRange;
    public Sprite notPlaceableRange;
    private TowerSelection towerSelection;
    private GameObject currentTowerInstance;
    public GameObject castleGameObject;
    public CardRandoEngine cardRandoEngine;

    private WaveManager Wave;
    Vector3 mousePosition;

    private float unitSquareSize = 10.0f;
    public bool collisionOccurred = false;
    private bool _initialized = false;
    public bool islandTowerSelection = false;
    public GameObject towerRig;
    private SpriteRenderer towerRigSprite;
    public SpriteRenderer rangeIndicator;

    private float _scallingFactor1 = 0.75f;
    private float _scallingFactor2 = 0.6f;
    private float _spellScallingFactor = 0.5f;

    public void Initialize()
    {
        Debug.Log("<color=cyan> INITIALIZING </color>");
        towerSelection = GetComponent<TowerSelection>();
        castleGameObject = GameObject.Find("Main Castle");
        Wave = ServiceLocator.Get<WaveManager>();
        cardRandoEngine = FindObjectOfType<CardRandoEngine>();
        towerRig.gameObject.SetActive(false);
        if (castleGameObject == null)
        {
            castleGameObject = GameObject.FindWithTag("Player");
        }

        _initialized = true;
    }

    private void Update()
    {
        if (_initialized == false) { return; }

        if (towerSelection.IsSelectingTower())
        {
            HandleTowerPlacement();
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
        }
    }

    private void HandleTowerPlacement()
    {
        towerRig.transform.position = mousePosition;
        towerRigSprite = towerRig.GetComponentInChildren<SpriteRenderer>();

        rangeIndicator.color = Color.white;
        rangeIndicator.sprite = placeableRange;
        towerRigSprite.color = Color.white;
        //Debug.Log($"Range Indicator Color: {rangeIndicator.color}, Sprite: {rangeIndicator.sprite.name}");
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 towerPosition = mousePos;
        mousePos.z = 0;
        float minDistance = 1.0f;
        Vector3 castlePosition = castleGameObject.transform.position;
        float distanceToCastle = Vector3.Distance(mousePos, castlePosition);

        Vector2 mouseRay = mousePos;
        RaycastHit2D hit = Physics2D.Raycast(mouseRay, Vector2.zero);

        if (distanceToCastle < minDistance)
        {
            towerRigSprite.color = notPlaceableColour;
            rangeIndicator.sprite = notPlaceableRange;
            return;
        }

        if (!towerSelection.IsSelectingSpell())
        {
            SetTowerRigSprite();
        }
        else
        {
            SetSpellRigSprite();
        }

        towerRig.gameObject.SetActive(true);

        if (Mathf.Abs(towerPosition.x) <= unitSquareSize / 1 && Mathf.Abs(towerPosition.y) <= unitSquareSize / 2.5)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 0.1f);
            bool towerCollision = false;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Tower") || collider.CompareTag("Buffer") || collider.CompareTag("Placed"))
                {
                    towerCollision = !towerSelection.IsSelectingSpell();
                    if (towerCollision) break;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                PlaceTowerOrSpell(towerCollision, mousePos, hit);
            }

            if (towerCollision)
            {
                towerRigSprite.color = notPlaceableColour;
                rangeIndicator.sprite = notPlaceableRange;
            }

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Platform"))
            {
                HandlePlatformPlacement(hit);
            }
            else
            {
                HandleNonPlatformPlacement(towerCollision);
            }

        }
        else
        {
            towerRigSprite.color = notPlaceableColour;
            rangeIndicator.sprite = notPlaceableRange;
        }
    }

    private void SetTowerRigSprite()
    {
        switch (towerSelection.towers)
        {
            case "Archer Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.ArcherTower.GetComponentInChildren<SpriteRenderer>().sprite;
                float towerRange = towerSelection.ArcherTower.GetComponent<ArcherTower>().attackRange;
                Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Cannon Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.CannonTower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.CannonTower.GetComponent<CannonTower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Fire Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor2, _scallingFactor2);
                towerRigSprite.sprite = towerSelection.FlameTower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.FlameTower.GetComponent<FlamethrowerTower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Frost Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor2, _scallingFactor2);
                towerRigSprite.sprite = towerSelection.FrostTower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.FrostTower.GetComponent<FrostTower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Wave Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor2, _scallingFactor2);
                towerRigSprite.sprite = towerSelection.Wave_Tower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.Wave_Tower.GetComponent<Wave_Tower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Buff Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor2, _scallingFactor2);
                towerRigSprite.sprite = towerSelection.BuffTower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.BuffTower.GetComponent<BuffTower>().buffRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Electric Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor2, _scallingFactor2);
                towerRigSprite.sprite = towerSelection.ElectricTower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.ElectricTower.GetComponent<ElectricTower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Earthquake Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor2, _scallingFactor2);
                towerRigSprite.sprite = towerSelection.EathQuack.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.EathQuack.GetComponent<EarthQuack>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Attraction Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor2, _scallingFactor2);
                towerRigSprite.sprite = towerSelection.attraction_Tower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.attraction_Tower.GetComponent<AttractionTower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Poison Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.Poison_Tower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.Poison_Tower.GetComponent<Poison_tower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Ballista Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.Ballista_Tower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.Ballista_Tower.GetComponent<Ballista_Tower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Mortar":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.Mortar.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.Mortar.GetComponent<Mortar_Tower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Mystery":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.Mystery.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.Mystery.GetComponent<Mystery>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Sniper Tower":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.Sniper_Tower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.Sniper_Tower.GetComponent<SniperTower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Organ Gun":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_scallingFactor1, _scallingFactor1);
                towerRigSprite.sprite = towerSelection.OrganGun_Tower.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.OrganGun_Tower.GetComponent<OrganGunTower>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
        }
    }

    private void SetSpellRigSprite()
    {
     
        switch (towerSelection.spells)
        {
            case "Lightning":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_spellScallingFactor, _spellScallingFactor);
                towerRigSprite.sprite = towerSelection.lightning.GetComponentInChildren<SpriteRenderer>().sprite;
                float towerRange = towerSelection.lightning.GetComponent<LightningStrike>().attackRange;
                Vector3 towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Fireball":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_spellScallingFactor, _spellScallingFactor);
                towerRigSprite.sprite = towerSelection.fireball.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.fireball.GetComponent<Fireball>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Nuke":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_spellScallingFactor, _spellScallingFactor);
                towerRigSprite.sprite = towerSelection.nuke.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.nuke.GetComponent<Nuke>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Big Bomb":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_spellScallingFactor, _spellScallingFactor);
                towerRigSprite.sprite = towerSelection.bigBomb.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.bigBomb.GetComponent<BigBomb>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Chill":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_spellScallingFactor, _spellScallingFactor);
                towerRigSprite.sprite = towerSelection.chill.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.chill.GetComponent<Chill>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Freeze":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_spellScallingFactor, _spellScallingFactor);
                towerRigSprite.sprite = towerSelection.freeze.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.freeze.GetComponent<Freeze>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
            case "Black Hole":
                towerRigSprite.gameObject.transform.localScale = new Vector2(_spellScallingFactor, _spellScallingFactor);
                towerRigSprite.sprite = towerSelection.BlackHole.GetComponentInChildren<SpriteRenderer>().sprite;
                towerRange = towerSelection.BlackHole.GetComponent<BlackHole>().attackRange;
                towerRangeScaling = new Vector3(towerRange, towerRange, towerRange);
                rangeIndicator.transform.localScale = towerRangeScaling * 0.4f;
                break;
        }
     
    }

    private void PlaceTowerOrSpell(bool towerCollision, Vector3 mousePos, RaycastHit2D hit)
    {
        towerRig.gameObject.SetActive(false);

        if (!towerCollision)
        {
            if (!towerSelection.IsSelectingSpell())
            {
                PlaceTower(mousePos);
            }
            else
            {
                PlaceSpell(mousePos);
            }

            if (currentTowerInstance.tag == "Tower")
            {
                currentTowerInstance.AddComponent<PositionUpdater>();
            }

            SpriteRenderer towerRenderer = currentTowerInstance.GetComponentInChildren<SpriteRenderer>();
            if (!islandTowerSelection)
            {
                if (!Wave.collisionOccurred)
                {
                    currentTowerInstance.tag = "Placed";
                }
            }

            islandTowerSelection = false;
            if (towerRenderer != null)
            {
                float orderInLayer = -(towerRig.transform.position.y * 100);
                towerRenderer.sortingOrder = (int)orderInLayer;

                if (hit.collider != null && hit.collider.gameObject.CompareTag("Platform") && !towerSelection.IsSelectingSpell())
                {
                    towerRenderer.sortingOrder = 2501;
                    setIsland(true); 
                }
            }

            towerSelection.SetSelectingTower(false);
            towerSelection.SetSelectingSpell(false);
            Wave.IncrementTowersPlaced();
            cardRandoEngine.MoveCardHandPanel(false);
        }
    }

    private void PlaceTower(Vector3 mousePos)
    {
        switch (towerSelection.towers)
        {
            case "Archer Tower":
                currentTowerInstance = Instantiate(towerSelection.ArcherTower, mousePos, Quaternion.identity);
                break;
            case "Cannon Tower":
                currentTowerInstance = Instantiate(towerSelection.CannonTower, mousePos, Quaternion.identity);
                break;
            case "Fire Tower":
                currentTowerInstance = Instantiate(towerSelection.FlameTower, mousePos, Quaternion.identity);
                break;
            case "Frost Tower":
                currentTowerInstance = Instantiate(towerSelection.FrostTower, mousePos, Quaternion.identity);
                break;
            case "Buff Tower":
                currentTowerInstance = Instantiate(towerSelection.BuffTower, mousePos, Quaternion.identity);
                break;
            case "Electric Tower":
                currentTowerInstance = Instantiate(towerSelection.ElectricTower, mousePos, Quaternion.identity);
                break;
            case "Earthquake Tower":
                currentTowerInstance = Instantiate(towerSelection.EathQuack, mousePos, Quaternion.identity);
                break;
            case "Attraction Tower":
                currentTowerInstance = Instantiate(towerSelection.attraction_Tower, mousePos, Quaternion.identity);
                break;
            case "Wave Tower":
                currentTowerInstance = Instantiate(towerSelection.Wave_Tower, mousePos, Quaternion.identity);
                break;
            case "Poison Tower":
                currentTowerInstance = Instantiate(towerSelection.Poison_Tower, mousePos, Quaternion.identity);
                break;
            case "Ballista Tower":
                currentTowerInstance = Instantiate(towerSelection.Ballista_Tower, mousePos, Quaternion.identity);
                break;
            case "Mortar":
                currentTowerInstance = Instantiate(towerSelection.Mortar, mousePos, Quaternion.identity);
                break;
            case "Sniper Tower":
                currentTowerInstance = Instantiate(towerSelection.Sniper_Tower, mousePos, Quaternion.identity);
                break;
            case "Organ Gun":
                currentTowerInstance = Instantiate(towerSelection.OrganGun_Tower, mousePos, Quaternion.identity);
                break;
            case "Mystery":
                GameObject[] towers = new GameObject[] {
                    towerSelection.ArcherTower,
                    towerSelection.CannonTower,
                    towerSelection.FlameTower,
                    towerSelection.FrostTower,
                    towerSelection.Wave_Tower,
                    towerSelection.BuffTower,
                    towerSelection.ElectricTower,
                    towerSelection.EathQuack,
                    towerSelection.attraction_Tower,
                    towerSelection.Poison_Tower,
                    towerSelection.Ballista_Tower,
                    towerSelection.Sniper_Tower,
                    towerSelection.OrganGun_Tower
                };

                int randomIndex = Random.Range(0, towers.Length);
                GameObject selectedTower = towers[randomIndex];
                currentTowerInstance = Instantiate(selectedTower, mousePos, Quaternion.identity);
                break;
        }
    }

    private void PlaceSpell(Vector3 mousePos)
    {
        switch (towerSelection.spells)
        {
            case "Lightning":
                currentTowerInstance = Instantiate(towerSelection.lightning, mousePos, Quaternion.identity);
                break;
            case "Fireball":
                currentTowerInstance = Instantiate(towerSelection.fireball, mousePos, Quaternion.identity);
                break;
            case "Nuke":
                currentTowerInstance = Instantiate(towerSelection.nuke, mousePos, Quaternion.identity);
                break;
            case "Big Bomb":
                currentTowerInstance = Instantiate(towerSelection.bigBomb, mousePos, Quaternion.identity);
                break;
            case "Chill":
                currentTowerInstance = Instantiate(towerSelection.chill, mousePos, Quaternion.identity);
                break;
            case "Freeze":
                currentTowerInstance = Instantiate(towerSelection.freeze, mousePos, Quaternion.identity);
                break;
            case "Black Hole":
                currentTowerInstance = Instantiate(towerSelection.BlackHole, mousePos, Quaternion.identity);
                break;
        }
         towerSelection.SetSelectingSpell(false);
        towerSelection.SetSelectingTower(false);
        towerSelection.towers = null;
        towerSelection.spells = null;
    }

    private void HandlePlatformPlacement(RaycastHit2D hit)
    {
        if (towerSelection.towers == "Frost Tower" || towerSelection.towers == "Buff Tower" ||
            towerSelection.towers == "Earthquake Tower" || towerSelection.spells == "Lightning" ||
            towerSelection.spells == "Black Hole" || towerSelection.spells == "Freeze" ||
            towerSelection.spells == "Fireball" || towerSelection.spells == "Nuke" ||
            towerSelection.spells == "Big Bomb" || towerSelection.spells == "Chill")
        {
            towerRigSprite.color = notPlaceableColour;
            rangeIndicator.sprite = notPlaceableRange;
        }
        else
        {
            towerRigSprite.color = onIslandColour;
            rangeIndicator.sprite = onPlatformRange;
        }
        islandTowerSelection = true;
    }

    private void HandleNonPlatformPlacement(bool towerCollision)
    {
        if (!towerCollision)
        {
            towerRigSprite.color = Color.white;
            rangeIndicator.sprite = placeableRange;
        }
        else
        {
            islandTowerSelection = false;
        }
    }
    public void ClearRig()
    {
        if (towerRig != null)
        {
            towerRig.gameObject.SetActive(false);
        }
    }

    public float SpellSlotCheck()
    {
        if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace0)
        {
            return 0;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace1)
        {
            return 1;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace2)
        {
            return 2;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace3)
        {
            return 3;
        }
        else if (cardRandoEngine._lastCardSlot == cardRandoEngine.cardSpace4)
        {
            return 4;
        }
        else
        {
            return float.NaN;
        }
    }

    public void setCollision()
    {
        collisionOccurred = true;
    }

    public void setIsland(bool island)
    {
        islandTowerSelection = island;
    }
}
