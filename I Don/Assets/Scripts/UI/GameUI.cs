using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    [Space]
    [SerializeField] Slider playerHP;
    [SerializeField] Slider playerMP;

    [SerializeField] Slider playerAttackSlider;

    [Header("Player Stats")]
    // order: Agi-Str-Stam-Int-Dmg-Pierc-Arm
    [Space]
    [SerializeField] Text[] stats = new Text[7];
    [SerializeField] Text armorEffieciency;
    [SerializeField] Text attackSpeed;

    [Header("Player Equipment")]
    // order: Head-Shoulders-Chest-Waist-Legs-Hands-Feet-LHand-RHand
    [Space]
    [SerializeField] Image consumable;
    [SerializeField] Text consumableUses;

    [Space]
    [SerializeField] Image[] equipment = new Image[9];
    [SerializeField] Text[] equipmentDurabilities = new Text[9];

    [Space]
    [SerializeField] Sprite baseEqImage;
    [SerializeField] Sprite baseConsumableImage;

    [Space]
    [Header("Player Floating Text")]
    [SerializeField] GameObject LeftPlayerFloatingText;
    [SerializeField] GameObject RightPlayerFloatingText;
    [SerializeField] Transform LeftFloatingTextParent;
    [SerializeField] Transform RightFloatingTextParent;

    [Space]
    [Header("Player Died UI")]
    [SerializeField] GameObject PlayerDeathUI;


    [Space]
    [Header("First buttons selected in a menu")]
    [SerializeField] GameObject deathRestartButton;

    private void Start()
    {
        FindObjectOfType<Player>().PlayerDeath.AddListener(ManagePlayerDeathUI);
    }

    public void UpdateStatsUI(int[] playerStats, float armorEff, float attSpeed)
    {
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].text = playerStats[i].ToString();
        }
        armorEffieciency.text = armorEff.ToString();
        attackSpeed.text = attSpeed.ToString();
    }
    public void UpdateStatsUI(int[] playerStats)
    {
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].text = playerStats[i].ToString();
        }
    }

    public void UpdatePlayerHealthUI(int playerHealth)
    {
        playerHP.value = playerHealth;
    }

    public void UpdatePlayerManaUI(int playerMana)
    {
        playerMP.value = playerMana;
    }

    public void UpdateEqUI(Item[] eq)
    {
        int i = 0;
        foreach(Image item in equipment)
        {
            if (eq[i])
            {
                item.sprite = eq[i].itemInfo.getImage;
            }
            else
            {
                item.sprite = baseEqImage;
            }
            i++;
        }
    }
    public void UpdateConsumableUI(Consumable cons)
    {
        if (cons)
        {
            consumable.sprite = cons.itemInfo.getImage();
            consumableUses.text = cons.itemInfo.Uses.ToString();
        }
        else
        {
            consumable.sprite = baseConsumableImage;
            consumableUses.text = "";
        }
    }
    public void UpdateConsumableUsesUI (int value)
    {
        if (value > 0)
        {
            consumableUses.text = value.ToString();
        }
        else
        {
            consumableUses.text = "";
        }
    }

    public void FloatingText(bool isDamage, int value)
    {
        if (isDamage)
        {
            GameObject obj = Instantiate(LeftPlayerFloatingText);
            obj.transform.SetParent(LeftFloatingTextParent);

            obj.GetComponent<Text>().text = $"-{value}";
            StartCoroutine(WaitAndDestroy(1f, obj));
        }
        else
        {
            GameObject obj = Instantiate(RightPlayerFloatingText);
            obj.transform.SetParent(RightFloatingTextParent);

            obj.GetComponent<Text>().text = $"+{value}";
            StartCoroutine(WaitAndDestroy(1f, obj));
        }
    }

    IEnumerator WaitAndDestroy(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }

    public void UpdateEquippedItemsDurabilitiesUI(Item[] eq)
    {
        int i = 0;
        foreach (Text item in equipmentDurabilities)
        {
            if (eq[i])
            {
                item.text = $"{eq[i].itemInfo.Durability}%";
            }
            else
            {
                item.text = "";
            }
            i++;
        }
    }
    public void UpdateEquippedWeaponsDurabilitiesUI(Item lWeap, Item rWeap)
    {
        if (lWeap)
            equipmentDurabilities[equipmentDurabilities.Length - 2].text = $"{lWeap.itemInfo.Durability}%";
        if (rWeap)
            equipmentDurabilities[equipmentDurabilities.Length - 1].text = $"{rWeap.itemInfo.Durability}%";
    }
    public void UpdateEquippedArmorDurabilitiesUI(Item[] eq)
    {
        for (int i = 0; i < eq.Length; i++)
        {
            if (eq[i])
            {
                equipmentDurabilities[i].text = $"{eq[i].itemInfo.Durability}%";
            }
            else
            {
                equipmentDurabilities[i].text = "";
            }
        }
    }

    public void UpdatePlayerAttackTimer(float current, float max)
    {
        playerAttackSlider.maxValue = max;
        playerAttackSlider.value = current;
    }
    public void ManagePlayerAttackTimer(float current)
    {
        playerAttackSlider.value = current;
    }

    public void ManagePlayerDeathUI()
    {
        TogglePlayerDeathUI(true);
    }

    public void TogglePlayerDeathUI(bool active)
    {
        //Debug.Log("[GameUI] Toggle DeathUI");
        PlayerDeathUI.SetActive(active); 

        if (PlayerDeathUI.activeInHierarchy)
        {
            //Debug.Log("[MenuManager] Set first selected button in deathUI");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(deathRestartButton);
        }
    }
}
