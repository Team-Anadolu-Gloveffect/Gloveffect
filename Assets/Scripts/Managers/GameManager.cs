using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float spellSpeed;
    [SerializeField] private float spellDamage;
    [SerializeField] private Transform rightGloveSpawnPoint, leftGloveSpawnPoint;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CastRightGloveSpell();
        }
        if (Input.GetMouseButtonDown(0))
        {
            CastLeftGloveSpell();
        }
    }

    public void CastRightGloveSpell()
    {
        GameObject rightGloveSpell = ObjectPoolingManager.Instance.GetPooledObject(TagManager.rightGloveTag);
        if (rightGloveSpell != null)
        {
            rightGloveSpell.transform.position = rightGloveSpawnPoint.position;
            rightGloveSpell.transform.rotation = rightGloveSpawnPoint.rotation;
            spellSpeed = rightGloveSpell.GetComponent<Spell>().speed;
            spellDamage = rightGloveSpell.GetComponent<Spell>().damage;
            rightGloveSpell.SetActive(true);
        }
    }
    public void CastLeftGloveSpell()
    {
        GameObject leftGloveSpell = ObjectPoolingManager.Instance.GetPooledObject(TagManager.leftGloveTag);
        if (leftGloveSpell != null)
        {
            leftGloveSpell.transform.position = leftGloveSpawnPoint.position;
            leftGloveSpell.transform.rotation = leftGloveSpawnPoint.rotation;
            spellSpeed = leftGloveSpell.GetComponent<Spell>().speed;
            spellDamage = leftGloveSpell.GetComponent<Spell>().damage;
            leftGloveSpell.SetActive(true);
        }   
    }
}
