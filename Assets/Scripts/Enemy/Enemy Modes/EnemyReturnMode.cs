using UnityEngine;

public class EnemyReturnMode : EnemyMode
{
    public override void EnemyOnTriggerEnter(EnemyController enemyController, Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enemyController.Enemy.WillPursue)
        {
            enemyController.ChangeEnemyMode(enemyController.enemyPursuingMode);
        }
    }

    public override void EnemyOnTriggerExit(EnemyController enemyController, Collider other)
    {

    }

    public override void EnemyUpdate(EnemyController enemyController)
    {
        Vector3 dest = new Vector3(enemyController.Enemy.CurrentTarget.transform.position.x, 0, enemyController.Enemy.CurrentTarget.transform.position.z);
        Vector3 curr = new Vector3(enemyController.transform.position.x, 0, enemyController.transform.position.z);
        float dist = Vector3.Distance(dest, curr);

        if (dist > 1f)
            enemyController.GoToTarget();
        else
            enemyController.ChangeEnemyModeToIdle();
    }

    public override void EnterMode(EnemyController enemyController)
    {
        enemyController.Enemy.CurrentTarget = enemyController.Enemy.BasePosition;
    }
}
