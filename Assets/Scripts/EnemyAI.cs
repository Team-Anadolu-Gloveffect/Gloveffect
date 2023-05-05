using Enums;
using UnityEngine;
using UnityEngine.AI;

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
    #endregion

    bool _alreadyAttacked;
    private bool _walkPointSet;
    private string _enemyTag;
    private float _enemySpellSpeed;

    public SpellTypes enemyType;

    private void Start()
    {
        _enemyTag = gameObject.tag;
        //Debug.Log(_enemyTag);
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
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
            Invoke(nameof(ReturnPool), 0f);
        }
    }
    private void ReturnPool()
    {
        string enemyTag = gameObject.tag;
        GameObject enemyToReturn = gameObject;
        ObjectPoolingManager.Instance.ReturnToPool(enemyTag,enemyToReturn);
    }
}
