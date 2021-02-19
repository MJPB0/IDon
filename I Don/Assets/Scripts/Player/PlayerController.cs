using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject body;
    Player player;
    PlayerAttack playerAttack;
    ItemPickup itemPickup;

    PlayerControls controls;
    Vector2 move;
    Vector2 rotate;
    bool isRotating = false;

    [Header("Movement")]
    [Space]
    [SerializeField] bool canMove = true;
    [SerializeField] bool canRotate = true;

    [Header("Dizzy Effect")]
    [Space]
    [SerializeField] float dizzyTime = 2f;
    [SerializeField] float dizzyTimer = 0f;
    [SerializeField] bool isDizzy = false;
    [SerializeField] GameObject dizzyParticles;

    float movementSpeed;

    [Header("Player Rotate")]
    [Space]
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float turnSmoothVelocity;

    public bool IsDizzy { get { return isDizzy; } set { isDizzy = value; } }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        itemPickup = FindObjectOfType<ItemPickup>();

        movementSpeed = player.getMoveSpeed();

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
    }

    private void Start()
    {
        player.CountStats();
        FindObjectOfType<GameUI>().UpdatePlayerHealthUI(player.PlayerHealth);
        FindObjectOfType<GameUI>().UpdatePlayerManaUI(player.PlayerMana);

        player.PlayerDeath.AddListener(ManagePlayerDeath);
    }

    private void Update()
    {
        if (canMove)
            Move(movementSpeed);
        if (canRotate)
            Rotate();

        if (isDizzy){
            if (dizzyTimer < dizzyTime)
                dizzyTimer += Time.deltaTime;
            else
            {
                dizzyTimer = 0f;
                isDizzy = false;
                canMove = true;
                canRotate = true;
                player.CanAttack = true;
            }    
        }
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
                    player.Exhaustion += Time.deltaTime * player.getSprintDepletionPerSecond();
            }
            else if (player.Exhaustion > 0)
                player.Exhaustion -= Time.deltaTime * player.getRestRate();
            else if (player.Exhaustion < 0)
                player.Exhaustion = 0;
        }
    }

    private void Rotate()
    {
        Vector3 translation = new Vector3(rotate.x, 0, rotate.y);
        if (translation.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(translation.x, translation.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(body.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime * player.getRotateSpeed());
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
        movementSpeed = player.getSprintSpeed();
        player.IsSprinting = true;
    }

    private void StopSprinting()
    {
        movementSpeed = player.getMoveSpeed();
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
        if (isDizzy)
            return;
        player.PlayerConsumable.UsePotion();
    }

    public void CancelPotionEffectCountdown(Consumable cons)
    {
        StartCoroutine(ConsumableEffectTime(player, FindObjectOfType<GameUI>(), cons.itemInfo.Effectiveness, cons.itemInfo.getType(), 
            cons.itemInfo.getStatPotionType(), cons.itemInfo.EffectTime));
    }

    IEnumerator ConsumableEffectTime(Player playr, GameUI gameUI, int eff, PotionType potType, StatPotionType statPotType, float time)
    {
        yield return new WaitForSeconds(time);
        switch (potType)
        {
            case PotionType.STATS:
                switch (statPotType)
                {
                    case StatPotionType.AGI:
                        playr.BonusAgility -= eff;
                        break;
                    case StatPotionType.STR:
                        playr.BonusStrength -= eff;
                        break;
                    case StatPotionType.INT:
                        playr.BonusIntellect -= eff;
                        break;
                    case StatPotionType.STAM:
                        playr.BonusStamina -= eff;
                        break;
                }
                break;
        }
        playr.CountStats();
        int[] currentStats = { playr.PlayerAgility,
                    playr.PlayerStrength,
                    playr.PlayerStamina,
                    playr.PlayerIntellect,
                    playr.getPlayerDamage(),
                    playr.getArmorPiercing(),
                    playr.PlayerArmor };
        gameUI.UpdateStatsUI(currentStats);
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

    public void PlayerDizziness()
    {
        canMove = false;
        player.CanAttack = false;
        StartCoroutine(DizzyEffect(dizzyTime, dizzyParticles));
    }

    IEnumerator DizzyEffect(float time, GameObject ps)
    {
        GameObject go = Instantiate(ps);
        go.transform.SetParent(body.transform);
        go.transform.localPosition = new Vector3(0, 1.3f, 0);
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
}
