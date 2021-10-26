using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidEnemy : Enemy
{
    [Header("Equipment")]
    [SerializeField] protected Item head;
    [SerializeField] protected Item shoulders;
    [SerializeField] protected Item chest;
    [SerializeField] protected Item waist;
    [SerializeField] protected Item legs;
    [SerializeField] protected Item hands;
    [SerializeField] protected Item feet;

    [Space]
    [SerializeField] protected Item leftHand;
    [SerializeField] protected Item rightHand;

    [Space]
    [Header("Armor")]
    [Range(0, 1)]
    [SerializeField] protected float armorDestructionRate;
    [Range(0, 1)]
    [SerializeField] protected float armorEffieciency;

    #region Getters & Setters
    public Item Headpiece { get { return head; } }
    public Item Shoulders { get { return shoulders; } }
    public Item Chestpiece { get { return chest; } }
    public Item Legpiece { get { return legs; } }
    public Item Gloves { get { return hands; } }
    public Item Boots { get { return feet; } }
    public Item Belt { get { return waist; } }
    public Item LeftHand { get { return leftHand; } }
    public Item RightHand { get { return rightHand; } }

    public float ArmorEffieciency { get { return armorEffieciency; } }
    public float ArmorDestructionRate { get { return armorDestructionRate; } }
    #endregion

    public new void CountStats()
    {
        Item[] equippedItems = { head, shoulders, chest, waist, legs, hands, feet, leftHand, rightHand };

        enemyDamage = enemySO.BaseDamage;
        enemyAgility = enemySO.EnemyBaseAgility;
        enemyStamina = enemySO.EnemyBaseStamina;
        enemyStrength = enemySO.EnemyBaseStrength;
        enemyIntellect = enemySO.EnemyBaseIntellect;
        enemyArmor = enemySO.BaseArmor;
        attackSpeed = enemySO.BaseAttackSpeed;

        foreach (Item item in equippedItems)
        {
            if (item)
            {
                if (item.ItemInfo.Durability <= 0)
                    continue;

                if (item.ItemInfo.WeaponType != WeaponType.NONE)
                {
                    if (attackSpeed != enemySO.BaseAttackSpeed)
                    {
                        attackSpeed += item.ItemInfo.AttackSpeed;
                        attackSpeed /= 2;
                    }
                    else
                    {
                        attackSpeed = item.ItemInfo.AttackSpeed;
                    }
                }

                enemyAgility += item.ItemInfo.Agility;
                enemyStamina += item.ItemInfo.Stamina;
                enemyStrength += item.ItemInfo.Strength;
                enemyIntellect += item.ItemInfo.Intellect;
                enemyArmor += Mathf.RoundToInt(item.ItemInfo.Armor * item.ItemInfo.Durability / 100);
                enemyDamage += item.ItemInfo.Damage;
            }
        }
    }

    public void ReduceItemsDurabilities()
    {
        Item[] equippedItems = { head, shoulders, chest, waist, legs, hands, feet };

        foreach (Item item in equippedItems)
        {
            if (item)
            {
                if (item.ItemInfo.Durability > armorDestructionRate)
                    item.ItemInfo.Durability -= item.ItemInfo.Durability * armorDestructionRate;
                else
                    item.ItemInfo.Durability = 0;
            }
        }
    }
}
