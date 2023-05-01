using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] private NavMeshAgent _enemy;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _whatIsGround, _whatIsPlayer;
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField] private float _enemyHealth;
    [SerializeField] private float _walkPointRange;
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private float _sightRange, _attackRange;      
    [SerializeField] private bool _playerInSightRange, _playerInAttackRange;
    [SerializeField] private Transform _enemySpellSpawnPoint;
    #endregion

    bool alreadyAttacked;
    private bool _walkPointSet;
    private string _enemyTag;
    private float _enemySpellSpeed;
    private float _enemySpellDamage;

    private void Start()
    {
        _enemyTag = gameObject.tag;
        Debug.Log(_enemyTag);
    }

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _enemy = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        if (!_playerInSightRange && !_playerInAttackRange) Patroling();
        if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
        if (_playerInAttackRange && _playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet)
            _enemy.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _whatIsGround))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _enemy.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        _enemy.SetDestination(transform.position);

        transform.LookAt(_player);

        if (!alreadyAttacked)
        {
            EnemyAttack();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }
    private void EnemyAttack()
    {
        GameObject enemySpell = ObjectPoolingManager.Instance.GetPooledObject(_enemyTag);
        if (enemySpell != null)
        {           
            enemySpell.transform.position = _enemySpellSpawnPoint.position;
            enemySpell.transform.rotation = _enemySpellSpawnPoint.rotation;
            enemySpell.GetComponent<Spell>().direction = transform.forward;
            _enemySpellSpeed = enemySpell.GetComponent<Spell>().SpellSpeed;
            _enemySpellDamage = enemySpell.GetComponent<Spell>().SpellDamage;
            enemySpell.SetActive(true);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        _enemyHealth -= damage;

        if (_enemyHealth <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
