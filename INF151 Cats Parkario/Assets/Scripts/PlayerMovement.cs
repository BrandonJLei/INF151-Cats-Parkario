﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    private float actualSpeed = 40f;

    bool jump = false;
    bool changeStick = false;
    bool changeCircle = false;
    bool changeTriangle = false;
    bool changeSquare = false;
    float horizontalMove = 0f;
    
    void Start()
    {
        actualSpeed = runSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * actualSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeStick = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeCircle = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeTriangle = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeSquare = true;
        }

        if (transform.position.y < -30f)
        {
            controller.death();
        }

    }

    void FixedUpdate()
    {
        // Time.FixedDeltaTime: amount of time that has elpased since the last time this function was called
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
        if(changeStick)
        {
            actualSpeed = runSpeed;
            controller.ChangeToStick();
            changeStick = false;
        }
        if (changeCircle)
        {
            controller.ChangeToCircle();
            actualSpeed = runSpeed + 20;
            changeCircle = false;
        }
        if (changeTriangle)
        {
            actualSpeed = runSpeed;
            controller.ChangeToTriangle();
            changeTriangle = false;
        }
        if (changeSquare)
        {
            actualSpeed = runSpeed;
            controller.ChangeToSquare();
            changeSquare = false;
        }


        
    }
}
