using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IEnemy
{
    public UnityEvent enemyDeath;
    public UnityEvent enemyAttacked;

    [SerializeField] protected EnemySO enemySO;
    [SerializeField] protected Transform currentTarget;
    [SerializeField] GameObject body;
    [SerializeField] protected EnemyTypes Type;

    [Space]
    [Header("Stats")]
    [SerializeField] protected int enemyHealth = 100;

    [Space]
    [SerializeField] protected int enemyAgility;
    [SerializeField] protected int enemyStrength;
    [SerializeField] protected int enemyStamina;
    [SerializeField] protected int enemyIntellect;

    [Space]
    [Header("Battle Settings")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float timeToAttack = 0;
    [SerializeField] protected bool canBeHit = true;
    [SerializeField] protected bool canAttack = true;
    [SerializeField] protected bool willPursue = true;

    [Space]
    [SerializeField] protected int enemyDamage;
    [Range(0, 1)]
    [SerializeField] protected float armorPiercing;
    [SerializeField] protected int enemyArmor;

    [Space]
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float rotateSpeed = 10f;
    [SerializeField] protected float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    [Space]
    [SerializeField] protected float idleTime = 2f;
    protected float currentIdleTime = 0f;

    protected Transform basePosition;

    #region Getters & Setters
    public Transform CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }
    public GameObject Body { get { return body; } }

    public Transform BasePosition { get { return basePosition; } set { basePosition = value; } }
    public EnemyTypes EnemyType { get { return Type; } }

    public float MoveSpeed { get { return moveSpeed; } }
    public float RotateSpeed { get { return rotateSpeed; } }
    public float TurnSmoothTime { get { return turnSmoothTime; } }

    public float AttackRange { get { return attackRange; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float TimeToAttack { get { return timeToAttack; } set { timeToAttack = value; } }
    public int Damage { get { return enemyDamage; } }

    public float ArmorPiercing { get { return armorPiercing; } }
    public int EnemyArmor { get { return enemyArmor; } set { enemyArmor = value; } }

    public int EnemyHealth { get { return enemyHealth; } set { enemyHealth = value; } }

    public bool CanBeHit { get { return canBeHit; } set { canBeHit = value; } }
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    public bool WillPursue { get { return willPursue; } set { willPursue = value; } }

    public int EnemyAgility { get { return enemyAgility; } set { enemyAgility = value; } }
    public int EnemyStrength { get { return enemyStrength; } set { enemyStrength = value; } }
    public int EnemyIntellect { get { return enemyIntellect; } set { enemyIntellect = value; } }
    public int EnemyStamina { get { return enemyStamina; } set { enemyStamina = value; } }

    public float IdleTime { get { return idleTime; } }
    public float CurrentIdleTime { get { return currentIdleTime; } set { currentIdleTime = value; } }
    #endregion

    public void CountStats()
    {
        enemyDamage = enemySO.BaseDamage;
        enemyAgility = enemySO.EnemyBaseAgility;
        enemyStamina = enemySO.EnemyBaseStamina;
        enemyStrength = enemySO.EnemyBaseStrength;
        enemyIntellect = enemySO.EnemyBaseIntellect;
        enemyArmor = enemySO.BaseArmor;
        attackSpeed = enemySO.BaseAttackSpeed;
    }

    public void EnemyDeath()
    {
        enemyDeath?.Invoke();
    }
}
