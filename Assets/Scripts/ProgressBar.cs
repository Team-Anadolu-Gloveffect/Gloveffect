using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Radial Progress Bar")]
    public static void AddRadialProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Radial Progress Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
#endif
    public int max;
    public int current;
    public Image mask;
    public Image fill;
    public Color color;
    public GameObject rageButton;

    private bool _rageModeCanUse;
    [SerializeField] private int _decreaseAmount = 1;
    [SerializeField] private float _decreaseInterval = 1f;

    void Update()
    {
        GetCurrentFill();
        
        if (Input.GetKeyDown(KeyCode.R) && _rageModeCanUse)
        {
            RageModeController.Instance.ToggleRageMode();
            rageButton.SetActive(false);
            StartCoroutine(DecreaseRage());
        }
        if(Input.GetKeyDown(KeyCode.Space)) IncreaseRage(25);
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)max;
        mask.fillAmount = fillAmount;

        fill.color = color;
    }

    public void IncreaseRage(int rage)
    {
        current = current + rage;

        if (current >= max)
        {
            rageButton.SetActive(true);
            _rageModeCanUse = true;
        }
    }

    IEnumerator DecreaseRage()
    {
        while (current > 0)
        {
            yield return new WaitForSeconds(_decreaseInterval);

            current -= _decreaseAmount;
        }
        if (current <= 0)
        {
            RageModeController.Instance.ToggleRageMode();
            current = 0;
            _rageModeCanUse = false;
        }
    }
}
