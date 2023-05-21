using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public string tag;
    public Color color;
}
public class EnemyTypeManager : MonoBehaviour
{
    public EnemyType[] enemyTypes;

    private void Start()
    {
        foreach (EnemyType enemyType in enemyTypes)
        {
            if (gameObject.CompareTag(enemyType.tag))
            {
                ChangeColor(enemyType.color);
                break;
            }
        }
    }

    private void ChangeColor(Color newColor)
    {
        Transform visualChild = transform.Find("Visual");
        if (visualChild != null)
        {
            Transform ch44Child = visualChild.Find("Ch44");
            if (ch44Child != null)
            {
                Renderer renderer = ch44Child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    material.SetColor("_Color", newColor);
                }
            }
        }
    }
}