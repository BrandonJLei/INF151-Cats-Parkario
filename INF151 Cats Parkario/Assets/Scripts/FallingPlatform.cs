﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();

        
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.name.Equals("FullPlayer"))
        {
            Invoke("DropPlatform", 0.5f);
            Destroy(gameObject, 2f);
        }
    }

    // Update is called once per frame
    void DropPlatform()
    {
        rb.isKinematic = false;
        
    }
}
