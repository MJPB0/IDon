using UnityEngine;

public class EnemyPursuingMode : EnemyMode
{
    public override void EnterMode(EnemyController enemyController)
    {
        enemyController.Enemy.CurrentTarget = enemyController.Player.gameObject.transform;
    }

    public override void EnemyOnTriggerEnter(EnemyController enemyController, Collider other)
    {

    }

    public override void EnemyOnTriggerExit(EnemyController enemyController, Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyController.ChangeEnemyMode(enemyController.enemyIdleMode);
        }
    }

    public override void EnemyUpdate(EnemyController enemyController)
    {
        Vector3 dest = new Vector3(enemyController.Enemy.CurrentTarget.transform.position.x, 0, enemyController.Enemy.CurrentTarget.transform.position.z);
        Vector3 curr = new Vector3(enemyController.transform.position.x, 0, enemyController.transform.position.z);
        float dist = Vector3.Distance(dest, curr);

        if (dist > enemyController.Enemy.AttackRange)
            enemyController.GoToTarget();
        else
            enemyController.ChangeEnemyMode(enemyController.enemyFightingMode);
    }
}
