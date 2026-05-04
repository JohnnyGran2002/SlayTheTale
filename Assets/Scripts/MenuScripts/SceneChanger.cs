using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private int targetScene;
    
    public void ChangeScene()
    {
        SceneManager.LoadScene(targetScene);
    }
}
