using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverBullet : MonoBehaviour
{
    [Header("===== Components =====")]
    [SerializeField] Rigidbody2D rb;

    [Header("===== Stats =====")]
    [SerializeField] public int damage;
    [SerializeField] public float speed;
    [SerializeField] public float destroyTime;


    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null && !other.CompareTag("Player"))
        {
            damageable.takeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void SetDestroyTime(float time)
    {
        destroyTime = time;
    }
}
