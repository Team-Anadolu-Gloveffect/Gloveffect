using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNameManager : MonoBehaviour
{
    public static string selectedSceneName;
    
    public static void ChangeSelectedSceneName(string newName)
    {
        selectedSceneName = newName;
    }

    public void SelectedSceneLoad()
    {
        SceneManager.LoadScene(selectedSceneName);
    }
}
