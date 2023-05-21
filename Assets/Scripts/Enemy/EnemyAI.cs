using System;
using Enums;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float enemyHealth;
    [SerializeField] private float walkPointRange;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float sightRange, attackRange;      
    [SerializeField] private bool playerInSightRange, playerInAttackRange;
    [SerializeField] private Transform enemySpellSpawnPoint;
    [SerializeField] private GameObject orbPrefab;
    #endregion

    bool _alreadyAttacked;
    private bool isMoving;
    private bool isNotMoving;
    private bool _walkPointSet;
    private string _enemyTag;
    private float _enemySpellSpeed;
    private EnemySpawnController _enemySpawnController;
    [SerializeField] private Animator animator;

    public SpellTypes enemyType;

    private void Start()
    {
        _enemySpawnController = FindObjectOfType<EnemySpawnController>();
    }

    private void Awake()
    {
        _enemyTag = gameObject.tag;
        Debug.Log(_enemyTag);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        isMoving = (enemy.velocity.magnitude > 0 || (playerInSightRange && !playerInAttackRange));
        isNotMoving = (enemy.velocity.magnitude == 0);
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        MoveEnemyAnimation();
    }

    private void Patroling()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet)
            enemy.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        enemy.SetDestination(transform.position);

        transform.LookAt(player);

        if (!_alreadyAttacked)
        {
            EnemyAttack();
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void EnemyAttack()
    {
        GameObject enemySpell = ObjectPoolingManager.Instance.GetPooledObject("Flametron");
        if (enemySpell != null)
        {           
            enemySpell.transform.position = enemySpellSpawnPoint.position;
            enemySpell.transform.rotation = enemySpellSpawnPoint.rotation;
            enemySpell.GetComponent<Spell>().direction = transform.forward;
            _enemySpellSpeed = enemySpell.GetComponent<Spell>().SpellSpeed;
            enemySpell.SetActive(true);
        }
        AttackAnimation();
    }
    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            SpawnOrb(gameObject.transform.position);
            ReturnPool();
            _enemySpawnController.EnemyKilled();
        }
        EnemyTakeDamageAnimation();
    }
    private void ReturnPool()
    {
        enemyHealth = 3;
        ObjectPoolingManager.Instance.ReturnToPool(gameObject.tag,gameObject);
    }
    private void SpawnOrb(Vector3 spawnPosition)
    {
        Instantiate(orbPrefab, spawnPosition, Quaternion.identity);
    }
    
    void MoveEnemyAnimation()
    {
        if (isMoving)
        {
            animator.SetFloat("move", 1);
        }
        else if (isNotMoving)
        {
            animator.SetFloat("move", -1);
        }
    }
    
    void EnemyTakeDamageAnimation()
    {
        animator.SetFloat("damage", 1);
        Invoke(nameof(ResetDamageAnimation), 1f);
    }

    void ResetDamageAnimation()
    {
        animator.SetFloat("damage", -1);
    }

    void AttackAnimation()
    {
        animator.SetFloat("attack", 1);
        Invoke(nameof(ResetAttackAnimation), 1f);
    }

    void ResetAttackAnimation()
    {
        animator.SetFloat("attack", -1);
    }
}
