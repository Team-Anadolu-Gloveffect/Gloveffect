using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAttackController : MonoBehaviour
{        
    [SerializeField] private GameObject poolParent;
    [SerializeField] private Transform rightGloveSpawnPoint, leftGloveSpawnPoint;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
            CastRightGloveSpell();

        if (Input.GetMouseButtonDown(0))
            CastLeftGloveSpell();
    }

    public void CastRightGloveSpell()
    {
        GameObject rightGloveSpell = ObjectPoolingManager.Instance.GetPooledObject(TagManager.rightGloveTag);
        if (rightGloveSpell != null)
        {
            rightGloveSpell.transform.parent = poolParent.transform;
            rightGloveSpell.transform.position = rightGloveSpawnPoint.position;
            rightGloveSpell.transform.rotation = rightGloveSpawnPoint.rotation;
            rightGloveSpell.GetComponent<Spell>().direction = transform.forward;
            rightGloveSpell.SetActive(true);
        }
    }
    public void CastLeftGloveSpell()
    {
        GameObject leftGloveSpell = ObjectPoolingManager.Instance.GetPooledObject(TagManager.leftGloveTag);
        if (leftGloveSpell != null)
        {
            leftGloveSpell.transform.parent = poolParent.transform;
            leftGloveSpell.transform.position = leftGloveSpawnPoint.position;
            leftGloveSpell.transform.rotation = leftGloveSpawnPoint.rotation;
            leftGloveSpell.GetComponent<Spell>().direction = transform.forward;
            leftGloveSpell.SetActive(true);
        }   
    }
}
