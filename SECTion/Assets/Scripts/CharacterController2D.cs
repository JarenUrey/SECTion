using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController2D : MonoBehaviour, IDamage
{
    [Header("===== Components =====")]
    private Rigidbody2D m_Rigidbody2D;
    [SerializeField] SpriteRenderer sprite;

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
    [SerializeField] int damage;
    [SerializeField] float shootDistance;
    [SerializeField] int maxAmmo;
    int currAmmo;

    bool isShooting;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        currAmmo = maxAmmo;
    }

    void Update()
    {
        PlayerRotation();
        Debug.DrawRay(transform.position, transform.up * shootDistance, Color.red);

        if (Input.GetButton("Fire1") && !isShooting)
        {
            StartCoroutine(Shoot());
            //StartCoroutine(HitScan());
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
        if (currAmmo > 0)
        {
            isShooting = true;
            currAmmo--;

            Ray ray = new Ray(transform.position, transform.forward);
            Vector3 targetPoint;

            targetPoint = ray.GetPoint(shootDistance);

            Vector3 shootDir = shootPos.transform.position - targetPoint;

            GameObject currBullet = Instantiate(bullet, shootPos.transform.position, transform.rotation);
            currBullet.transform.forward = shootDir.normalized;

            yield return new WaitForSeconds(shootRate);

            isShooting = false;
        }
    }    

    IEnumerator HitScan()
    {
        isShooting = true;
        RaycastHit hit;
        Ray direction = new Ray(transform.position, transform.up);
        Vector3 targetPoint;

        targetPoint = direction.GetPoint(shootDistance);

        Vector3 sDir = shootPos.transform.position - targetPoint;
        Debug.Log("Shoot");
        if (Physics.Raycast(transform.position, sDir, out hit, shootDistance))
        {
            Debug.Log(direction);
            IDamage damagable = hit.collider.GetComponent<IDamage>();
            if (damagable != null)
            {
                Debug.Log("Damagable");
            }
            if (hit.transform != transform && damagable != null)
            {
                Debug.Log("Hit");
                damagable.takeDamage(damage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amount)
    {
        Health -= amount;
        StartCoroutine(flashDamage());
    }

    IEnumerator flashDamage()
    {
        sprite.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }
}
