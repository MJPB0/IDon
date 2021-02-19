using UnityEngine;

public enum Mode { FIGHTING, IDLE, PURSUING, PATROLLING}
public abstract class EnemyMode : MonoBehaviour
{
    public abstract void EnterMode(EnemyController enemy);
    public abstract void EnemyUpdate(EnemyController enemy);
    public abstract void EnemyOnTriggerEnter(EnemyController enemy, Collider other);
    public abstract void EnemyOnTriggerExit(EnemyController enemy, Collider other);
}
