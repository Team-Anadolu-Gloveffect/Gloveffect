using Enums;
using UnityEngine;
using UnityEngine.Pool;

public class Spell : MonoBehaviour
{
    public float SpellSpeed = 10f;
    public float SpellDamage = 3f;
    public SpellTypes spellType;

    public Vector3 direction = Vector3.zero;

    private void Start()
    {
        direction = transform.forward;
    }
    private void OnEnable()
    {      
        Invoke(nameof(Disable), 3f);
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
        
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            int damage = ElementalReaction.ElementTable[spellType][enemy.enemyType];
            enemy.TakeDamage(damage);
            Disable();
        }
    }
    private void FixedUpdate()
    {
        transform.position += direction * SpellSpeed * Time.fixedDeltaTime;
    }
}

