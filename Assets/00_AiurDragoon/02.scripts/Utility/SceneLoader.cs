using UnityEngine;
using Defines;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader SceneLoaderInstance;


    private void Awake()
    {
        SceneLoader.SceneLoaderInstance = this;
    }


    private void Start()
    {
        DontDestroyOnLoad(SceneLoaderInstance);
    }



    public void ChangeScene(eSceneNames Scene)
    {
        SceneManager.LoadScene(Scene.ToString());
    }



}
