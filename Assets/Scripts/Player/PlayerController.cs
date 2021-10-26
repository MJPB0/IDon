using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameUI gameUI;

    [SerializeField] private GameObject body;
    private Player player;
    private PlayerAttack playerAttack;
    private ItemPickup itemPickup;

    private PlayerControls controls;
    private Vector2 move;
    private Vector2 rotate;
    private bool isRotating = false;

    [Header("Movement")]
    [Space]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool canRotate = true;

    private float movementSpeed;

    [Header("Player Rotate")]
    [Space]
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        itemPickup = FindObjectOfType<ItemPickup>();

        movementSpeed = player.MoveSpeed;

        controls = new PlayerControls();

        controls.Gameplay.Pickup.performed += ctx => Pickup();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += cts => move = Vector2.zero;

        controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotate.canceled += ctx => rotate = Vector2.zero;

        controls.Gameplay.Sprint.performed += ctx => Sprint();
        controls.Gameplay.Sprint.canceled += ctx => StopSprinting();

        controls.Gameplay.Attack.performed += ctx => Attack();
        controls.Gameplay.HeavyAttack.performed += ctx => HeavyAttack();

        controls.Gameplay.UseConsumable.performed += ctx => UsePotion();

        controls.Gameplay.ToggleInventory.performed += ctx => ToggleInventory();
    }

    private void Start()
    {
        player.CountStats();
        gameUI.UpdatePlayerHealthUI(player.PlayerHealth);
        gameUI.UpdatePlayerManaUI(player.PlayerMana);
        gameUI.UpdatePlayerStamina(player.Exhaustion, player.SprintDuration);

        player.PlayerDeath.AddListener(ManagePlayerDeath);
        player.PlayerAttacked.AddListener(ManageDamageTaken);
    }

    private void Update()
    {
        if (canMove)
            Move(movementSpeed);

        if (player.Exhaustion > 0)
        {
            player.Exhaustion -= Time.deltaTime * player.RestRate;
            gameUI.ManagePlayerStamina(player.Exhaustion);
            gameUI.ToggleStaminaSlider(true);
        }
        else if (player.Exhaustion < 0)
        {
            player.Exhaustion = 0;
            gameUI.ManagePlayerStamina(player.Exhaustion);
            gameUI.ToggleStaminaSlider(false);
        }

        if (canRotate)
            Rotate();
    }

    private void ToggleInventory()
    {
        gameUI.ToggleInventoryPanel();
    }

    private void Move(float speed)
    {
        Vector3 translation = new Vector3(move.x, 0, move.y);
        if (translation.magnitude >= 0.1f)
        {
            if (!isRotating)
            {
                float targetAngle = Mathf.Atan2(translation.x, translation.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(body.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                body.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            transform.Translate(translation * Time.deltaTime * speed);
            if (player.IsSprinting)
            {
                if (player.Exhaustion >= player.SprintDuration)
                    StopSprinting();
                else
                    player.Exhaustion += Time.deltaTime * player.SprintDepletionPerSecond;
            }
        }
    }

    private void Rotate()
    {
        Vector3 translation = new Vector3(rotate.x, 0, rotate.y);
        if (translation.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(translation.x, translation.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(body.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime * player.RotateSpeed);
            body.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
    }

    private void Sprint()
    {
        if (player.Exhaustion >= 98)
            return;
        movementSpeed = player.SprintSpeed;
        player.IsSprinting = true;
    }

    private void StopSprinting()
    {
        movementSpeed = player.MoveSpeed;
        player.IsSprinting = false;
    }

    private void Pickup()
    {
        itemPickup.CheckRange();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Attack()
    {
        playerAttack.TryToAttack(false);
    }

    private void HeavyAttack()
    {
        playerAttack.TryToAttack(true);
    }

    private void UsePotion()
    {
        if (!player.PlayerConsumable)
            return;
        player.PlayerConsumable.UsePotion();
    }

    public void CancelPotionEffectCountdown(Consumable cons)
    {
        StartCoroutine(ConsumableEffectTime(player, FindObjectOfType<GameUI>(), cons.itemInfo.Effectiveness, cons.itemInfo.Type, 
            cons.itemInfo.StatPotionType, cons.itemInfo.EffectTime));
    }

    IEnumerator ConsumableEffectTime(Player targetPlayer, GameUI gameUI, int eff, PotionType potType, StatPotionType statPotType, float time)
    {
        yield return new WaitForSeconds(time);
        switch (potType)
        {
            case PotionType.STATS:
                switch (statPotType)
                {
                    case StatPotionType.AGI:
                        targetPlayer.BonusAgility -= eff;
                        break;
                    case StatPotionType.STR:
                        targetPlayer.BonusStrength -= eff;
                        break;
                    case StatPotionType.INT:
                        targetPlayer.BonusIntellect -= eff;
                        break;
                    case StatPotionType.STAM:
                        targetPlayer.BonusStamina -= eff;
                        break;
                }
                break;
        }
        targetPlayer.CountStats();
        int[] currentStats = { targetPlayer.PlayerAgility,
                    targetPlayer.PlayerStrength,
                    targetPlayer.PlayerStamina,
                    targetPlayer.PlayerIntellect,
                    targetPlayer.PlayerDamage,
                    targetPlayer.PlayerArmor };
        gameUI.UpdateStatsUI(currentStats);
    }

    public void ManageDamageTaken()
    {
        player.ReduceItemsDurabilities();
        player.CountStats();
        gameUI.UpdatePlayerHealthUI(player.PlayerHealth);
    }

    public void ManagePlayerDeath()
    {
        Time.timeScale = 0.1f;
        player.CanAttack = false;
        canMove = false;
        canRotate = false;
        player.CanBeHit = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
