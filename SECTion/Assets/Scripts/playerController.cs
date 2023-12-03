using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rB;
    [SerializeField] Camera cam;
    [SerializeField] GameObject mousePos;
    [SerializeField] public Transform shootPos;
    
    [Header("Player Stats")]
    [SerializeField] int playerSpeed;
    [SerializeField] int playerHp;

    public Vector2 move;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
    }

    void playerMove()
    {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");
        controller.Move(move*Time.deltaTime*playerSpeed);
    }
}
