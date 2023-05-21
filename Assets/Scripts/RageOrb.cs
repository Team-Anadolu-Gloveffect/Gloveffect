using UnityEngine;

public class RageOrb : MonoBehaviour
{
    private bool _canTakeOrb;
    private void OnCollisionEnter(Collision collision)
    {
        ProgressBar increaseRage = collision.gameObject.GetComponent<ProgressBar>();
        if (collision.collider.CompareTag("Player") && _canTakeOrb)
        {
            increaseRage.IncreaseRage(25);
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
