using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController2D : MonoBehaviour, IDamage
{
    [Header("===== Components =====")]
    private Rigidbody2D m_Rigidbody2D;

    [Header("===== Player Stats =====")]
    [SerializeField] public int Health;

    [Header("===== Movement =====")]
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = 0.05f;
    [SerializeField] private LayerMask m_WhatIsWall;
    [SerializeField] private Transform m_GroundCheck;
    private Vector3 m_Velocity = Vector3.zero;

    [Header("===== Gun Stats =====")]
    [SerializeField] public GameObject shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();


    }

    void Update()
    {
        PlayerRotation();

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    public void Move(float hMove, float vMove)
    {
        //Might need to update to include the X velocity
        Vector3 targetvelocity = new Vector2(hMove * 10f, vMove * 10f);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetvelocity, ref m_Velocity, m_MovementSmoothing);

    }

    private void PlayerRotation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        Debug.Log("shoot");
        Ray ray = new Ray(transform.position, transform.forward);
        Vector3 targetPoint;

        targetPoint = ray.GetPoint(50);

        Vector3 shootDir = targetPoint - shootPos.transform.position;

        GameObject currBullet = Instantiate(bullet, shootPos.transform.position, Quaternion.identity);
        currBullet.transform.forward = shootDir.normalized;

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }    

    public void takeDamage(int amount)
    {
        Health -= amount;
    }
}
