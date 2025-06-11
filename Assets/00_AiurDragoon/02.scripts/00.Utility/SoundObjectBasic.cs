using UnityEngine;

public class SoundObjectBasic : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = UserDataManager.UserDataManagerInstance.Volume;
    }

    private void OnEnable()
    {
        gameObject.GetComponent<AudioSource>().volume = UserDataManager.UserDataManagerInstance.Volume;
    }



}
