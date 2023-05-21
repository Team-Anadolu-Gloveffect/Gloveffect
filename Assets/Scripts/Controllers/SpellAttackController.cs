using UnityEngine;

public class SpellAttackController : MonoBehaviour
{        
    [SerializeField] private GameObject poolParent;
    [SerializeField] private Transform rightGloveSpawnPoint, leftGloveSpawnPoint;
    public bool activateSecondGlove = false;
    private bool _canSpellUse; 

    public void Update()
    {
        if (_canSpellUse)
        {
            if (Input.GetMouseButtonDown(1) /*&& activateSecondGlove*/)
                CastRightGloveSpell();

            if (Input.GetMouseButtonDown(0))
                CastLeftGloveSpell();
        }
    }

    private void CastRightGloveSpell()
    {
        GameObject rightGloveSpell = ObjectPoolingManager.Instance.GetPooledObject(TagManager.rightGloveTag);
        if (rightGloveSpell != null)
        {
            rightGloveSpell.transform.parent = poolParent.transform;
            rightGloveSpell.transform.position = rightGloveSpawnPoint.position;
            rightGloveSpell.transform.rotation = rightGloveSpawnPoint.rotation;
            rightGloveSpell.GetComponent<Spell>().direction = transform.forward;
        }
    }
    private void CastLeftGloveSpell()
    {
        GameObject leftGloveSpell = ObjectPoolingManager.Instance.GetPooledObject(TagManager.leftGloveTag);
        if (leftGloveSpell != null)
        {
            leftGloveSpell.transform.parent = poolParent.transform;
            leftGloveSpell.transform.position = leftGloveSpawnPoint.position;
            leftGloveSpell.transform.rotation = leftGloveSpawnPoint.rotation;
            leftGloveSpell.GetComponent<Spell>().direction = transform.forward;
        }   
    }
    
    private void OnEnable()
    {
        RageModeController.OnRageModeChanged += HandleRageModeChanged;
    }

    private void OnDisable()
    {
        RageModeController.OnRageModeChanged -= HandleRageModeChanged;
    }

    private void HandleRageModeChanged(bool isRageModeActive)
    {
        if (isRageModeActive) _canSpellUse = true;

        else _canSpellUse = false;
    }
}

