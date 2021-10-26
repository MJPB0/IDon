using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent PlayerDeath;
    public UnityEvent PlayerAttacked;

    [SerializeField] private GameUI gameUI;

    [Space]
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = .5f;

    [Space]
    [Header("Equipment")]
    [SerializeField] private Item head;
    [SerializeField] private Item shoulders;
    [SerializeField] private Item chest;
    [SerializeField] private Item waist;
    [SerializeField] private Item legs;
    [SerializeField] private Item hands;
    [SerializeField] private Item feet;

    [Space]
    [SerializeField] private Item leftHand;
    [SerializeField] private Item rightHand;

    [Space]
    [SerializeField] private Consumable consumable;

    [Space]
    [Header("Stats")]
    [SerializeField] private int maxPlayerHealth = 100;
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private int maxPlayerMana = 100;
    [SerializeField] private int playerMana = 100;

    [Space]
    [SerializeField] private int playerDamage;
    [SerializeField] private int playerArmor;
    [Range(0, 1)]
    [SerializeField] private float armorEffieciency;
    [Range(0, 1)]
    [SerializeField] private float armorDestructionRate;

    [Space]
    [SerializeField] private int playerAgility;
    [SerializeField] private int playerStrength;
    [SerializeField] private int playerStamina;
    [SerializeField] private int playerIntellect;

    [Header("Player Base Stats")]
    [SerializeField] private int basePlayerDamage;
    [SerializeField] private float baseHeavyAttackMultiplier;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private int basePlayerArmor;

    [Space]
    [SerializeField] private int basePlayerAgility;
    [SerializeField] private int basePlayerStrength;
    [SerializeField] private int basePlayerStamina;
    [SerializeField] private int basePlayerIntellect;

    [Header("Player Bonus Stats")]
    [SerializeField] private int bonusPlayerDamage;
    [SerializeField] private float bonusAttackSpeed;

    [Space]
    [SerializeField] private int bonusPlayerAgility;
    [SerializeField] private int bonusPlayerStrength;
    [SerializeField] private int bonusPlayerStamina;
    [SerializeField] private int bonusPlayerIntellect;

    [Header("Sprinting")]
    [Space]
    [SerializeField] private bool isSprinting;
    [SerializeField] private float sprintSpeed = 4f;
    [SerializeField] private float sprintDuration = 100f;
    [Range(0, 100)]
    [SerializeField] private float exhaustion = 0f;
    [SerializeField] private float sprintDepletionPerSecond = 2f;
    [SerializeField] private float restRate = 1.5f;

    [Header("Attacking")]
    [Space]
    [Range(0, 1)]
    [SerializeField] private float criticalStrikeChance;
    [SerializeField] private float criticalStrikeMultiplier;
    [SerializeField] private float heavyAttackMultiplier;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float timeToNextAttack;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float heavyAttackTimeMultiplier;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canBeHit = true;
    [Range(0,1)]
    [SerializeField] private float armorPiercing;
    [SerializeField] private Collider colliderAOE;

    #region Getters & Setters

    public float MoveSpeed { get { return moveSpeed; } }
    public float RotateSpeed { get { return rotateSpeed; } }

    public Item Headpiece { get { return head; } set { head = value; } }
    public Item Shoulders { get { return shoulders; } set { shoulders = value; } }
    public Item Chestpiece { get { return chest; } set { chest = value; } }
    public Item Legpiece { get { return legs; } set { legs = value; } }
    public Item Gloves { get { return hands; } set { hands = value; } }
    public Item Boots { get { return feet; } set { feet = value; } }
    public Item Belt { get { return waist; } set { waist = value; } }
    public Item LeftHand { get { return leftHand; } set { leftHand = value; } }
    public Item RightHand { get { return rightHand; } set { rightHand = value; } }
    public Consumable PlayerConsumable { get { return consumable; } set { consumable = value; } }

    public float SprintSpeed { get { return sprintSpeed; } }
    public float SprintDuration { get { return sprintDuration; } set { sprintDuration = value; } }
    public bool IsSprinting { get { return isSprinting; } set { isSprinting = value; } }
    public float SprintDepletionPerSecond { get { return sprintDepletionPerSecond; } }
    public float Exhaustion { get { return exhaustion; } set { exhaustion = value; } }
    public float RestRate { get { return restRate; } }

    public float AttackSpeed { get { return attackSpeed; } }
    public float TimeToNextAttack { get { return timeToNextAttack; } set { timeToNextAttack = value; } }
    public float TimeBetweenAttacks { get { return timeBetweenAttacks; } }
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    public bool CanBeHit { get { return canBeHit; } set { canBeHit = value; } }
    public float ArmorPiercing { get { return armorPiercing; } }
    public float ArmorDestructionRate { get { return armorDestructionRate; } }
    public int PlayerDamage { get { return playerDamage; } }
    public float HeavyAttackMultiplier { get { return heavyAttackMultiplier; } }
    public float HeavyAttackTimeMultiplier { get { return heavyAttackTimeMultiplier; } }
    public float PlayerCriticalStrikeChance { get { return criticalStrikeChance; } }
    public float PlayerCriticalStrikeMultiplier { get { return criticalStrikeMultiplier; } }

    public int PlayerArmor { get { return playerArmor; } set { playerArmor = value; } }
    public float ArmorEffieciency { get { return armorEffieciency; } }
    public int PlayerHealth { get { return playerHealth; } set { playerHealth = value; } }
    public int PlayerMana { get { return playerMana; } set { playerMana = value; } }
    public int PlayerMaxHealth { get { return maxPlayerHealth; } set { maxPlayerHealth = value; } }
    public int PlayerMaxMana { get { return maxPlayerMana; } set { maxPlayerMana = value; } }

    public int PlayerAgility { get { return playerAgility; } set { playerAgility = value; } }
    public int PlayerStrength { get { return playerStrength; } set { playerStrength = value; } }
    public int PlayerIntellect { get { return playerIntellect; } set { playerIntellect = value; } }
    public int PlayerStamina { get { return playerStamina; } set { playerStamina = value; } }

    public int BonusAgility { get { return bonusPlayerAgility; } set { bonusPlayerAgility = value; } }
    public int BonusStrength { get { return bonusPlayerStrength; } set { bonusPlayerStrength = value; } }
    public int BonusStamina { get { return bonusPlayerStamina; } set { bonusPlayerStamina = value; } }
    public int BonusIntellect { get { return bonusPlayerIntellect; } set { bonusPlayerIntellect = value; } }
    public int BonusDamage { get { return bonusPlayerDamage; } set { bonusPlayerDamage = value; } }
    public float BonusAttackSpeed { get { return bonusAttackSpeed; } set { bonusAttackSpeed = value; } }

    #endregion

    public void CountStats()
    {
        Item[] equippedItems = { head, shoulders, chest, waist, legs, hands, feet, leftHand, rightHand};

        playerDamage = basePlayerDamage + bonusPlayerDamage;
        playerAgility = basePlayerAgility + bonusPlayerAgility;
        playerStamina = basePlayerStamina + bonusPlayerStamina;
        playerStrength = basePlayerStrength + bonusPlayerStrength;
        playerIntellect = basePlayerIntellect + bonusPlayerIntellect;
        playerArmor = basePlayerArmor;
        attackSpeed = baseAttackSpeed + bonusAttackSpeed;

        foreach (Item item in equippedItems)
        {
            if (item)
            {
                if (item.ItemInfo.Durability <= 0)
                    continue;

                if (item.ItemInfo.WeaponType != WeaponType.NONE && item.ItemInfo.Durability > 0f)
                {
                    if (attackSpeed != baseAttackSpeed)
                    {
                        attackSpeed += item.ItemInfo.AttackSpeed;
                        attackSpeed /= 2;
                    }
                    else
                    {
                        attackSpeed = item.ItemInfo.AttackSpeed;
                    }
                }

                playerAgility += item.ItemInfo.Agility;
                playerStamina += item.ItemInfo.Stamina;
                playerStrength += item.ItemInfo.Strength;
                playerIntellect += item.ItemInfo.Intellect;
                playerArmor += Mathf.RoundToInt(item.ItemInfo.Armor * item.ItemInfo.Durability / 100);
                playerDamage += item.ItemInfo.Damage;
            }
        }
        
        int[] currentStats = { playerAgility, playerStrength, playerStamina, playerIntellect, playerDamage, playerArmor};
        gameUI.UpdateStatsUI(currentStats, armorPiercing, armorEffieciency, attackSpeed);
    }

    public void ReduceItemsDurabilities()
    {
        Item[] equippedItems = { head, shoulders, chest, waist, legs, hands, feet};

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
        gameUI.UpdateEquippedArmorDurabilitiesUI(equippedItems);
    }

    public void ReduceWeaponsDurabilities(float destructionRate)
    {
        Item[] equippedItems = { leftHand, rightHand };

        foreach (Item item in equippedItems)
        {
            if (item)
            {
                if (item.ItemInfo.Durability > destructionRate)
                    item.ItemInfo.Durability -= item.ItemInfo.Durability * destructionRate;
                else
                    item.ItemInfo.Durability = 0;
            }
        }
        gameUI.UpdateEquippedWeaponsDurabilitiesUI(equippedItems[0], equippedItems[1]);
    }

    public void TakeDamage(float dmg)
    {
        if (!canBeHit)
            return;

        int healthToTake = Mathf.RoundToInt(dmg - PlayerArmor * ArmorEffieciency);
        if (healthToTake < 0)
            healthToTake = 0;

        if (playerHealth <= healthToTake)
        {
            PlayerHealth -= healthToTake;
            gameUI.UpdatePlayerHealthUI(0);
            gameUI.FloatingText(true, healthToTake);
            PlayerDie();
        }
        else
        {
            PlayerHealth -= healthToTake;
            gameUI.FloatingText(true, healthToTake);
            PlayerDamaged();
        }
    }

    public void PlayerDamaged()
    {
        PlayerAttacked?.Invoke();
    }

    public void PlayerDie()
    {
        PlayerDeath?.Invoke();
    }

    public void ToggleColliderAOE(bool value)
    {
        colliderAOE.enabled = value;
    }
}
