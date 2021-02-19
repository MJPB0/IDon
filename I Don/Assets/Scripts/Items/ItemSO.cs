using UnityEngine;

public enum Type { WEAPON, ARMOR, ITEM }
public enum Slot { HEAD, SHOULDERS, CHEST, HANDS, WAIST, LEGS, FEET, LEFTHAND, RIGHTHAND, NONE}
public enum WeaponType { ONEHANDED, TWOHANDED, NONE }
public enum ArmorType { CLOTH, PLATE, LEATHER, SHIELD, NONE }
public enum Weapon { DAGGER, STAFF, SWORD, AXE, NONE }

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [SerializeField] bool pickable;

    [Header("Equipable Info")]
    [SerializeField] Type itemType;
    [SerializeField] Slot slot;

    [Header("Armor Info")]
    [Space]
    [SerializeField] ArmorType armorType;
    [SerializeField] int armor;

    [Header("Weapon Info")]
    [Space]
    [SerializeField] WeaponType weaponType;
    [SerializeField] Weapon weapon;
    [SerializeField] int damage;
    [SerializeField] float attackSpeed;

    [Header("Equipable basic Info")]
    [Space]
    [Range(0, 100)]
    [SerializeField] float durability;
    [Range(0, 100)]
    [SerializeField] int startingDurability;

    [Header("Basic stats")]
    [Space]
    [SerializeField] int agility;
    [SerializeField] int strength;
    [SerializeField] int stamina;
    [SerializeField] int intellect;

    [Header("UI Sprite")]
    [Space]
    [SerializeField] Sprite img;

    public bool isPickable { get { return pickable; } set { pickable = value; } }

    public Type getType { get { return itemType; } }
    public Slot getSlot { get { return slot; } }

    public float Durability { get { return durability; } set { durability = value; } }
    public int getStartDurability { get { return startingDurability; } }

    public int getAgility { get { return agility; } }
    public int getStrength { get { return strength; } }
    public int getStamina { get { return stamina; } }
    public int getIntellect { get { return intellect; } }

    public Sprite getImage { get { return img; } }

    public WeaponType getWeaponType { get { return weaponType; } }
    public Weapon getWeapon { get { return weapon; } }
    public int getDamage { get { return damage; } }
    public float getAttackSpeed { get { return attackSpeed; } }

    public ArmorType getArmorType { get { return armorType; } }
    public int getArmor { get { return armor; } }
}
