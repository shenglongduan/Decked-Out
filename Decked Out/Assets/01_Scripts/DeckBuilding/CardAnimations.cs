using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardAnimations
{
    public static void makeRows(int numRows, int rowLength, List<GameObject> deck)
    {
        for (int i = 0; i < numRows; i++)
        {
            int offsetFromDeck = -10;
            int newX = offsetFromDeck;
            int newY = 0;

            for (int j = i * rowLength; j < Math.Min((i * rowLength) + rowLength, deck.Count); j++)
            {
                newX += 5; 
                CardScript script =deck[deck.Count - 1 - j].GetComponent<CardScript>(); 
                script.setEndPosition(new Vector3(newX, newY, -2));
            }
        }
    }



   
}
