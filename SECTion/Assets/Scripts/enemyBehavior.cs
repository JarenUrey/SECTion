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
    [SerializeField] float animationTime;
    [SerializeField] Collider2D damageCol;
    [SerializeField] AIPath movementScript;
    [SerializeField] AIDestinationSetter destinationSetter;

    [Header("---- Enemy Stats ----")]
    [SerializeField] public int Hp;
    [SerializeField] public float speed;
    [SerializeField] int shootAngle;
    [SerializeField] float despawnTime;

    [Header("---- Attack stats ----")]
    [SerializeField] int hitRange;
    [SerializeField] int targetFaceSpeed;
    [SerializeField] float hitRate;
    [SerializeField] int attackRange;
    [SerializeField] int attackDamage;

    int HPOrig;
    public bool dead;
    Vector2 playerDr;
    public float detectionRange;
    bool isAttacking;
    bool playerInRange;

    void Start()
    {
        HPOrig = Hp;
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

    IEnumerator attack()
    {
        anim.SetBool("IsAttacking", true);
        isAttacking = true;
        //used as a temporary marker for attacking
        StartCoroutine(flashAttack());
        
        //checks to see if object can be damaged
        IDamage damage = player.GetComponent<IDamage>();
        if (damage != null)
        {
            damage.takeDamage(attackDamage);
        }

        yield return new WaitForSeconds(animationTime);
        anim.SetBool("IsAttacking", false);

        yield return new WaitForSeconds(hitRate);
        isAttacking = false;
    }

    public void takeDamage(int amount)
    {
        Hp -= amount;

        StartCoroutine(flashDamage());

        if (Hp <= 0)
        {
            //is dead
            movementScript.enabled = false;
            destinationSetter.enabled = false;
            dead = true;
            damageCol.enabled = false;
            StopAllCoroutines();

            StartCoroutine(die());
        }
    }

    //temp for tracking for when taking damage
    IEnumerator flashDamage()
    {
        sprite.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    //temp for tracking for when attacking
    IEnumerator flashAttack()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    IEnumerator die()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }

    //used as rudementary detection radius 
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
