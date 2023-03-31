using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed =10f;
    public float damage;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Invoke the Disable() method after 5 seconds
        Invoke(nameof(Disable), 5f);
    }

    private void Disable()
    {
        // Deactivate the spell and return it to the object pool
        gameObject.SetActive(false);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    // Check if the spell collides with an enemy
    //    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
    //    if (enemy != null)
    //    {
    //        // Deal damage to the enemy
    //        enemy.TakeDamage(damage);

    //        // Deactivate the spell and return it to the object pool
    //        gameObject.SetActive(false);
    //    }
    //}

    private void FixedUpdate()
    {
        // Move the spell forward with a constant speed
        rb.MovePosition(rb.position + transform.forward * (speed * Time.fixedDeltaTime));
    }
}

