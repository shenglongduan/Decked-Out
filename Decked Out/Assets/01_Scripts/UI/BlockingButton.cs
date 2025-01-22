using UnityEngine;
using UnityEngine.EventSystems;

public class BlockingButton : MonoBehaviour, IPointerEnterHandler
{
    private GameLoader _loader;
    private CardRandoEngine _randoEngine;
    private MouseInputHandling _mouseInput;
    private TowerSelection _towerSelection;

    float _lastSpellSlot;
    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }
    private void Initialize()
    {
        _randoEngine = FindObjectOfType<CardRandoEngine>();
        _mouseInput = FindObjectOfType<MouseInputHandling>();
        _towerSelection = FindObjectOfType<TowerSelection>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PutCardBack();
        _randoEngine.MoveCardHandPanel(false);
    }
    private void PutCardBack()
    {
        Debug.Log(_randoEngine._lastCardSlot);
        _mouseInput.ClearRig();
        if (_towerSelection.isSelectingSpell == true && _towerSelection.isSelectingSpell == true)
        {
            Debug.Log("Putting Spell Back");
            _randoEngine.SpellSlotCheck();
            _towerSelection.SetSelectingSpell(false);
            _towerSelection.SetSelectingTower(false);
            _towerSelection.towers = null;
            _towerSelection.spells = null;
        }
        else if (_towerSelection.isSelectingTower == true && _towerSelection.isSelectingSpell != true)
        {
            Debug.Log("Putting Tower Back");
            _randoEngine._lastCardSlot.SetActive(true);
            _towerSelection.SetSelectingTower(false);
            _towerSelection.towers = null;
            _towerSelection.spells = null;
        }
    }

}
