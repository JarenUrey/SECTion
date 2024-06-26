using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nPMovement : MonoBehaviour
{
    public CharacterController2D controller;
    [SerializeField] public Rigidbody2D rb;
    public float runSpeed = 40f;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    Vector2 move;
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
        move.x = horizontalMove;
        move.y = verticalMove;
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.deltaTime, verticalMove * Time.deltaTime);
    }
}
