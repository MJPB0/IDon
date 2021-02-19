using UnityEngine;

public class EnemyPatrollingMode : EnemyMode
{
    public override void EnterMode(EnemyController enemyController)
    {
        enemyController.getEnemy().CurrentTarget = enemyController.getEnemy().getWaypoints()[enemyController.getEnemy().WaypointIndex];
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
        Vector3 dest = new Vector3(enemyController.getEnemy().CurrentTarget.position.x, 0, enemyController.getEnemy().CurrentTarget.position.z);
        Vector3 curr = new Vector3(enemyController.transform.position.x, 0, enemyController.transform.position.z);
        float dist = Vector3.Distance(dest, curr);

        if (dist > enemyController.getEnemy().getDistanceToWaypoint())
            enemyController.GoToTarget();
        else
        {
            enemyController.ChangeEnemyMode(enemyController.enemyIdleMode);
            if (enemyController.getEnemy().WaypointIndex + 1 >= enemyController.getEnemy().getWaypoints().Length)
            {
                enemyController.getEnemy().WaypointIndex = 0;
                enemyController.getEnemy().CurrentTarget = enemyController.getEnemy().getWaypoints()[0];
            }
            else
                enemyController.getEnemy().CurrentTarget = enemyController.getEnemy().getWaypoints()[enemyController.getEnemy().WaypointIndex++];
        }
    }
}
