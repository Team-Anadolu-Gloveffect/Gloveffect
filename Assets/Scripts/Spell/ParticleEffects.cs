using UnityEngine;

public class ParticleEffects : MonoBehaviour, IPooledObject
{
    public void OnObjectSpawn()
    {
        Debug.Log("OnOnjectSpawn is working");
        Invoke(nameof(OnObjectReturn), 0.2f);
    }
    private void OnObjectReturn()
    {
        ObjectPoolingManager.Instance.ReturnToPool(gameObject.tag, gameObject);
    }
}
