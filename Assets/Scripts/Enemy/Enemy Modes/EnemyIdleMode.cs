using UnityEngine;

public class EnemyIdleMode : EnemyMode
{
    bool isPatrolUnit = false;

    public override void EnterMode(EnemyController enemyController)
    {
        if (enemyController.Enemy.EnemyType == EnemyTypes.ENEMY || enemyController.Enemy.EnemyType == EnemyTypes.HUMANOID)
            isPatrolUnit = false;
        else
            isPatrolUnit = true;
    }

    public override void EnemyOnTriggerEnter(EnemyController enemyController, Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enemyController.Enemy.WillPursue)
        {
            enemyController.ChangeEnemyMode(enemyController.enemyPursuingMode);
        }
        else if (other.gameObject.CompareTag("Player") && PlayerInRange(enemyController.Enemy, enemyController.Player.transform))
        {
            enemyController.Enemy.CurrentTarget = enemyController.Player.gameObject.transform;
            enemyController.ChangeEnemyMode(enemyController.enemyFightingMode);
        }
    }

    public override void EnemyOnTriggerExit(EnemyController enemyController, Collider other)
    {

    }

    public override void EnemyUpdate(EnemyController enemyController)
    {
        Enemy enemy = enemyController.Enemy;
        if (enemy.CurrentIdleTime < enemy.IdleTime)
            enemy.CurrentIdleTime += Time.deltaTime;
        else if (!isPatrolUnit && ShouldReturnToWatchPost(enemy.gameObject.transform.position, enemy.BasePosition.position))
        {
            enemyController.ChangeEnemyMode(enemyController.enemyReturnMode);
            enemy.CurrentIdleTime = 0f;
        }
        else if (isPatrolUnit && enemyController.gameObject.TryGetComponent<PatrollingEnemyController>(out PatrollingEnemyController c))
        {
            c.ChangeEnemyMode(c.enemyPatrollingMode);
            enemy.CurrentIdleTime = 0f;
        }
    }

    private bool ShouldReturnToWatchPost(Vector3 enemyPos, Vector3 enemyBasePos)
    {
        if (Vector3.Distance(enemyPos, enemyBasePos) > 1f)
            return true;
        return false;
    }

    private bool PlayerInRange(Enemy enemy, Transform player)
    {
        if (Vector3.Distance(enemy.transform.position, player.position) <= enemy.AttackRange)
            return true;
        else
            return false;
    }
}
