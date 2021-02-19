using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PotionType { HEALTH, MANA, STATS }
public enum StatPotionType { STR, STAM, INT, AGI, NONE}
[CreateAssetMenu(fileName = "Potion", menuName = "Potion", order = 2)]
public class ConsumableSO : ScriptableObject
{
    [SerializeField] bool isPickable;
    [SerializeField] bool isTemporary;
    [SerializeField] float effectTime;

    [SerializeField] PotionType potionType;
    [SerializeField] StatPotionType statsPotionType;
    [SerializeField] int effectiveness;
    [SerializeField] int currentUses;
    [SerializeField] int startUses;

    [Header("UI Sprite")]
    [Space]
    [SerializeField] Sprite img;

    public int Effectiveness { get { return effectiveness; } set { effectiveness = value; } }
    public PotionType getType() { return potionType; }
    public StatPotionType getStatPotionType() { return statsPotionType; }
    public bool IsPickable { get { return isPickable; } set { isPickable = value; } }
    public Sprite getImage() { return img; }
    public int Uses { get { return currentUses; } set { currentUses = value; } }
    public int StartUses { get { return startUses; } }
    public bool IsTemporary { get { return isTemporary; } }
    public float EffectTime { get { return effectTime; } set { effectTime = value; } }
}
