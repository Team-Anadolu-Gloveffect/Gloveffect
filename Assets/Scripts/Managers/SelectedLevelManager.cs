using UnityEngine;

public class SelectedLevelManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        DraggableItem draggable = other.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.SetSelectedLevel(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        DraggableItem draggable = other.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.ResetSelectedLevel();
        }
    }
}
