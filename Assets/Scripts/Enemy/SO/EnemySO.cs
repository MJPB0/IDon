using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy", order = 2)]
public class EnemySO : ScriptableObject
{
    [Header("Enemy Base Stats")]
    [SerializeField] private int baseEnemyDamage;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private int baseEnemyArmor;

    [Space]
    [SerializeField] private int baseEnemyAgility;
    [SerializeField] private int baseEnemyStrength;
    [SerializeField] private int baseEnemyStamina;
    [SerializeField] private int baseEnemyIntellect;

    public int EnemyBaseAgility { get { return baseEnemyAgility; } set { baseEnemyAgility = value; } }
    public int EnemyBaseStrength { get { return baseEnemyStrength; } set { baseEnemyStrength = value; } }
    public int EnemyBaseIntellect { get { return baseEnemyIntellect; } set { baseEnemyIntellect = value; } }
    public int EnemyBaseStamina { get { return baseEnemyStamina; } set { baseEnemyStamina = value; } }

    public int BaseDamage { get { return baseEnemyDamage; } set { baseEnemyDamage = value; } }
    public float BaseAttackSpeed { get { return baseAttackSpeed; } set { baseAttackSpeed = value; } }
    public int BaseArmor { get { return baseEnemyArmor; } set { baseEnemyArmor = value; } }
}
