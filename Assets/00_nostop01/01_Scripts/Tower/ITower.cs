using UnityEngine;

public interface ITower
{
    int AttackPower { get; set; }
    int AttackSpeed { get; set; }
    float AttackRange {  get; set; }
    

    void Attack();
    void Upgrade();
    void TowerDeSpawn();
    Transform GetTransform();
}
