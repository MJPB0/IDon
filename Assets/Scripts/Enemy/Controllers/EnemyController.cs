using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    protected Enemy enemy;
    protected Player player;

    protected EnemyMode currentMode;

    [SerializeField] protected Slider HealthUI;

    [Header("Enemy Floating Text")]
    [SerializeField] protected GameObject LeftEnemyFloatingText;
    [SerializeField] protected GameObject RightEnemyFloatingText;
    [SerializeField] protected Transform LeftFloatingTextEnemyParent;
    [SerializeField] protected Transform RightFloatingTextEnemyParent;

    public readonly EnemyFightingMode enemyFightingMode = new EnemyFightingMode();
    public readonly EnemyIdleMode enemyIdleMode = new EnemyIdleMode();
    public readonly EnemyReturnMode enemyReturnMode = new EnemyReturnMode();
    public readonly EnemyPursuingMode enemyPursuingMode = new EnemyPursuingMode();
    public readonly EnemyDeathMode enemyDeathMode = new EnemyDeathMode();

    public Enemy Enemy { get { return enemy; } }
    public Player Player { get { return player; } }

    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        player = FindObjectOfType<Player>();

        ChangeEnemyMode(enemyIdleMode);
        enemy.CountStats();
        UpdateEnemyHealthUI(enemy.EnemyHealth);
        player.PlayerDeath.AddListener(ChangeEnemyModeToIdle);

        enemy.enemyDeath.AddListener(EnemyDeath);
        enemy.enemyAttacked.AddListener(ManageDamageTaken);
    }
    private void Update()
    {
        if (Enemy.TimeToAttack > 0)
            Enemy.TimeToAttack -= Time.deltaTime;

        currentMode.EnemyUpdate(this);
    }

    public void ChangeEnemyMode(EnemyMode mode)
    {
        currentMode = mode;
        currentMode.EnterMode(this);
    }

    public void ChangeEnemyModeToIdle()
    {
        currentMode = enemyIdleMode;
        currentMode.EnterMode(this);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        currentMode.EnemyOnTriggerEnter(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentMode.EnemyOnTriggerExit(this, other);
    }

    public void GoToTarget()
    {
        float transX = Enemy.CurrentTarget.transform.position.x - transform.position.x;
        float transZ = Enemy.CurrentTarget.transform.position.z - transform.position.z;
        Vector3 translation = new Vector3(transX, 0, transZ);
        translation.Normalize();

        if (translation.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(translation.x, translation.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(Enemy.Body.transform.eulerAngles.y, targetAngle, ref Enemy.turnSmoothVelocity, Enemy.TurnSmoothTime);
            Enemy.Body.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            transform.Translate(Enemy.MoveSpeed * Time.deltaTime * translation);
        }
    }

    public void Attack()
    {
        player.TakeDamage(enemy.Damage * enemy.ArmorPiercing);
        //Debug.Log($"Enemy attacked Player for {healthToTake}dmg");
    }

    public void EnemyAttacked(float dmg)
    {
        if (!enemy.CanBeHit)
            return;

        int healthToTake = Mathf.RoundToInt(dmg - enemy.EnemyArmor);
        if (gameObject.TryGetComponent<HumanoidEnemy>(out HumanoidEnemy e))
            healthToTake = Mathf.RoundToInt(dmg - enemy.EnemyArmor * e.ArmorEffieciency);

        if (healthToTake < 0)
            healthToTake = 0;

        if (enemy.EnemyHealth <= healthToTake)
        {
            enemy.EnemyHealth -= healthToTake;
            UpdateEnemyHealthUI(0);
            FloatingText(true, healthToTake);
            enemy.EnemyDeath();
        }
        else
        {
            enemy.EnemyHealth -= healthToTake;
            FloatingText(true, healthToTake);
            EnemyDamaged();
        }
    }

    public void EnemyDamaged()
    {
        enemy.enemyAttacked?.Invoke();
    }

    public void ManageDamageTaken()
    {
        enemy.CountStats();
        UpdateEnemyHealthUI(enemy.EnemyHealth);
        if (!gameObject.TryGetComponent<HumanoidEnemy>(out HumanoidEnemy e))
            return;
        e.ReduceItemsDurabilities();
    }

    public void UpdateEnemyHealthUI(int value)
    {
        HealthUI.value = value;
    }

    public void FloatingText(bool isDamage, int value)
    {
        if (isDamage)
        {
            GameObject obj = Instantiate(LeftEnemyFloatingText);
            obj.transform.SetParent(LeftFloatingTextEnemyParent);

            obj.GetComponent<Text>().text = $"-{value}";
            StartCoroutine(WaitAndDestroy(1f, obj));
        }
        else
        {
            GameObject obj = Instantiate(RightEnemyFloatingText);
            obj.transform.SetParent(RightFloatingTextEnemyParent);

            obj.GetComponent<Text>().text = $"+{value}";
            StartCoroutine(WaitAndDestroy(1f, obj));
        }
    }
    public void EnemyDeath()
    {
        ChangeEnemyMode(enemyDeathMode);
    }

    IEnumerator WaitAndDestroy(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
    public void DestroyEnemy()
    {
        StartCoroutine(WaitAndDestroy(3f, gameObject));
    }
}
