using UnityEngine;

public class UIController : MonoBehaviour
{

    public void Start()
    {
        ObjectDisable();
    }

    public void ObjectDisable()
    {
        this.gameObject.SetActive(false);
    }
    
}
