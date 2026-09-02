using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string targetScene;
    
    public void ChangeScene()
    {
        SceneManager.LoadScene(targetScene);
    }
}
