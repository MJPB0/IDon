using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    Enemy enemy;
    Player player;

    EnemyMode currentMode;

    [SerializeField] GameObject enemyGO;

    [SerializeField] Slider HealthUI;

    [Header("Enemy Floating Text")]
    [SerializeField] GameObject LeftEnemyFloatingText;
    [SerializeField] GameObject RightEnemyFloatingText;
    [SerializeField] Transform LeftFloatingTextEnemyParent;
    [SerializeField] Transform RightFloatingTextEnemyParent;

    public readonly EnemyFightingMode enemyFightingMode = new EnemyFightingMode();
    public readonly EnemyIdleMode enemyIdleMode = new EnemyIdleMode();
    public readonly EnemyPatrollingMode enemyPatrollingMode = new EnemyPatrollingMode();
    public readonly EnemyPursuingMode enemyPursuingMode = new EnemyPursuingMode();
    public readonly EnemyDeathMode enemyDeathMode = new EnemyDeathMode();

    public Enemy getEnemy() { return enemy; }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>();

        ChangeEnemyMode(enemyIdleMode);
        enemy.CountStats();
        UpdateEnemyHealthUI(enemy.EnemyHealth);
        player.PlayerDeath.AddListener(ChangeEnemyModeToIdle);

        enemy.enemyDeath.AddListener(EnemyDeath);
    }
    private void Update()
    {
        if (getEnemy().TimeToAttack > 0)
            getEnemy().TimeToAttack -= Time.deltaTime;

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
        float transX = getEnemy().CurrentTarget.transform.position.x - transform.position.x;
        float transZ = getEnemy().CurrentTarget.transform.position.z - transform.position.z;
        Vector3 translation = new Vector3(transX, 0, transZ);
        translation.Normalize();

        if (translation.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(translation.x, translation.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(getEnemy().getBody().transform.eulerAngles.y, targetAngle, ref getEnemy().turnSmoothVelocity, getEnemy().TurnSmoothTime());
            getEnemy().getBody().transform.rotation = Quaternion.Euler(0f, angle, 0f);

            transform.Translate(translation * Time.deltaTime * getEnemy().getMoveSpeed());
        }
    }

    public void Attack()
    {
        int healthToTake = enemy.getDamage() - Mathf.RoundToInt(player.PlayerArmor * player.getArmorEffieciency);
        if (healthToTake < 0)
            healthToTake = 0;
        player.TakeDamage(healthToTake, enemy);
        //Debug.Log($"Enemy attacked Player for {healthToTake}dmg");
    }
    public void TakeDamage(int value, Player target)
    {
        if (!enemy.CanBeHit)
            return;
        if (enemy.EnemyHealth <= value)
        {
            enemy.EnemyHealth -= value;
            UpdateEnemyHealthUI(0);
            FloatingText(true, value);
            enemy.EnemyDeath();
        }
        else
        {
            enemy.EnemyHealth -= value;
            enemy.ReduceItemsDurabilities(target.getArmorPiercing());
            enemy.CountStats();
            UpdateEnemyHealthUI(enemy.EnemyHealth);
            FloatingText(true, value);
            //Debug.Log("Reduced all items durability by " + target.getArmorPiercing() + "%");
            //Debug.Log(gameObject.name + " suffered " + value + "dmg");
        }
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
        StartCoroutine(WaitAndDestroy(3f, enemyGO));
    }
}
