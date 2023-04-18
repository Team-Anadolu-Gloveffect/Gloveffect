using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellExplosion : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(Disable), 1.5f);
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
