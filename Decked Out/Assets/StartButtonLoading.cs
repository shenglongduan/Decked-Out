using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonLoading : MonoBehaviour
{
    GameObject _button;
    private void Start()
    {
        _button = this.gameObject;
    }
    public void DisableButton()
    {
        _button.SetActive(false);
    }
    
}
