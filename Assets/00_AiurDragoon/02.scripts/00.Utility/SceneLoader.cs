using UnityEngine;
using Defines;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader SceneLoaderInstance;


    private void Awake()
    {
        SceneLoaderInstance = this;
    }


    private void Start()
    {
        DontDestroyOnLoad(SceneLoaderInstance);
    }



    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }



}
