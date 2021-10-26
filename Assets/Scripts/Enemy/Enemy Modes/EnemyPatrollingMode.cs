using UnityEngine;

public class EnemyPatrollingMode : EnemyMode
{
    public override void EnterMode(EnemyController enemyController)
    {
        if (!enemyController.gameObject.TryGetComponent(out PatrollingEnemyController c))
            enemyController.ChangeEnemyMode(enemyController.enemyIdleMode);
        else
            c.Enemy.CurrentTarget = c.Waypoints[c.WaypointIndex];
    }

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
        if (!enemyController.gameObject.TryGetComponent(out PatrollingEnemyController c))
        {
            enemyController.ChangeEnemyMode(enemyController.enemyIdleMode);
            return;
        }

        Vector3 dest = new Vector3(enemyController.Enemy.CurrentTarget.transform.position.x, 0, enemyController.Enemy.CurrentTarget.transform.position.z);
        Vector3 curr = new Vector3(enemyController.transform.position.x, 0, enemyController.transform.position.z);
        float dist = Vector3.Distance(dest, curr);

        if (dist > c.DistanceToWaypoint)
            enemyController.GoToTarget();
        else
        {
            enemyController.ChangeEnemyMode(enemyController.enemyIdleMode);
            if (c.WaypointIndex + 1 >= c.Waypoints.Length)
            {
                c.WaypointIndex = 0;
                c.Enemy.CurrentTarget = c.Waypoints[0];
            }
            else
                c.Enemy.CurrentTarget = c.Waypoints[c.WaypointIndex++];
        }
    }
}
