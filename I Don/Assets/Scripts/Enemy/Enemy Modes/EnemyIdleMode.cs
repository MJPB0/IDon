using UnityEngine;

public class EnemyIdleMode : EnemyMode
{
    public override void EnterMode(EnemyController enemyController)
    {

    }

    public override void EnemyOnTriggerEnter(EnemyController enemyController, Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemyController.ChangeEnemyMode(enemyController.enemyPursuingMode);
        }
    }

    public override void EnemyOnTriggerExit(EnemyController enemyController, Collider other)
    {

    }

    public override void EnemyUpdate(EnemyController enemyController)
    {
        if (enemyController.getEnemy().CurrentIdleTime < enemyController.getEnemy().getIdleTime())
        {
            enemyController.getEnemy().CurrentIdleTime += Time.deltaTime;
        }
        else
        {
            enemyController.getEnemy().CurrentIdleTime = 0f;
            enemyController.ChangeEnemyMode(enemyController.enemyPatrollingMode);
        }
    }
}
