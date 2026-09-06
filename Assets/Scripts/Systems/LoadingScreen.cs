using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;

    //[SerializeField] private Image loadingBar;

    void Awake()
    {
        //DontDestroyOnLoad(this);
    }
    
    public void LoadScene(string sceneName)
    {
        Debug.Log(sceneName);
        StartCoroutine(AsyncLoad(sceneName));
    }

    IEnumerator AsyncLoad(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            //var progress = Mathf.Clamp(operation.progress / 0.9f);

            yield return null;
        }
    }
}
