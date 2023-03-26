using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : MonoBehaviour
{
    [SerializeField] private FlametronPoolingManager poolingManager;
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
        var bullet = FlametronPoolingManager.Instance.DequeuePoolableGameObject();
        bullet.transform.position = gunMuzzle.position;
        bullet.GetComponent<BulletController>().IsCalledByPooling = true;
        bullet.GetComponent<Rigidbody>().AddForce(gunMuzzle.forward * bulletSpeed, ForceMode.Impulse);
    }
}
