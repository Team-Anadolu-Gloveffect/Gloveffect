using UnityEngine;

public class RageOrb : MonoBehaviour
{
    private bool _canTakeOrb = true;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ProgressBar increaseRage = collision.gameObject.GetComponent<ProgressBar>();
            if (increaseRage != null && _canTakeOrb)
            {
                Debug.Log("Increased Rage");
                increaseRage.IncreaseRage(25);
            }
            Destroy(gameObject);
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
        if (isRageModeActive) _canTakeOrb = false;

        else _canTakeOrb = true;
    }
}
