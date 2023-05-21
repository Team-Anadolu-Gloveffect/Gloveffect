using UnityEngine;

public class RageModeController : MonoBehaviour
{
    #region Singleton
    public static RageModeController Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    
    public delegate void RageModeChangedDelegate(bool isRageModeActive);
    public static event RageModeChangedDelegate OnRageModeChanged;

    private bool isRageModeActive;

    public bool IsRageModeActive
    {
        get { return isRageModeActive; }
        set
        {
            if (isRageModeActive != value)
            {
                isRageModeActive = value;
                OnRageModeChanged?.Invoke(isRageModeActive);
            }
        }
    }
    public void ToggleRageMode()
    {
        IsRageModeActive = !IsRageModeActive;
    }
}
