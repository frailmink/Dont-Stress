using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetScript : MonoBehaviour
{
    public static void ResetScene()
    {
        // Reload the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}