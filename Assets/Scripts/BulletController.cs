using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public bool IsCalledByPooling;

    private void OnEnable()
    {
        Invoke(nameof(EnqueueCheck), 1);
    }

    private void EnqueueCheck()
    {
        if (IsCalledByPooling)
        {
            Invoke(nameof(Enqueue), 1);
        }
    }

    private void Enqueue()
    {
        IsCalledByPooling = false;
        ObjectPoolingManager.Instance.GetPooledObject(tag);
    }
}
