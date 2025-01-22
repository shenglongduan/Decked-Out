using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandling : MonoBehaviour
{
    public GameObject cardPrefab;
    public Camera cam;

    private List<GameObject> deck = new List<GameObject>();
    private Vector3 deckLocation;
    private configOptions selectedAnimation;

    public int deckSize = 15;
    public int rowLength = 5;

    public GameObject blocker;

    public enum configOptions
    {
        Rows,
    };

    public configOptions configuration;

    private void createDeck(int numCards, Vector3 deckLocation)
    {
        this.deckLocation = deckLocation;



        for (int i = 0; i < numCards; i++)
        {
            // make sure the cards are stacked (so change the z-value for each card)
            float newZ = deckLocation.z - (i + 1) * 0.05f;
            Vector3 startPos = new Vector3(deckLocation.x, deckLocation.y, newZ);

            GameObject card = Instantiate(cardPrefab, startPos, Quaternion.Euler(0, 0, 0));

            card.AddComponent<BoxCollider>();
            card.AddComponent<CardScript>();
            card.GetComponent<CardScript>().setStartPosition(startPos);
            card.GetComponent<CardScript>().Name = "card" + i;

            this.deck.Add(card);
        }
        blocker.gameObject.SetActive(true);
    }

    private void setFlip(bool flip)
    {
        foreach (GameObject card in this.deck)
        {
            CardScript script = card.GetComponent<CardScript>();
            if (flip)
            {
                script.toggleFlip(true);
            }
            else
            {
                script.toggleFlip(false);
            }
        }
    }

    private void setCardEndPositions(configOptions configuration)
    {
        int rowLength = this.rowLength;
        int numRows = (int)Math.Ceiling(this.deck.Count / (float)rowLength);

        switch (configuration)
        {
            case configOptions.Rows:
                CardAnimations.makeRows(numRows, rowLength, this.deck);
                break;
        }
    }

    public IEnumerator placeCard()
    {

        for (int i = this.deck.Count - 1; i >= 0; i--)
        {
            this.deck[i].GetComponent<CardScript>().placeCard();

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void shootRay()
    {
        RaycastHit hit;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider != null)
            {
                CardScript cardScript = hit.transform.GetComponent<CardScript>();
                if (cardScript != null)
                {

                    if (cardScript.isStatic())
                    {

                        // cardScript.doFlipOnce();

                        cardScript.onClick();
                        blocker.gameObject.SetActive(false);
                    }
                }
            }
        }
    }


    void clearDeck()
    {
        foreach (GameObject card in this.deck)
        {
            Destroy(card);
        }
        deck = new List<GameObject>();
    }

   public void doCardAnimation()
    {
        clearDeck();
        createDeck(this.deckSize, new Vector3(-7, 4, -2));
        setCardEndPositions(selectedAnimation);
        setFlip(false);
        StartCoroutine("placeCard");
    }

    void Start()
    {


        selectedAnimation = configOptions.Rows;
        doCardAnimation();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            shootRay();
        }
    }
}
