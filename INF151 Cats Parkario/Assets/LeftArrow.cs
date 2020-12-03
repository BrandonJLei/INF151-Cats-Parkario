using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArrow : MonoBehaviour
{

    void Update()
    {
        if (transform.position.x >= -10)
        {
            transform.Translate(-0.01f, 0f, 0f);
        }
        else 
        {
            transform.Translate(0f, 0f, 0f);
        } 
        
    }
}
