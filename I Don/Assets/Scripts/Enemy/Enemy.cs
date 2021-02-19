using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public UnityEvent enemyDeath;

    [Space]
    [Header("Movement")]
    [SerializeField] Transform currentTarget;

    [Space]
    [SerializeField] float idleTime = 2f;
    [SerializeField] float currentIdleTime = 0f;

    [Space]
    [SerializeField] Transform[] waypoints;
    [SerializeField] int waypointIndex = 0;
    [SerializeField] float distanceToWaypoint = .5f;

    [Space]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    [SerializeField] GameObject body;

    [Space]
    [Header("Equipment")]
    [SerializeField] Item head;
    [SerializeField] Item shoulders;
    [SerializeField] Item chest;
    [SerializeField] Item waist;
    [SerializeField] Item legs;
    [SerializeField] Item hands;
    [SerializeField] Item feet;

    [Space]
    [SerializeField] Item leftHand;
    [SerializeField] Item rightHand;

    [Space]
    [Header("Stats")]
    [SerializeField] int enemyHealth = 100;
    [SerializeField] int enemyMana = 100;

    [Space]
    [SerializeField] int enemyDamage;
    [SerializeField] int armorPiercing;
    [SerializeField] int enemyArmor;
    [Range(0, 1)]
    [SerializeField] float armorEffieciency;

    [Space]
    [SerializeField] int enemyAgility;
    [SerializeField] int enemyStrength;
    [SerializeField] int enemyStamina;
    [SerializeField] int enemyIntellect;

    [Header("Enemy Base Stats")]
    [SerializeField] int baseEnemyDamage;
    [SerializeField] float baseAttackSpeed;
    [SerializeField] int baseEnemyArmor;

    [Space]
    [SerializeField] int baseEnemyAgility;
    [SerializeField] int baseEnemyStrength;
    [SerializeField] int baseEnemyStamina;
    [SerializeField] int baseEnemyIntellect;

    [Space]
    [Header("Battle Settings")]
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    [SerializeField] float timeToAttack = 0;
    [SerializeField] bool canBeHit = true;
    [SerializeField] bool canAttack = true;

    public Transform CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }
    public Transform[] getWaypoints() { return waypoints; }
    public int WaypointIndex { get { return waypointIndex; } set { waypointIndex = value; } }
    public float getDistanceToWaypoint() { return distanceToWaypoint; }

    public float getIdleTime() { return idleTime; }
    public float CurrentIdleTime { get { return currentIdleTime; } set { currentIdleTime = value; } }

    public float getMoveSpeed() { return moveSpeed; }
    public float getRotateSpeed() { return rotateSpeed; }
    public float TurnSmoothTime() { return turnSmoothTime; }

    public GameObject getBody() { return body; }

    public Item Headpiece { get { return head; } set { head = value; } }
    public Item Shoulders { get { return shoulders; } set { shoulders = value; } }
    public Item Chestpiece { get { return chest; } set { chest = value; } }
    public Item Legpiece { get { return legs; } set { legs = value; } }
    public Item Gloves { get { return hands; } set { hands = value; } }
    public Item Boots { get { return feet; } set { feet = value; } }
    public Item Belt { get { return waist; } set { waist = value; } }
    public Item LeftHand { get { return leftHand; } set { leftHand = value; } }
    public Item RightHand { get { return rightHand; } set { rightHand = value; } }

    public float getAttackRange() { return attackRange; }
    public float getAttackSpeed() { return attackSpeed; }
    public float TimeToAttack { get { return timeToAttack; } set { timeToAttack = value; } }
    public int getDamage() { return enemyDamage; }

    public int getArmorPiercing { get { return armorPiercing; } }
    public int EnemyArmor { get { return enemyArmor; } set { enemyArmor = value; } }
    public float getArmorEffieciency() { return armorEffieciency; }

    public int EnemyHealth { get { return enemyHealth; } set { enemyHealth = value; } }
    public bool CanBeHit { get { return canBeHit; } set { canBeHit = value; } }
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    public void CountStats()
    {
        Item[] equippedItems = { head, shoulders, chest, waist, legs, hands, feet, leftHand, rightHand };

        enemyDamage = baseEnemyDamage;
        enemyAgility = baseEnemyAgility;
        enemyStamina = baseEnemyStamina;
        enemyStrength = baseEnemyStrength;
        enemyIntellect = baseEnemyIntellect;
        enemyArmor = baseEnemyArmor;
        attackSpeed = baseAttackSpeed;

        foreach (Item item in equippedItems)
        {
            if (item)
            {
                if (item.itemInfo.Durability <= 0)
                    continue;

                if (item.itemInfo.getWeaponType != WeaponType.NONE)
                {
                    if (attackSpeed != baseAttackSpeed)
                    {
                        attackSpeed += item.itemInfo.getAttackSpeed;
                        attackSpeed /= 2;
                    }
                    else
                    {
                        attackSpeed = item.itemInfo.getAttackSpeed;
                    }
                }

                enemyAgility += item.itemInfo.getAgility;
                enemyStamina += item.itemInfo.getStamina;
                enemyStrength += item.itemInfo.getStrength;
                enemyIntellect += item.itemInfo.getIntellect;
                enemyArmor += Mathf.RoundToInt(item.itemInfo.getArmor * item.itemInfo.Durability / 100);
                enemyDamage += item.itemInfo.getDamage;
            }
        }
    }

    public void ReduceItemsDurabilities(int value)
    {
        Item[] equippedItems = { head, shoulders, chest, waist, legs, hands, feet };

        foreach (Item item in equippedItems)
        {
            if (item)
            {
                if (item.itemInfo.Durability > value)
                    item.itemInfo.Durability -= value;
                else
                    item.itemInfo.Durability = 0;
            }
        }
    }
    public void EnemyDeath()
    {
        enemyDeath?.Invoke();
    }
}
