using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour, IDamage
{
    public GameObject player;
    public SpriteRenderer sprite;
    [SerializeField] public float speed;
    public float detectionRange;
    [SerializeField] int shootAngle;
    [SerializeField] float hitRate;
    [SerializeField] int attackRange;
    [SerializeField] int attackDamage;

    private float distance;
    private float angleToPlayer;
    [SerializeField] public int Hp;
    bool dead;
    bool isAttacking;
    bool playerInRange;

    void Update()
    {
        //follow player
        if (!dead)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;


            //face player when chasing
            direction.Normalize();
            angleToPlayer = Vector2.Angle(new Vector2(player.transform.position.x, player.transform.position.y), transform.forward);
        }

        //detection range
        if(distance < detectionRange){
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        if (isAttacking && playerInRange)
        {
            //Note to Jaren, if check is not working, fix later
            StartCoroutine(attack());
            Debug.Log("Attack");
        }
        
    }

    IEnumerator attack()
    {
        isAttacking = true;
        StartCoroutine(flashAttack());

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            IDamage damagable = hit.collider.GetComponent<IDamage>();
            if (hit.transform != transform && damagable != null)
            {
                damagable.takeDamage(attackDamage);
            }
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
            dead = true;
            Destroy(gameObject);
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
}
