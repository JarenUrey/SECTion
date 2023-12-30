using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nPMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.deltaTime, verticalMove * Time.deltaTime);
    }
}
