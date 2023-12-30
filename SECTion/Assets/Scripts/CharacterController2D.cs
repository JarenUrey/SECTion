using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = 0.05f;
    [SerializeField] private LayerMask m_WhatIsWall;
    [SerializeField] private Transform m_GroundCheck;

    const float k_GroundedRadius = 0.2f;

    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();


    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    public void Move(float hMove, float vMove)
    {
        //Might need to update to include the X velocity
        Vector3 targetvelocity = new Vector2(hMove * 10f, vMove * 10f);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetvelocity, ref m_Velocity, m_MovementSmoothing);

    }
}
