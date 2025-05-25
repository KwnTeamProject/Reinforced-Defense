using UnityEngine;

public interface IEnemy
{
    int maxHealth { get; set; }
    int currentHealth { get; set; }
    int defense { get; set; }

    [Range(0f, 1f)] float physicalResistance { get; set; }
    [Range(0f, 1f)] float magicalResistance { get; set; }
    [Range(0f, 1f)] float elementalResistance { get; set; }

    void OnEnable();
    void OnDisable();
}
