using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public Transform selectedLevel;
    public bool isFull = false;
    [SerializeField] private string sceneName;
    [SerializeField] private float minX, maxX, minY, maxY;

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isDragging = true;
    }

    private void OnMouseUp()
    {
        isDragging = false;
        
        if (selectedLevel != null && isFull == false)
        {
            transform.position = selectedLevel.position;
            isFull = true;
        }
    }

    private void Update()
    {
        Debug.Log(sceneName);
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
            transform.position = newPosition;
        }
    }
    
    public void SetSelectedLevel(Transform yuvaTransform)
    {
        if (isFull)
        {
            return;
        }
        selectedLevel = yuvaTransform;
        sceneName = gameObject.name;
        SceneNameManager.ChangeSelectedSceneName(sceneName);
    }

    public void ResetSelectedLevel()
    {
        sceneName = string.Empty;
        SceneNameManager.ChangeSelectedSceneName(sceneName);
        selectedLevel = null;
        isFull = false;
    }
}
