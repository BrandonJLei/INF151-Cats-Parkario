using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour
{
    public CharacterController2D controller;
    private bool alreadyUpgraded = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(alreadyUpgraded == false)
        {
            controller.Upgrade();
        }
        alreadyUpgraded = true;
    }
}
