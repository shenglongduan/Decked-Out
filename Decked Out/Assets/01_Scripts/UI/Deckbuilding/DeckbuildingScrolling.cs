using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckbuildingScrolling : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] Scrollbar _miniScrollbar;


    [SerializeField] RectTransform _scrollingPanel;
    [SerializeField] float maxY;
    [SerializeField] float minY;

    private void Start()
    {
        _scrollbar.value = _miniScrollbar.value;

        _scrollbar.onValueChanged.AddListener(OnScrollbarValueChange);
        _miniScrollbar.onValueChanged.AddListener(OnMiniScrollbarValueChange);
    }

    private void Update()
    {
        float yPOs = Mathf.Lerp(maxY, minY, 1 - _scrollbar.value);
        _scrollingPanel.anchoredPosition = new Vector2(_scrollingPanel.anchoredPosition.x, yPOs);

    }

    private void OnScrollbarValueChange(float value)
    {
        _miniScrollbar.onValueChanged.RemoveListener(OnMiniScrollbarValueChange);
        _miniScrollbar.value = value;
        _miniScrollbar.onValueChanged.AddListener(OnMiniScrollbarValueChange);
    }
    private void OnMiniScrollbarValueChange(float value)
    {
        _scrollbar.onValueChanged.RemoveListener(OnScrollbarValueChange);
        _scrollbar.value = value;
        _scrollbar.onValueChanged.AddListener(OnScrollbarValueChange);
    }
}
