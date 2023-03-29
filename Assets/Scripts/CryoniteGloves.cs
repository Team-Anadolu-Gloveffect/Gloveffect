using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoniteGloves : MonoBehaviour
{
    [SerializeField] private CryonitePoolingManager poolingManager;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private float bulletSpeed = 10;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            FireBulletWithPooling();
        }
    }
    private void FireBulletWithPooling()
    {
        var bullet = CryonitePoolingManager.Instance.DequeuePoolableGameObject();
        bullet.transform.position = gunMuzzle.position;
        bullet.GetComponent<BulletController>().IsCalledByPooling = true;
        bullet.GetComponent<Rigidbody>().AddForce(gunMuzzle.forward * bulletSpeed, ForceMode.Impulse);
    }
}
