using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float meleeAttackDamage = 10f;
    [SerializeField] private float attackRange = 1f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.distance <= attackRange)
        {
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("There is an enemy");
                hit.collider.GetComponent<EnemyAI>().TakeDamage(meleeAttackDamage);
                animator.SetTrigger("MeleeAttack");
            }
        }
    }
}
