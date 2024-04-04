using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController2D : MonoBehaviour, IDamage
{
    [Header("===== Components =====")]
    private Rigidbody2D m_Rigidbody2D;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator animator;
    [SerializeField] float animationTime;
    [SerializeField] AudioSource aud;
    [SerializeField] CharacterController2D controller;
    [SerializeField] public Rigidbody2D rb;

    [Header("===== Audio =====")]
    [SerializeField] AudioClip audRevolverShot;
    [Range(0, 1)][SerializeField] float audRevolerShotVol;
    [SerializeField] AudioClip audRevolverCock;
    [Range(0, 1)][SerializeField] float audRevolerCockVol;
    [SerializeField] AudioClip audRevolverEmpty;
    [Range(0, 1)][SerializeField] float audRevolverEmptyVol;
    [SerializeField] AudioClip audRevolverReload;
    [Range(0, 1)][SerializeField] float audRevolverReloadvol;

    [Header("===== Player Stats =====")]
    [SerializeField] public int Health;
    [SerializeField] public float speed;

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
    [SerializeField] float reloadSpeed;
    
    int currAmmo;
    int HPOrig;
    bool isShooting;
    bool isReloading;
    Vector2 move;

    void Start()
    {
        spawnPlayer();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        currAmmo = maxAmmo;
        UpdateRevolverAmmoCount();
    }

    void Update()
    {
        //Used for debugging where the player is looking
        Debug.DrawRay(transform.position, transform.up * shootDistance, Color.red);

        move.x = Input.GetAxisRaw("Horizontal") * speed;
        move.y = Input.GetAxisRaw("Vertical") * speed;

        //M1 is shoot
        if (Input.GetButton("Fire1") && !isShooting && !isReloading)
        {
            StartCoroutine(Shoot());
        }

        //R is reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currAmmo != maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    //used to run parallel to the main update function. To deal with player movement.
    private void FixedUpdate()
    {
        Move(move.x, move.y);
        PlayerRotation();
    }

    public void Move(float hMove, float vMove)
    {
        Vector2 targetvelocity = new Vector2(hMove, vMove);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetvelocity, ref m_Velocity, m_MovementSmoothing);

    }

    private void PlayerRotation()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 lookDir = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    IEnumerator Shoot()
    {
        //checks to see if player has ammo
        if (currAmmo > 0)
        {
            animator.SetBool("IsShooting", true);
            aud.PlayOneShot(audRevolverShot, audRevolerShotVol);
            isShooting = true;
            currAmmo--;

            //needs to have a target point for the bullet to fly towards
            Ray ray = new Ray(transform.position, transform.forward);
            Vector3 targetPoint;

            targetPoint = ray.GetPoint(shootDistance);

            Vector3 shootDir = shootPos.transform.position - targetPoint;

            GameObject currBullet = Instantiate(bullet, shootPos.transform.position, transform.rotation);
            currBullet.transform.forward = shootDir.normalized;
            UpdateRevolverAmmoCount();
            yield return new WaitForSeconds(animationTime);

            animator.SetBool("IsShooting", false);
            yield return new WaitForSeconds(shootRate - 0.5f);
            aud.PlayOneShot(audRevolverCock, audRevolerCockVol);
            yield return new WaitForSeconds(0.5f);
            isShooting = false;
        }
        else
        {
            isShooting = true;
            aud.PlayOneShot(audRevolverEmpty, audRevolverEmptyVol);
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }    

    IEnumerator Reload()
    {
        isReloading = true;
        aud.PlayOneShot(audRevolverReload, audRevolverReloadvol);
        yield return new WaitForSeconds(reloadSpeed);
        currAmmo = maxAmmo;
        UpdateRevolverAmmoCount();
        isReloading = false;
    }

    public void takeDamage(int amount)
    {
        Health -= amount;
        StartCoroutine(flashDamage());

        if (Health <= 0)
        {
            GameManager.instance.youLose();
        }
    }

    IEnumerator flashDamage()
    {
        sprite.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public void spawnPlayer()
    {
        controller.enabled = false;
        transform.position = GameManager.instance.PlayerSpawnPos.transform.position;
        controller.enabled = true;
    }


    void UpdateRevolverAmmoCount()
    {
        if (currAmmo == maxAmmo)
        {
            GameManager.instance.currRevolverAmmo.SetActive(false);
            GameManager.instance.currRevolverAmmo = GameManager.instance.revolverFiveShot;
            GameManager.instance.currRevolverAmmo.SetActive(true);
        }

        if (currAmmo == 4)
        {
            GameManager.instance.currRevolverAmmo.SetActive(false);
            GameManager.instance.currRevolverAmmo = GameManager.instance.revolverFourShot2;
            GameManager.instance.currRevolverAmmo.SetActive(true);
        }

        if (currAmmo == 3)
        {
            GameManager.instance.currRevolverAmmo.SetActive(false);
            GameManager.instance.currRevolverAmmo = GameManager.instance.revolverThreeShot;
            GameManager.instance.currRevolverAmmo.SetActive(true);
        }
        if (currAmmo == 2)
        {
            GameManager.instance.currRevolverAmmo.SetActive(false);
            GameManager.instance.currRevolverAmmo = GameManager.instance.revolverTwoShot;
            GameManager.instance.currRevolverAmmo.SetActive(true);
        }
        if (currAmmo == 1)
        {
            GameManager.instance.currRevolverAmmo.SetActive(false);
            GameManager.instance.currRevolverAmmo = GameManager.instance.revolverOneShot;
            GameManager.instance.currRevolverAmmo.SetActive(true);
        }
        if (currAmmo == 0)
        {
            GameManager.instance.currRevolverAmmo.SetActive(false);
            GameManager.instance.currRevolverAmmo = GameManager.instance.revolverZeroShot;
            GameManager.instance.currRevolverAmmo.SetActive(true);
        }
    }
}
