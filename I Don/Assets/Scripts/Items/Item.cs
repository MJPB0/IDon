using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Item stuff")]
    [SerializeField] ItemSO item;
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
    [SerializeField] Text[] compareItemStatsArmor;
    [SerializeField] Text[] compareItemStatsWeapon;

    [Header("Comparing Shield to a weapon")]
    [SerializeField] GameObject compareFullUI;
    [SerializeField] Text[] compareItemStatsFull;

    public ItemSO itemInfo { get { return item; } }

    private void Start()
    {
        item.isPickable = true;
        item.Durability = item.getStartDurability;
        UpdateInfoUI();
    }

    public void UpdateInfoUI()
    {
        ToggleCompareInfoUI(false);
        if (compareFullUI)
            ToggleCompareFullUI(false);
        string name = item.name;
        string stats = "";
        if (item.getType == Type.ARMOR)
        {
            stats = $"" +
                $"Type: {item.getArmorType}\n" +
                $"Slot: {item.getSlot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Armor: {item.getArmor} \n" +
                $"Agility: {item.getAgility} \n" +
                $"Strength: {item.getStrength} \n" +
                $"Stamina: {item.getStamina} \n" +
                $"Intellect: {item.getIntellect}";
        }else if (item.getType == Type.WEAPON)
        {
            stats = $"" +
                $"Type: {item.getWeaponType}\n" +
                $"Slot: {item.getSlot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Damage: {item.getDamage} \n" +
                $"Attack Speed: {item.getAttackSpeed} \n" +
                $"Agility: {item.getAgility} \n" +
                $"Strength: {item.getStrength} \n" +
                $"Stamina: {item.getStamina} \n" +
                $"Intellect: {item.getIntellect}";
        }
        itemName.text = name;
        itemStats.text = stats;
    }
    public void UpdateInfoUI(ItemSO comparedItem)
    {
        ToggleCompareInfoUI(false);
        if (compareFullUI)
            ToggleCompareFullUI(false);

        string name = item.name;
        string stats = "";
        if ((item.getType == Type.WEAPON && comparedItem.getArmorType == ArmorType.SHIELD) || (item.getArmorType == ArmorType.SHIELD && comparedItem.getType == Type.WEAPON))
        {
            string[] compare = FullCompare(item, comparedItem);
            stats = $"" +
                $"Type: {item.getWeaponType}\n" +
                $"Slot: {item.getSlot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Armor: {item.getArmor} \n" +
                $"Damage: {item.getDamage} \n" +
                $"Attack Speed: {item.getAttackSpeed} \n" +
                $"Agility: {item.getAgility} \n" +
                $"Strength: {item.getStrength} \n" +
                $"Stamina: {item.getStamina} \n" +
                $"Intellect: {item.getIntellect}";

            compareItemStatsFull[0].text = compare[0];
            compareItemStatsFull[1].text = compare[1];
            compareItemStatsFull[2].text = compare[2];
            compareItemStatsFull[3].text = compare[3];
            compareItemStatsFull[4].text = compare[4];
            compareItemStatsFull[5].text = compare[5];
            compareItemStatsFull[6].text = compare[6];
            compareItemStatsFull[7].text = compare[7];

            foreach (Text t in compareItemStatsFull)
            {
                if (t.text[1] == '0' && t.text.Length == 2)
                {
                    t.color = Color.blue;
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
        }
        else if (item.getType == Type.ARMOR)
        {
            string[] compare = CompareItems(item, comparedItem);
            stats = $"" +
                $"Type: {item.getArmorType}\n" +
                $"Slot: {item.getSlot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Armor: {item.getArmor} \n" +
                $"Agility: {item.getAgility} \n" +
                $"Strength: {item.getStrength} \n" +
                $"Stamina: {item.getStamina} \n" +
                $"Intellect: {item.getIntellect}";

            compareItemStatsArmor[0].text = compare[0];
            compareItemStatsArmor[1].text = compare[1];
            compareItemStatsArmor[2].text = compare[4];
            compareItemStatsArmor[3].text = compare[5];
            compareItemStatsArmor[4].text = compare[6];
            compareItemStatsArmor[5].text = compare[7];

            foreach (Text t in compareItemStatsArmor)
            {
                if (t.text[1] == '0' && t.text.Length == 2)
                {
                    t.color = Color.blue;
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
        }
        else if (item.getType == Type.WEAPON)
        {
            string[] compare = CompareItems(item, comparedItem);
            stats = $"" +
                $"Type: {item.getWeaponType}\n" +
                $"Slot: {item.getSlot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Damage: {item.getDamage} \n" +
                $"Attack Speed: {item.getAttackSpeed} \n" +
                $"Agility: {item.getAgility} \n" +
                $"Strength: {item.getStrength} \n" +
                $"Stamina: {item.getStamina} \n" +
                $"Intellect: {item.getIntellect}";

            compareItemStatsWeapon[0].text = compare[0];
            compareItemStatsWeapon[1].text = compare[2];
            compareItemStatsWeapon[2].text = compare[3];
            compareItemStatsWeapon[3].text = compare[4];
            compareItemStatsWeapon[4].text = compare[5];
            compareItemStatsWeapon[5].text = compare[6];
            compareItemStatsWeapon[6].text = compare[7];

            foreach (Text t in compareItemStatsWeapon)
            {
                if (t.text == "")
                    continue;
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
        }
        itemName.text = name;
        itemStats.text = stats;
    }
    public void UpdateInfoUI(Player player)
    {
        ToggleCompareInfoUI(false);
        if (compareFullUI)
            ToggleCompareFullUI(false);

        string name = item.name;

        string[] compare = CompareThree(item, player);
        string stats = $"" +
            $"Type: {item.getWeaponType}\n" +
            $"Slot: {item.getSlot} \n" +
            $"Durability: {item.Durability} \n" +
            $"Armor: {item.getArmor} \n" +
            $"Damage: {item.getDamage} \n" +
            $"Attack Speed: {item.getAttackSpeed} \n" +
            $"Agility: {item.getAgility} \n" +
            $"Strength: {item.getStrength} \n" +
            $"Stamina: {item.getStamina} \n" +
            $"Intellect: {item.getIntellect}";

        compareItemStatsFull[0].text = compare[0];
        compareItemStatsFull[1].text = compare[1];
        compareItemStatsFull[2].text = compare[2];
        compareItemStatsFull[3].text = compare[3];
        compareItemStatsFull[4].text = compare[4];
        compareItemStatsFull[5].text = compare[5];
        compareItemStatsFull[6].text = compare[6];
        compareItemStatsFull[7].text = compare[7];

        foreach (Text t in compareItemStatsFull)
        {
            if (t.text[0] != '+' && t.text[0] != '-')
                t.color = Color.black;
            else if (t.text[1] == '0' && t.text.Length == 2)
                t.color = Color.blue;
            else if (t.text[0] == '+')
                t.color = Color.green;
            else if (t.text[0] == '-')
                t.color = Color.red;
        }

        itemName.text = name;
        itemStats.text = stats;
    }
    private string[] CompareItems(ItemSO item1, ItemSO item2)
    {
        ToggleCompareInfoUI(true);
        // ORDER: Durability-Armor-Damage-AttackSpeed-Agi-Str-Stam-Int
        string[] r = new string[8];
        if (item1.getType == Type.ARMOR && item2.getType == Type.ARMOR)
        {
            r[0] = $"{item1.Durability - item2.Durability}";
            r[1] = $"{item1.getArmor - item2.getArmor}";
            r[2] = "";
            r[3] = "";
            r[4] = $"{item1.getAgility - item2.getAgility}";
            r[5] = $"{item1.getStrength - item2.getStrength}";
            r[6] = $"{item1.getStamina - item2.getStamina}";
            r[7] = $"{item1.getIntellect - item2.getIntellect}";

            if (r[0][0] != '-')
                r[0] = $"+{r[0]}";
            if (r[1][0] != '-')
                r[1] = $"+{r[1]}";
            if ( r[4][0] != '-')
                r[4] = $"+{r[4]}";
            if (r[5][0] != '-')
                r[5] = $"+{r[5]}";
            if (r[6][0] != '-')
                r[6] = $"+{r[6]}";
            if (r[7][0] != '-')
                r[7] = $"+{r[7]}";
        }
        else if (item1.getType == Type.WEAPON && item2.getType == Type.WEAPON)
        {
            r[0] = $"{item1.Durability - item2.Durability}";
            r[1] = "";
            r[2] = $"{item1.getDamage - item2.getDamage}";
            r[3] = $"{item1.getAttackSpeed - item2.getAttackSpeed}";
            r[4] = $"{item1.getAgility - item2.getAgility}";
            r[5] = $"{item1.getStrength - item2.getStrength}";
            r[6] = $"{item1.getStamina - item2.getStamina}";
            r[7] = $"{item1.getIntellect - item2.getIntellect}";

            if (r[0][0] != '-')
                r[0] = $"+{r[0]}";
            if (r[2][0] != '-')
                r[2] = $"+{r[2]}";
            if (r[3][0] != '-')
                r[3] = $"+{r[3]}";
            if (r[4][0] != '-')
                r[4] = $"+{r[4]}";
            if (r[5][0] != '-')
                r[5] = $"+{r[5]}";
            if (r[6][0] != '-')
                r[6] = $"+{r[6]}";
            if (r[7][0] != '-')
                r[7] = $"+{r[7]}";
        }
        return r;
    }
    private string[] FullCompare(ItemSO item1, ItemSO item2)
    {
        ToggleCompareFullUI(true);
        string[] r = new string[8];
        r[0] = $"{item1.Durability - item2.Durability}";
        r[1] = $"{item1.getArmor - item2.getArmor}";
        r[2] = $"{item1.getDamage - item2.getDamage}";
        r[3] = $"{item1.getAttackSpeed - item2.getAttackSpeed}";
        r[4] = $"{item1.getAgility - item2.getAgility}";
        r[5] = $"{item1.getStrength - item2.getStrength}";
        r[6] = $"{item1.getStamina - item2.getStamina}";
        r[7] = $"{item1.getIntellect - item2.getIntellect}";

        if (r[0][0] != '-')
            r[0] = $"+{r[0]}";
        if (r[1][0] != '-')
            r[1] = $"+{r[1]}";
        if (r[2][0] != '-')
            r[2] = $"+{r[2]}";
        if (r[3][0] != '-')
            r[3] = $"+{r[3]}";
        if (r[4][0] != '-')
            r[4] = $"+{r[4]}";
        if (r[5][0] != '-')
            r[5] = $"+{r[5]}";
        if (r[6][0] != '-')
            r[6] = $"+{r[6]}";
        if (r[7][0] != '-')
            r[7] = $"+{r[7]}";
        return r;
    }
    private string[] CompareThree(ItemSO item1, Player player)
    {
        ToggleCompareFullUI(true);
        ItemSO iteml = player.LeftHand.item;
        ItemSO itemr = player.RightHand.item;

        string[] r = new string[8];
        r[0] = $"{iteml.Durability}/{itemr.Durability}";
        r[1] = $"{item1.getArmor - (iteml.getArmor + itemr.getArmor)}";
        r[2] = $"{item1.getDamage - (iteml.getDamage + itemr.getDamage)}";
        r[3] = $"{item1.getAttackSpeed - (iteml.getAttackSpeed + itemr.getAttackSpeed)}";
        r[4] = $"{item1.getAgility - (iteml.getAgility + itemr.getAgility)}";
        r[5] = $"{item1.getStrength - (iteml.getStrength + itemr.getStrength)}";
        r[6] = $"{item1.getStamina - (iteml.getStamina + itemr.getStamina)}";
        r[7] = $"{item1.getIntellect - (iteml.getIntellect + itemr.getIntellect)}";

        if (r[1][0] != '-')
            r[1] = $"+{r[1]}";
        if (r[2][0] != '-')
            r[2] = $"+{r[2]}";
        if (r[3][0] != '-')
            r[3] = $"+{r[3]}";
        if (r[4][0] != '-')
            r[4] = $"+{r[4]}";
        if (r[5][0] != '-')
            r[5] = $"+{r[5]}";
        if (r[6][0] != '-')
            r[6] = $"+{r[6]}";
        if (r[7][0] != '-')
            r[7] = $"+{r[7]}";
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
    public void ToggleCompareFullUI(bool active)
    {
        compareFullUI.SetActive(active);
    }
}
