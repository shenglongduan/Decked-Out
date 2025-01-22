using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Quaternion endRotation = Quaternion.Euler(0f, 180f, 0f);
    private Quaternion flippedRotation;

    private float speed = 9.0f;
    private float arcHeight = -0.7f;
    private float zRotationAngle = 0.0f; 

    private bool isMoving = false;
    private bool flipOnce = false; 
    private bool doFlip = false;


    void FixedUpdate()
    {
        if (isMoving)
        {
            
            if (transform.position.x < endPosition.x)
            {
                float xDistance = Math.Abs(endPosition.x - startPosition.x);
                float yDistance = Math.Abs(endPosition.y - startPosition.y);
                float nextX = Mathf.MoveTowards(transform.position.x, endPosition.x, speed * Time.deltaTime);
                float nextY = Mathf.MoveTowards(transform.position.y, endPosition.y, speed * Time.deltaTime * (yDistance / xDistance));
                float baseZ = Mathf.Lerp(transform.position.z, endPosition.z, (nextX - startPosition.x) / xDistance);
                float arc = arcHeight * (nextX - startPosition.x) * (nextX - endPosition.x) / (-0.25f * xDistance * xDistance);

                transform.position = new Vector3(nextX, nextY, baseZ + arc);

              

                float xDistCovered = Math.Abs(transform.position.x - startPosition.x);

               
                Quaternion endRot = doFlip ? endRotation : Quaternion.Euler(0f, 0f, (zRotationAngle * 180 / Mathf.PI));

                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0f, 0f, 0f), endRot, xDistCovered / xDistance);
            }
            else
            {
                transform.position = new Vector3(endPosition.x, endPosition.y, endPosition.z);
                isMoving = false;
            }
        }

        if (flipOnce)
        {
         
            transform.rotation = Quaternion.Lerp(transform.rotation, flippedRotation, Time.deltaTime * 10f);
        }
    }

    public void setStartPosition(Vector3 start)
    {
        transform.position = start;

        startPosition = new Vector3(start.x, start.y, start.z);
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }
    public void onClick()
    {
        GameObject[] deckObjects = GameObject.FindGameObjectsWithTag("Deck");
        foreach (GameObject deckObject in deckObjects)
        {
            Destroy(deckObject);
        }
    }

    public void setEndPosition(Vector3 end)
    {
        endPosition = end;
    }

    // angle is in radians here
    public void setZRotationAngle(float angle)
    {
        zRotationAngle = angle;

    
        endRotation = transform.rotation * Quaternion.Euler(0f, 0f, (angle * 180) / Mathf.PI) * Quaternion.Euler(0f, 180f, 0f);
    }

    public void toggleFlip(bool flip)
    {
        doFlip = flip;
    }

    public string Name { get; set; } = "card";

    public void placeCard()
    {
        isMoving = true;
    }

    public void doFlipOnce()
    {
        flipOnce = true;

        flippedRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
    }

    public bool isStatic()
    {
        return !isMoving;
    }
}
