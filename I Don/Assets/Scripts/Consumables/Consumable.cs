using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Consumable : MonoBehaviour
{
    [SerializeField] ConsumableSO item;
    [SerializeField] GameObject ingameModel;
    [SerializeField] Sprite spriteUI;

    [Header("Item Info UI")]
    [Space]
    [SerializeField] GameObject InfoUI;
    [SerializeField] Text itemName;
    [SerializeField] Text itemStats;


    [Header("Compare items UI")]
    [Space]
    [SerializeField] GameObject compareItemsUI;
    [SerializeField] Text[] compareItem;

    public ConsumableSO itemInfo { get { return item; } }

    private void Start()
    {
        item.Uses = item.StartUses;
        item.IsPickable = true;
        UpdateInfoUI();
    }

    public void UpdateInfoUI()
    {
        ToggleCompareInfoUI(false);
        string name = item.name;
        string stats = $"" +
            $"Effectiveness: {item.Effectiveness}";

        itemName.text = name;
        itemStats.text = stats;
    }
    public void UpdateInfoUI(ConsumableSO cons)
    {
        ToggleCompareInfoUI(true);
        string name = item.name;
        string stats = $"" +
            $"Effectiveness: {item.Effectiveness}";

        string[] compare = CompareItems(item, cons);
        compareItem[0].text = compare[0];

        foreach (Text t in compareItem)
        {
            if (t.text.Length == 2)
            {
                if (t.text[1] == '0')
                {
                    t.color = Color.blue;
                }
            }
            else if (t.text[0] == '+')
            {
                t.color = Color.green;
            }
            else if (t.text[0] == '-')
            {
                t.color = Color.red;
            }
        }

        itemName.text = name;
        itemStats.text = stats;
    }

    private string[] CompareItems(ConsumableSO item1, ConsumableSO item2)
    {
        ToggleCompareInfoUI(true);
        // ORDER: Durability-Armor-Damage-AttackSpeed-Agi-Str-Stam-Int
        string[] r = new string[1];
        r[0] = $"{item1.Effectiveness - item2.Effectiveness}";

        if (r[0][0] != '-')
            r[0] = $"+{r[0]}";

        return r;
    }
    public void ToggleInfoUI(bool active)
    {
        InfoUI.SetActive(active);
    }
    public void ToggleCompareInfoUI(bool active)
    {
        compareItemsUI.SetActive(active);
    }

    public void UsePotion()
    {
        Player player = FindObjectOfType<Player>();
        GameUI gameUI = FindObjectOfType<GameUI>();

        switch (item.getType())
        {
            case PotionType.HEALTH:
                player.PlayerHealth += item.Effectiveness;
                gameUI.UpdatePlayerHealthUI(player.PlayerHealth);
                if (player.PlayerHealth > player.PlayerMaxHealth)
                    player.PlayerHealth = player.PlayerMaxHealth;
                break;
            case PotionType.MANA:
                player.PlayerMana += item.Effectiveness;
                gameUI.UpdatePlayerManaUI(player.PlayerMana);
                if (player.PlayerMana > player.PlayerMaxMana)
                    player.PlayerMana = player.PlayerMaxMana;
                break;
            case PotionType.STATS:
                switch (item.getStatPotionType())
                {
                    case StatPotionType.AGI:
                        player.BonusAgility += item.Effectiveness;
                        break;
                    case StatPotionType.STR:
                        player.BonusStrength += item.Effectiveness;
                        break;
                    case StatPotionType.STAM:
                        player.BonusStamina += item.Effectiveness;
                        break;
                    case StatPotionType.INT:
                        player.BonusIntellect += item.Effectiveness;
                        break;
                }
                player.CountStats();
                int[] currentStats = { player.PlayerAgility,
                    player.PlayerStrength,
                    player.PlayerStamina,
                    player.PlayerIntellect,
                    player.getPlayerDamage(),
                    player.getArmorPiercing(),
                    player.PlayerArmor };
                gameUI.UpdateStatsUI(currentStats);
                break;
        }
        player.PlayerConsumable.itemInfo.Uses -= 1;
        gameUI.UpdateConsumableUsesUI(player.PlayerConsumable.itemInfo.Uses);

        if (item.IsTemporary)
        {
            PlayerController controller = FindObjectOfType<PlayerController>();
            controller.CancelPotionEffectCountdown(this);
        }

        if (player.PlayerConsumable.itemInfo.Uses <= 0)
        {
            GameObject old = player.PlayerConsumable.gameObject;
            player.PlayerConsumable = null;
            gameUI.UpdateConsumableUI(null);
            Destroy(old);
        }
    }
}
