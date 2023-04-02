using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float SpellSpeed = 10f;
    public float SpellDamage = 3f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke(nameof(Disable), 7f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {    
            enemy.TakeDamage(SpellDamage);
            Disable();
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * (SpellSpeed * Time.fixedDeltaTime));
    }
}

