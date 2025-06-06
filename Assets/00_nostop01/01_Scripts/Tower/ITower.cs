using UnityEngine;

public interface ITower
{
    int AttackPower { get; set; }
    int AttackSpeed { get; set; }
    float AttackRange {  get; set; }
    
    void OnSelected();
    void OnDeselected();
    void Upgrade();
    void Attack();
    void TowerDeSpawn();
    Transform GetTransform();
}
