using Enums;
using UnityEngine;

public class Spell : MonoBehaviour, IPooledObject
{
    public float SpellSpeed = 10f;
    public SpellTypes spellType;

    public Vector3 direction = Vector3.zero;

    private void Start()
    {
        direction = transform.forward;
    }
    public void OnObjectSpawn()
    {      
        Invoke(nameof(OnObjectReturn), 3f);
    }

    public void OnObjectReturn()
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
            OnObjectReturn();
        }
    }
    private void FixedUpdate()
    {
        transform.position += direction * SpellSpeed * Time.fixedDeltaTime;
    }
}

