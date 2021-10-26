using UnityEngine;

public abstract class EnemyMode
{
    public abstract void EnterMode(EnemyController enemy);
    public abstract void EnemyUpdate(EnemyController enemy);
    public abstract void EnemyOnTriggerEnter(EnemyController enemy, Collider other);
    public abstract void EnemyOnTriggerExit(EnemyController enemy, Collider other);
}
