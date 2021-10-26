using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType { HEALTH, MANA, STATS }
public enum StatPotionType { STR, STAM, INT, AGI, NONE}
[CreateAssetMenu(fileName = "Potion", menuName = "Potion", order = 2)]
public class ConsumableSO : ScriptableObject
{
    [SerializeField] int Id;

    [Space]
    [SerializeField] private bool isPickable;
    [SerializeField] private bool isTemporary;
    [SerializeField] private float effectTime;

    [SerializeField] private PotionType potionType;
    [SerializeField] private StatPotionType statsPotionType;
    [SerializeField] private int effectiveness;
    [SerializeField] private int currentUses;
    [SerializeField] private int startUses;

    [Header("UI Sprite")]
    [Space]
    [SerializeField] private Sprite img;

    public int ID { get { return Id; } private set { Id = value; } }
    public int Effectiveness { get { return effectiveness; } set { effectiveness = value; } }
    public PotionType Type { get { return potionType; } }
    public StatPotionType StatPotionType { get { return statsPotionType; } }
    public bool IsPickable { get { return isPickable; } set { isPickable = value; } }
    public Sprite Image { get { return img; } }
    public int Uses { get { return currentUses; } set { currentUses = value; } }
    public int StartUses { get { return startUses; } }
    public bool IsTemporary { get { return isTemporary; } }
    public float EffectTime { get { return effectTime; } set { effectTime = value; } }
}
