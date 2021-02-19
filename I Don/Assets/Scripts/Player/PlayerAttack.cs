using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Player player;
    GameUI gameUI;
    [SerializeField] List<GameObject> enemiesInRange;

    [Space]
    [Range(0, 100)]
    [SerializeField] float weaponDestructionRate;

    public List<GameObject> getEnemiesInRange() { return enemiesInRange; }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && !enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        gameUI = FindObjectOfType<GameUI>();

        gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.getTimeBetweenAttacks());
    }

    private void Update()
    {
        ManageAttackTime();
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        if (enemiesInRange.Contains(enemy))
            enemiesInRange.Remove(enemy);
    }

    private void ManageAttackTime()
    {
        if (player.TimeToNextAttack > 0)
        {
            float tmp = Time.deltaTime * player.getAttackSpeed();
            player.TimeToNextAttack -= tmp;
            gameUI.ManagePlayerAttackTimer(player.TimeToNextAttack);
        }
        else if (!FindObjectOfType<PlayerController>().IsDizzy)   
        {
            player.CanAttack = true;
            gameUI.UpdatePlayerAttackTimer(player.getTimeBetweenAttacks(), player.getTimeBetweenAttacks());
        }
    }

    public GameObject GetClosestEnemy()
    {
        if (enemiesInRange.Count != 0)
        {
            GameObject closestEnemy = enemiesInRange[0];
            foreach (GameObject enemy in enemiesInRange)
            {
                if (Vector3.Distance(transform.position, closestEnemy.transform.position) > Vector3.Distance(transform.position, enemy.transform.position))
                {
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }
        return null;
    }

    public void TryToAttack(bool isCharged)
    {
        if (player.LeftHand == null || player.LeftHand.itemInfo.getWeaponType != WeaponType.TWOHANDED)
        {
            AttackNo2Hand(isCharged);
        }else if (player.LeftHand.itemInfo.getWeaponType == WeaponType.TWOHANDED)
        {
            Attack2Hand(isCharged);
        }

    }

    private void AttackNo2Hand(bool isCharged) {
        GameObject closestEnemy = GetClosestEnemy();
        if (closestEnemy && player.CanAttack)
        {
            float min = 0f;
            float max = 1f;
            float tmp = Random.Range(min, max);
            bool isCritical = false;
            if (tmp > player.PlayerCriticalStrikeChance)
                isCritical = true;

            player.TimeToNextAttack = player.getTimeBetweenAttacks();
            player.ReduceWeaponsDurabilities(weaponDestructionRate);
            Enemy target = closestEnemy.GetComponent<Enemy>();                                              

            int healthToTake = player.getPlayerDamage() - Mathf.RoundToInt(target.EnemyArmor * target.getArmorEffieciency());
            if (healthToTake < 0)
                healthToTake = 0;
            if (isCharged)
                healthToTake = Mathf.RoundToInt(healthToTake * player.getHeavyAttackMultiplier());
            if (isCritical)
                healthToTake = Mathf.RoundToInt(healthToTake * player.PlayerCriticalStrikeMultiplier);
            closestEnemy.GetComponent<EnemyController>().TakeDamage(healthToTake, player);
            //Debug.Log("Player attacked " + closestEnemy.name + " for " + healthToTake + "dmg");
            if (closestEnemy.GetComponent<Enemy>().EnemyHealth <= 0)
            {
                Debug.Log($"Deleting {closestEnemy.name} from enemiesInRange Array");
                enemiesInRange.Remove(closestEnemy);
            }
            player.CanAttack = false;
            if (isCharged)
            {
                player.TimeToNextAttack = player.getTimeBetweenAttacks() * 2f;
                gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
            }
            else
            {
                player.TimeToNextAttack = player.getTimeBetweenAttacks();
                gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
            }
        }
    }

    private void Attack2Hand(bool isCharged)
    {
        if (player.CanAttack)
        {
            player.TimeToNextAttack = player.getTimeBetweenAttacks();
            player.ReduceWeaponsDurabilities(weaponDestructionRate);

            float tmp = Random.Range(0, 1);
            bool isCritical = false;
            if (tmp > player.PlayerCriticalStrikeChance)
                isCritical = true;

            foreach (GameObject enemy in enemiesInRange)
            {
                Enemy target = enemy.GetComponent<Enemy>();
                int healthToTake = player.getPlayerDamage() - Mathf.RoundToInt(target.EnemyArmor * target.getArmorEffieciency());
                if (healthToTake < 0)
                    healthToTake = 0;
                if (isCharged)
                    healthToTake = Mathf.RoundToInt(healthToTake * player.getHeavyAttackMultiplier());
                if (isCritical)
                    healthToTake = Mathf.RoundToInt(healthToTake * player.PlayerCriticalStrikeMultiplier);
                enemy.GetComponent<EnemyController>().TakeDamage(healthToTake, player);
                //Debug.Log("Player attacked " + enemy.name + " for " + healthToTake + "dmg [AOE]");
            }
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                Debug.Log($"Checking {enemiesInRange[i].name}...");
                if (enemiesInRange[i].GetComponent<Enemy>().EnemyHealth <= 0)
                {
                    Debug.Log($"Deleting {enemiesInRange[i].name} from enemiesInRange Array");
                    enemiesInRange.Remove(enemiesInRange[i]);
                    if (enemiesInRange.Count > i)
                        i--;
                }
            }
            player.CanAttack = false;
            if (isCharged)
            {
                player.TimeToNextAttack = player.getTimeBetweenAttacks() * 2f;
                gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
            }
            else
            {
                player.TimeToNextAttack = player.getTimeBetweenAttacks();
                gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
            }
        }
    }
}
