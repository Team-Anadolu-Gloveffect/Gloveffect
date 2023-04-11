using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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
        if (collision.collider != null)
        {
            GameObject explosion = ObjectPoolingManager.Instance.GetPooledObject(gameObject.tag +"Explosion");
            if (explosion != null)
            {
                explosion.transform.position = collision.contacts[0].point;
                //Vector3 normal = collision.contacts[0].normal;
                //Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
                //explosion.transform.rotation = rotation;
                explosion.SetActive(true);
            }           
        }
        
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

