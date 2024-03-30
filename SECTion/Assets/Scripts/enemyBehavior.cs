using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBehavior : MonoBehaviour, IDamage
{
    [Header("---- Components ----")]
    [SerializeField] GameObject player;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] Collider2D damageCol;
    [SerializeField] AIPath movementScript;
    [SerializeField] AIDestinationSetter destinationSetter;

    [Header("---- Enemy Stats ----")]
    [SerializeField] public int Hp;
    [SerializeField] public float speed;
    [SerializeField] int shootAngle;

    [Header("---- Attack stats ----")]
    [SerializeField] int hitRange;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] float hitRate;
    [SerializeField] int attackRange;
    [SerializeField] int attackDamage;

    public bool dead;
    private float distance;
    private float angleToPlayer;
    Vector2 playerDr;
    public float detectionRange;
    bool isAttacking;
    bool playerInRange;

    void Start()
    {
        dead = false;
    }

    void Update()
    {
        //Checks to see if the enemy is alive
        if (dead != true)
        {
            if (playerInRange == true && !isAttacking)
            {
                StartCoroutine(attack());
            }
        }
        
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDr);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }

    IEnumerator attack()
    {
        isAttacking = true;
        StartCoroutine(flashAttack());

        
        IDamage damage = player.GetComponent<IDamage>();
        if (damage != null)
        {
            damage.takeDamage(attackDamage);
        }

        yield return new WaitForSeconds(hitRate);
        isAttacking = false;
    }

    public void takeDamage(int amount)
    {
        Hp -= amount;

        StartCoroutine(flashDamage());

        if (Hp <= 0)
        {
            movementScript.enabled = false;
            destinationSetter.enabled = false;
            dead = true;
            damageCol.enabled = false;
            StopAllCoroutines();
            //Destroy(gameObject);
        }
    }

    IEnumerator flashDamage()
    {
        sprite.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    IEnumerator flashAttack()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
