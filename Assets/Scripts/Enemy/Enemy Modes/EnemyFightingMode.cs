using UnityEngine;

public class EnemyFightingMode : EnemyMode
{
    public override void EnterMode(EnemyController enemyController)
    {

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
        Vector3 player =  new Vector3(enemyController.Enemy.CurrentTarget.transform.position.x, 0, enemyController.Enemy.CurrentTarget.transform.position.z);
        Vector3 enemy = new Vector3(enemyController.transform.position.x, 0, enemyController.transform.position.z);
        float dist = Vector3.Distance(player, enemy);

        float transX = enemyController.Enemy.CurrentTarget.transform.position.x - enemyController.Enemy.Body.transform.position.x;
        float transZ = enemyController.Enemy.CurrentTarget.transform.position.z - enemyController.Enemy.Body.transform.position.z;
        Vector3 translation = new Vector3(transX, 0, transZ);

        float targetAngle = Mathf.Atan2(translation.x, translation.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(enemyController.Enemy.Body.transform.eulerAngles.y, targetAngle, ref enemyController.Enemy.turnSmoothVelocity, enemyController.Enemy.TurnSmoothTime);
        enemyController.Enemy.Body.transform.rotation = Quaternion.Euler(0f, angle, 0f);

        if (dist <= enemyController.Enemy.AttackRange && enemyController.Enemy.TimeToAttack <= 0)
        {
            enemyController.Enemy.TimeToAttack = enemyController.Enemy.AttackSpeed;
            if (enemyController.Enemy.CanAttack)
                enemyController.Attack();
        }
        else if (dist > enemyController.Enemy.AttackRange && enemyController.Enemy.WillPursue)
        {
            enemyController.ChangeEnemyMode(enemyController.enemyPursuingMode);
        }
    }
}
