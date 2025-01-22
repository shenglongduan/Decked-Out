using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialBookManager : MonoBehaviour
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button _nextPageButton;
    [SerializeField] Button _previousPageButton;
    [SerializeField] GameObject[] _pages;

    int _pageIndex = 0;

    public void Exit()
    {
        gameObject.SetActive(false);
    }
    public void NextPage()
    {
        Debug.Log("Going to Next Page");
        _pages[_pageIndex].SetActive(false);
        ChangePageIndex(true);
    }
    public void PreviousPage()
    {
        Debug.Log("Going to Previous Page");
        _pages[_pageIndex].SetActive(false);
        ChangePageIndex(false);
    }
    private void ChangePageIndex(bool increase)
    {
        if (increase == true)
        {
            _pageIndex++;
            Debug.Log(_pageIndex);
            if (_pageIndex > _pages.Length - 1)
            {
                _pageIndex = 0;
            }
        }
        else if (increase == false)
        {
            _pageIndex--;
            Debug.Log(_pageIndex);
            if (_pageIndex < 0)
            {
                _pageIndex = _pages.Length - 1;
            }
        }
        _pages[_pageIndex].SetActive(true);
    }
}
