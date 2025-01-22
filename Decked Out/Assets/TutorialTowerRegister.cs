using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTowerRegister : MonoBehaviour
{
    [Range(0f, 5f)]
    [SerializeField] int _expectedNumberOfTowers;

    Popup _popup;
    bool _waiting;

    private void Awake()
    {
        _popup = GetComponent<Popup>();
    }


}
