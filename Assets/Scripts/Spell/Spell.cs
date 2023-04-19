using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spell : MonoBehaviour
{
    public float SpellSpeed = 10f;
    public float SpellDamage = 3f;

    //private Rigidbody rb;
    public Vector3 direction = Vector3.zero;

    private void Start()
    {
        direction = transform.forward;
    }
    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();        
    }

    private void OnEnable()
    {      
        Invoke(nameof(Disable), 5f);
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
        transform.position += direction * SpellSpeed * Time.fixedDeltaTime;
        //rb.MovePosition(rb.position + transform.forward * (SpellSpeed * Time.fixedDeltaTime));
    }
}

