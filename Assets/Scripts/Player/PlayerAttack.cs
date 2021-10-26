using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private List<GameObject> enemiesInRange;

    [Space]
    [Range(0, 1)]
    [SerializeField] private float weaponDestructionRate;

    public List<GameObject> EnemiesInRange { get { return enemiesInRange; } }

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
        gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeBetweenAttacks);
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
            float tmp = Time.deltaTime * player.AttackSpeed;
            player.TimeToNextAttack -= tmp;
            gameUI.ManagePlayerAttackTimer(player.TimeToNextAttack);
            gameUI.ToggleTimeToNextAttackSlider(true);
        }
        else 
        {
            player.CanAttack = true;
            gameUI.UpdatePlayerAttackTimer(player.TimeBetweenAttacks, player.TimeBetweenAttacks);
            gameUI.ToggleTimeToNextAttackSlider(false);
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
        if (player.LeftHand == null || player.LeftHand.ItemInfo.WeaponType != WeaponType.TWOHANDED)
        {
            Attack1Hand(isCharged);
        }else if (player.LeftHand.ItemInfo.WeaponType == WeaponType.TWOHANDED)
        {
            Attack2Hand(isCharged);
        }

    }

    private void Attack1Hand(bool isCharged) {
        GameObject closestEnemy = GetClosestEnemy();

        if (!closestEnemy || !player.CanAttack)
            return;

        float min = 0f;
        float max = 1f;
        float tmp = Random.Range(min, max);
        bool isCritical = false;
        if (tmp > player.PlayerCriticalStrikeChance)
            isCritical = true;

        player.TimeToNextAttack = player.TimeBetweenAttacks;

        if (enemiesInRange.Count > 0)
            player.ReduceWeaponsDurabilities(weaponDestructionRate);

        float dmg = CalculateDamage(isCharged, isCritical);
        closestEnemy.GetComponent<EnemyController>().EnemyAttacked(dmg);

        if (closestEnemy.GetComponent<Enemy>().EnemyHealth <= 0)
            enemiesInRange.Remove(closestEnemy);

        player.CanAttack = false;
        if (isCharged)
        {
            player.TimeToNextAttack = player.TimeBetweenAttacks * player.HeavyAttackTimeMultiplier;
            gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
        }
        else
        {
            player.TimeToNextAttack = player.TimeBetweenAttacks;
            gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
        }
    }

    private void Attack2Hand(bool isCharged)
    {
        if (player.CanAttack)
        {
            player.TimeToNextAttack = player.TimeBetweenAttacks;
            if (enemiesInRange.Count > 0)
                player.ReduceWeaponsDurabilities(weaponDestructionRate);

            float tmp = Random.Range(0, 1);
            bool isCritical = false;
            if (tmp > player.PlayerCriticalStrikeChance)
                isCritical = true;

            foreach (GameObject enemy in enemiesInRange)
            {
                float dmg = CalculateDamage(isCharged, isCritical);
                enemy.GetComponent<EnemyController>().EnemyAttacked(dmg);
            }
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i].GetComponent<Enemy>().EnemyHealth <= 0)
                {
                    enemiesInRange.Remove(enemiesInRange[i]);
                    if (enemiesInRange.Count > i)
                        i--;
                }
            }
            player.CanAttack = false;
            if (isCharged)
            {
                player.TimeToNextAttack = player.TimeBetweenAttacks * player.HeavyAttackTimeMultiplier;
                gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
            }
            else
            {
                player.TimeToNextAttack = player.TimeBetweenAttacks;
                gameUI.UpdatePlayerAttackTimer(player.TimeToNextAttack, player.TimeToNextAttack);
            }
        }
    }

    private float CalculateDamage(bool isCharged, bool isCritical)
    {
        float healthToTake = player.PlayerDamage;
        if (isCharged)
            healthToTake = Mathf.RoundToInt(healthToTake * player.HeavyAttackMultiplier);
        if (isCritical)
            healthToTake = Mathf.RoundToInt(healthToTake * player.PlayerCriticalStrikeMultiplier);
        return healthToTake;
    }
}
