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
        Vector3 player =  new Vector3(enemyController.getEnemy().CurrentTarget.transform.position.x, 0, enemyController.getEnemy().CurrentTarget.transform.position.z);
        Vector3 enemy = new Vector3(enemyController.transform.position.x, 0, enemyController.transform.position.z);
        float dist = Vector3.Distance(player, enemy);

        if (dist < enemyController.getEnemy().getAttackRange())
        {
            float transX = enemyController.getEnemy().CurrentTarget.transform.position.x - enemyController.transform.position.x;
            float transZ = enemyController.getEnemy().CurrentTarget.transform.position.z - enemyController.transform.position.z;
            Vector3 translation = new Vector3(transX, 0, transZ);

            float targetAngle = Mathf.Atan2(translation.x, translation.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(enemyController.getEnemy().getBody().transform.eulerAngles.y, targetAngle, ref enemyController.getEnemy().turnSmoothVelocity, enemyController.getEnemy().TurnSmoothTime());
            enemyController.getEnemy().getBody().transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if (enemyController.getEnemy().TimeToAttack <= 0)
            {
                enemyController.getEnemy().TimeToAttack = enemyController.getEnemy().getAttackSpeed();
                if (enemyController.getEnemy().CanAttack)
                    enemyController.Attack();
            }
        }
        else
        {
            enemyController.ChangeEnemyMode(enemyController.enemyPursuingMode);
        }
    }
}
