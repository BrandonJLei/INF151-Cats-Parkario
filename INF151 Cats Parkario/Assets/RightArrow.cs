﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArrow : MonoBehaviour
{
    public Vector3 startPosition;

    void Update()
    {
        if (transform.position.x <= 10)
        {
            transform.Translate(0.01f, 0f, 0f);
        }
        else
        {
            transform.position = startPosition;
        }

    }
}

