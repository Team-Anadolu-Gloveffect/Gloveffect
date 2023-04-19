using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellAttackController : MonoBehaviour
{        
    [SerializeField] private GameObject poolParent;
    [SerializeField] private Transform rightGloveSpawnPoint, leftGloveSpawnPoint;
    private float spellSpeed;
    private float spellDamage;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
            CastRightGloveSpell();

        if (Input.GetMouseButtonDown(0))
            CastLeftGloveSpell();

        //if (Input.GetKeyDown(KeyCode.Space))
        //    CastForPlayTest();
    }

    public void CastRightGloveSpell()
    {
        GameObject rightGloveSpell = ObjectPoolingManager.Instance.GetPooledObject("Cryonite");
        if (rightGloveSpell != null)
        {
            rightGloveSpell.transform.parent = poolParent.transform;
            rightGloveSpell.transform.position = rightGloveSpawnPoint.position;
            rightGloveSpell.transform.rotation = rightGloveSpawnPoint.rotation;
            rightGloveSpell.GetComponent<Spell>().direction = transform.forward;
            spellSpeed = rightGloveSpell.GetComponent<Spell>().SpellSpeed;
            spellDamage = rightGloveSpell.GetComponent<Spell>().SpellDamage;
            rightGloveSpell.SetActive(true);
        }
    }
    public void CastLeftGloveSpell()
    {
        GameObject leftGloveSpell = ObjectPoolingManager.Instance.GetPooledObject("Flametron");
        if (leftGloveSpell != null)
        {
            leftGloveSpell.transform.parent = poolParent.transform;
            leftGloveSpell.transform.position = leftGloveSpawnPoint.position;
            leftGloveSpell.transform.rotation = leftGloveSpawnPoint.rotation;
            leftGloveSpell.GetComponent<Spell>().direction = transform.forward;
            spellSpeed = leftGloveSpell.GetComponent<Spell>().SpellSpeed;
            spellDamage = leftGloveSpell.GetComponent<Spell>().SpellDamage;
            leftGloveSpell.SetActive(true);
        }   
    }

    //This one is for play test.
    //public void CastForPlayTest()
    //{
    //    GameObject leftGloveSpell = ObjectPoolingManager.Instance.GetPooledObject("DarkMagic");
    //    if (leftGloveSpell != null)
    //    {
    //        leftGloveSpell.transform.parent = poolParent.transform;
    //        leftGloveSpell.transform.position = leftGloveSpawnPoint.position;
    //        leftGloveSpell.transform.rotation = leftGloveSpawnPoint.rotation;
    //        spellSpeed = leftGloveSpell.GetComponent<Spell>().SpellSpeed;
    //        spellDamage = leftGloveSpell.GetComponent<Spell>().SpellDamage;
    //        leftGloveSpell.SetActive(true);
    //    }
    //}
}
