using UnityEngine;

public enum Type { WEAPON, ARMOR, ITEM }
public enum Slot { HEAD, SHOULDERS, CHEST, HANDS, WAIST, LEGS, FEET, LEFTHAND, RIGHTHAND, NONE}
public enum WeaponType { ONEHANDED, TWOHANDED, NONE }
public enum ArmorType { CLOTH, PLATE, LEATHER, SHIELD, NONE }
public enum Weapon { DAGGER, STAFF, SWORD, AXE, NONE }

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class ItemSO : ScriptableObject
{
    [SerializeField] private int Id;

    [Space]
    [SerializeField] private bool pickable;

    [Header("Equipable Info")]
    [SerializeField] private Type itemType;
    [SerializeField] private Slot slot;

    [Header("Armor Info")]
    [Space]
    [SerializeField] private ArmorType armorType;
    [SerializeField] private int armor;

    [Header("Weapon Info")]
    [Space]
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private Weapon weapon;
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;

    [Header("Equipable basic Info")]
    [Space]
    [Range(0, 100)]
    [SerializeField] private float durability;
    [Range(0, 100)]
    [SerializeField] private int startingDurability;

    [Header("Basic stats")]
    [Space]
    [SerializeField] private int agility;
    [SerializeField] private int strength;
    [SerializeField] private int stamina;
    [SerializeField] private int intellect;

    [Header("UI Sprite")]
    [Space]
    [SerializeField] private Sprite img;

    public int ID { get { return Id; } private set { Id = value; } }

    public bool IsPickable { get { return pickable; } set { pickable = value; } }

    public Type Type { get { return itemType; } }
    public Slot Slot { get { return slot; } }

    public float Durability { get { return durability; } set { durability = value; } }
    public int StartDurability { get { return startingDurability; } }

    public int Agility { get { return agility; } }
    public int Strength { get { return strength; } }
    public int Stamina { get { return stamina; } }
    public int Intellect { get { return intellect; } }

    public Sprite Image { get { return img; } }

    public WeaponType WeaponType { get { return weaponType; } }
    public Weapon Weapon { get { return weapon; } }
    public int Damage { get { return damage; } }
    public float AttackSpeed { get { return attackSpeed; } }

    public ArmorType ArmorType { get { return armorType; } }
    public int Armor { get { return armor; } }
}
