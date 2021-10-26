using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Item stuff")]
    [SerializeField] private ItemSO item;
    [SerializeField] private GameObject ingameModel;
    [SerializeField] private Sprite spriteUI;

    [Header("Item Info UI")]
    [Space]
    [SerializeField] private GameObject InfoUI;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemStats;
    [SerializeField] private float canvasHeight;

    [Header("Compare items UI")]
    [Space]
    [SerializeField] private GameObject compareItemsUI;
    [SerializeField] private Text[] compareItemStatsArmor;
    [SerializeField] private Text[] compareItemStatsWeapon;

    [Header("Comparing Shield to a weapon")]
    [SerializeField] private GameObject compareFullUI;
    [SerializeField] private Text[] compareItemStatsFull;

    public ItemSO ItemInfo { get { return item; } }

    private void Start()
    {
        item.IsPickable = true;
        item.Durability = item.StartDurability;
        UpdateInfoUI();
    }

    private void Update()
    {
        InfoUI.transform.position = new Vector3(transform.position.x, transform.position.y + canvasHeight, transform.position.z);
    }

    public void UpdateInfoUI()
    {
        ToggleCompareInfoUI(false);
        if (compareFullUI)
            ToggleCompareFullUI(false);
        string name = item.name;
        string stats = "";
        if (item.Type == Type.ARMOR)
        {
            stats = $"" +
                $"Type: {item.ArmorType}\n" +
                $"Slot: {item.Slot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Armor: {item.Armor} \n" +
                $"Agility: {item.Agility} \n" +
                $"Strength: {item.Strength} \n" +
                $"Stamina: {item.Stamina} \n" +
                $"Intellect: {item.Intellect}";
        }else if (item.Type == Type.WEAPON)
        {
            stats = $"" +
                $"Type: {item.WeaponType}\n" +
                $"Slot: {item.Slot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Damage: {item.Damage} \n" +
                $"Attack Speed: {item.AttackSpeed} \n" +
                $"Agility: {item.Agility} \n" +
                $"Strength: {item.Strength} \n" +
                $"Stamina: {item.Stamina} \n" +
                $"Intellect: {item.Intellect}";
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
        if ((item.Type == Type.WEAPON && comparedItem.ArmorType == ArmorType.SHIELD) || (item.ArmorType == ArmorType.SHIELD && comparedItem.Type == Type.WEAPON))
        {
            string[] compare = FullCompare(item, comparedItem);
            stats = $"" +
                $"Type: {item.WeaponType}\n" +
                $"Slot: {item.Slot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Armor: {item.Armor} \n" +
                $"Damage: {item.Damage} \n" +
                $"Attack Speed: {item.AttackSpeed} \n" +
                $"Agility: {item.Agility} \n" +
                $"Strength: {item.Strength} \n" +
                $"Stamina: {item.Stamina} \n" +
                $"Intellect: {item.Intellect}";

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
        else if (item.Type == Type.ARMOR)
        {
            string[] compare = CompareItems(item, comparedItem);
            stats = $"" +
                $"Type: {item.ArmorType}\n" +
                $"Slot: {item.Slot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Armor: {item.Armor} \n" +
                $"Agility: {item.Agility} \n" +
                $"Strength: {item.Strength} \n" +
                $"Stamina: {item.Stamina} \n" +
                $"Intellect: {item.Intellect}";

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
        else if (item.Type == Type.WEAPON)
        {
            string[] compare = CompareItems(item, comparedItem);
            stats = $"" +
                $"Type: {item.WeaponType}\n" +
                $"Slot: {item.Slot} \n" +
                $"Durability: {item.Durability} \n" +
                $"Damage: {item.Damage} \n" +
                $"Attack Speed: {item.AttackSpeed} \n" +
                $"Agility: {item.Agility} \n" +
                $"Strength: {item.Strength} \n" +
                $"Stamina: {item.Stamina} \n" +
                $"Intellect: {item.Intellect}";

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
            $"Type: {item.WeaponType}\n" +
            $"Slot: {item.Slot} \n" +
            $"Durability: {item.Durability} \n" +
            $"Armor: {item.Armor} \n" +
            $"Damage: {item.Damage} \n" +
            $"Attack Speed: {item.AttackSpeed} \n" +
            $"Agility: {item.Agility} \n" +
            $"Strength: {item.Strength} \n" +
            $"Stamina: {item.Stamina} \n" +
            $"Intellect: {item.Intellect}";

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
        if (item1.Type == Type.ARMOR && item2.Type == Type.ARMOR)
        {
            r[0] = $"{item1.Durability - item2.Durability}";
            r[1] = $"{item1.Armor - item2.Armor}";
            r[2] = "";
            r[3] = "";
            r[4] = $"{item1.Agility - item2.Agility}";
            r[5] = $"{item1.Strength - item2.Strength}";
            r[6] = $"{item1.Stamina - item2.Stamina}";
            r[7] = $"{item1.Intellect - item2.Intellect}";

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
        else if (item1.Type == Type.WEAPON && item2.Type == Type.WEAPON)
        {
            r[0] = $"{item1.Durability - item2.Durability}";
            r[1] = "";
            r[2] = $"{item1.Damage - item2.Damage}";
            r[3] = $"{item1.AttackSpeed - item2.AttackSpeed}";
            r[4] = $"{item1.Agility - item2.Agility}";
            r[5] = $"{item1.Strength - item2.Strength}";
            r[6] = $"{item1.Stamina - item2.Stamina}";
            r[7] = $"{item1.Intellect - item2.Intellect}";

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
        r[1] = $"{item1.Armor - item2.Armor}";
        r[2] = $"{item1.Damage - item2.Damage}";
        r[3] = $"{item1.AttackSpeed - item2.AttackSpeed}";
        r[4] = $"{item1.Agility - item2.Agility}";
        r[5] = $"{item1.Strength - item2.Strength}";
        r[6] = $"{item1.Stamina - item2.Stamina}";
        r[7] = $"{item1.Intellect - item2.Intellect}";

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
        r[1] = $"{item1.Armor - (iteml.Armor + itemr.Armor)}";
        r[2] = $"{item1.Damage - (iteml.Damage + itemr.Damage)}";
        r[3] = $"{item1.AttackSpeed - (iteml.AttackSpeed + itemr.AttackSpeed)}";
        r[4] = $"{item1.Agility - (iteml.Agility + itemr.Agility)}";
        r[5] = $"{item1.Strength - (iteml.Strength + itemr.Strength)}";
        r[6] = $"{item1.Stamina - (iteml.Stamina + itemr.Stamina)}";
        r[7] = $"{item1.Intellect - (iteml.Intellect + itemr.Intellect)}";

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
