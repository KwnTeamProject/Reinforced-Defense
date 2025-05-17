using UnityEngine;

public interface IEnemy
{
    float speed { get; set; }
    float rayDistance { get; set; }

    void OnEnable();
    void OnDisable();
}
