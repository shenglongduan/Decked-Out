using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialCoverup : MonoBehaviour
{
    [SerializeField] string nextSceneName = "SampleScene";
    [Header("Tutorial Cards")]
    [SerializeField] TowerCardSO[] _tutorialCommonCards;
    [SerializeField] TowerCardSO[] _tutorialUncommonCards;
    [SerializeField] TowerCardSO[] _tutorialRareCards;
    [SerializeField] TowerCardSO[] _tutorialEpicCards;
    [SerializeField] TowerCardSO[] _tutorialLegendaryCards;
    [Header("Coverup Fade")]
    [SerializeField] GameObject _loadingText;
    [SerializeField] float _fadeTime;
    [SerializeField] Image[] _coverupImages;


    bool _fading;
    private void Start()
    {
        TutorialPassthrough tutorialPassthrough = FindObjectOfType<TutorialPassthrough>();
        if (tutorialPassthrough != null)
        {
            _loadingText.SetActive(true);
            DeckbuildingManager manager = FindObjectOfType<DeckbuildingManager>();
            manager.LoadTutorialCards(_tutorialCommonCards, "Common");
            manager.LoadTutorialCards(_tutorialUncommonCards, "Uncommon");
            manager.LoadTutorialCards(_tutorialRareCards, "Rare");
            manager.LoadTutorialCards(_tutorialEpicCards, "Epic");
            manager.LoadTutorialCards(_tutorialLegendaryCards, "Legendary");
            var loadSceneTask = SceneManager.LoadSceneAsync(nextSceneName);
        }
        else
        {
            _loadingText.SetActive(false);
            _fading = true;
            foreach (Image image in _coverupImages)
            {
                image.raycastTarget = false;
            }
            StartCoroutine(FadeOutCoverup());
        }
    }

    IEnumerator FadeOutCoverup()
    {
        float elapsedTime = 0f;
        float fadeDuration = _fadeTime;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            foreach (Image image in _coverupImages)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }

            yield return null;
        }

        foreach (Image image in _coverupImages)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
    }
}
