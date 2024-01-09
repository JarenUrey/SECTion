using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour, IDamage
{
    public GameObject player;
    public float speed;
    public float detectionRange;

    private float distance;
    public int Hp;
    bool dead;

    void Update()
    {
        //follow player
        if (!dead)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;


            //face player when chasing
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        //detection range
        if(distance < detectionRange){
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
        
    }

    public void takeDamage(int amount)
    {
        Hp -= amount;

        if (Hp <= 0)
        {
            dead = true;
            Destroy(gameObject);
        }
    }
}
