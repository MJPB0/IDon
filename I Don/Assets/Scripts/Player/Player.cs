using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent PlayerDeath;

    [SerializeField] GameUI gameUI;

    [Space]
    [Header("Movement")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotateSpeed = .5f;

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
    [SerializeField] Consumable consumable;

    [Space]
    [Header("Stats")]
    [SerializeField] int maxPlayerHealth = 100;
    [SerializeField] int playerHealth = 100;
    [SerializeField] int maxPlayerMana = 100;
    [SerializeField] int playerMana = 100;

    [Space]
    [SerializeField] int playerDamage;
    [SerializeField] int playerArmor;
    [Range(0, 1)]
    [SerializeField] float armorEffieciency;

    [Space]
    [SerializeField] int playerAgility;
    [SerializeField] int playerStrength;
    [SerializeField] int playerStamina;
    [SerializeField] int playerIntellect;

    [Header("Player Base Stats")]
    [SerializeField] int basePlayerDamage;
    [SerializeField] float baseHeavyAttackMultiplier;
    [SerializeField] float baseAttackSpeed;
    [SerializeField] int basePlayerArmor;

    [Space]
    [SerializeField] int basePlayerAgility;
    [SerializeField] int basePlayerStrength;
    [SerializeField] int basePlayerStamina;
    [SerializeField] int basePlayerIntellect;

    [Header("Player Bonus Stats")]
    [SerializeField] int bonusPlayerDamage;
    [SerializeField] float bonusAttackSpeed;

    [Space]
    [SerializeField] int bonusPlayerAgility;
    [SerializeField] int bonusPlayerStrength;
    [SerializeField] int bonusPlayerStamina;
    [SerializeField] int bonusPlayerIntellect;

    [Header("Sprinting")]
    [Space]
    [SerializeField] bool isSprinting;
    [SerializeField] float sprintSpeed = 4f;
    [SerializeField] float sprintDuration = 100f;
    [Range(0, 100)]
    [SerializeField] float exhaustion = 0f;
    [SerializeField] float sprintDepletionPerSecond = 2f;
    [SerializeField] float restRate = 1.5f;

    [Header("Attacking")]
    [Space]
    [Range(0, 1)]
    [SerializeField] float criticalStrikeChance;
    [SerializeField] float criticalStrikeMultiplier;
    [SerializeField] float heavyAttackMultiplier;
    [SerializeField] float attackSpeed;
    [SerializeField] float timeToNextAttack;
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] bool canAttack;
    [SerializeField] bool canBeHit = true;
    [SerializeField] int armorPiercing;
    [SerializeField] Collider colliderAOE;
    [Range(0, 1)]
    [SerializeField] float dizzinessChance;

    public float getMoveSpeed() { return moveSpeed; }
    public float getRotateSpeed() { return rotateSpeed; }

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

    public float getSprintSpeed() { return sprintSpeed; }
    public float SprintDuration { get { return sprintDuration; } set { sprintDuration = value; } }
    public bool IsSprinting { get { return isSprinting; } set { isSprinting = value; } }
    public float getSprintDepletionPerSecond() { return sprintDepletionPerSecond; }
    public float Exhaustion { get { return exhaustion; } set { exhaustion = value; } }
    public float getRestRate() { return restRate; }

    public float getAttackSpeed() { return attackSpeed; }
    public float TimeToNextAttack { get { return timeToNextAttack; } set { timeToNextAttack = value; } }
    public float getTimeBetweenAttacks() { return timeBetweenAttacks; }
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    public bool CanBeHit { get { return canBeHit; } set { canBeHit = value; } }
    public int getArmorPiercing() { return armorPiercing; }
    public int getPlayerDamage() { return playerDamage; }
    public float getHeavyAttackMultiplier() { return heavyAttackMultiplier; }
    public float PlayerCriticalStrikeChance { get { return criticalStrikeChance; } }
    public float PlayerCriticalStrikeMultiplier { get { return criticalStrikeMultiplier; } }

    public int PlayerArmor { get { return playerArmor; } set { playerArmor = value; } }
    public float getArmorEffieciency { get { return armorEffieciency; } }
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
                if (item.itemInfo.Durability <= 0)
                    continue;

                if (item.itemInfo.getWeaponType != WeaponType.NONE && item.itemInfo.Durability > 0f)
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

                playerAgility += item.itemInfo.getAgility;
                playerStamina += item.itemInfo.getStamina;
                playerStrength += item.itemInfo.getStrength;
                playerIntellect += item.itemInfo.getIntellect;
                playerArmor += Mathf.RoundToInt(item.itemInfo.getArmor * item.itemInfo.Durability / 100);
                playerDamage += item.itemInfo.getDamage;
            }
        }
        
        int[] currentStats = { playerAgility, playerStrength, playerStamina, playerIntellect, playerDamage, armorPiercing, playerArmor};
        gameUI.UpdateStatsUI(currentStats, armorEffieciency, attackSpeed);
    }

    public void ReduceItemsDurabilities(float value)
    {
        Item[] equippedItems = { head, shoulders, chest, waist, legs, hands, feet};

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
        gameUI.UpdateEquippedArmorDurabilitiesUI(equippedItems);
    }

    public void ReduceWeaponsDurabilities(float value)
    {
        Item[] equippedItems = { leftHand, rightHand };

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
        gameUI.UpdateEquippedWeaponsDurabilitiesUI(equippedItems[0], equippedItems[1]);
    }

    public void TakeDamage(int value, Enemy target)
    {
        if (!canBeHit)
            return;
        if (playerHealth <= value)
        {
            PlayerHealth -= value;
            gameUI.UpdatePlayerHealthUI(0);
            gameUI.FloatingText(true, value);
            PlayerDie();
        }
        else
        {
            if (!FindObjectOfType<PlayerAttack>().getEnemiesInRange().Contains(target.gameObject) && !gameObject.GetComponent<PlayerController>().IsDizzy)
            {
                float tmp = Random.Range(0, 1);
                if (tmp > dizzinessChance)
                {
                    gameObject.GetComponent<PlayerController>().IsDizzy = true;
                    gameObject.GetComponent<PlayerController>().PlayerDizziness();
                }
            }
            PlayerHealth -= value;
            ReduceItemsDurabilities(target.getArmorPiercing);
            CountStats();
            gameUI.UpdatePlayerHealthUI(PlayerHealth);
            gameUI.FloatingText(true, value);
            //Debug.Log("Reduced all items durability by " + target.getArmorPiercing + "%");
            //Debug.Log("Player suffered " + value + "dmg");
        }
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
