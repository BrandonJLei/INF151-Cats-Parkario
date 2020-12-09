using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArrow : MonoBehaviour
{
    Vector3 startPosition;
    public float distance;

    void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        if (transform.position.x >= startPosition.x + (distance * -1))
        {
            transform.Translate(-0.05f, 0f, 0f);
        }
        else 
        {
            transform.position = startPosition;
        } 
        
    }
}