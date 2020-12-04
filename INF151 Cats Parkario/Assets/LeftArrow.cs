using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArrow : MonoBehaviour
{
    private Vector3 startPosition;
    public float distance = 10f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (transform.position.x >= (distance*-1))
        {
            transform.Translate(-0.1f, 0f, 0f);
        }
        else 
        {
            transform.position = startPosition;
        } 
        
    }

}