using UnityEngine;

public interface ITower
{
    int AttackPower { get; set; }
    int AttackSpeed { get; set; }
    int AttackRange {  get; set; }
    

    void Attack();
    void Upgrade();
    Transform GetTransform();
}
