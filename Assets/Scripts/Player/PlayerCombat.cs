using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Companion companion;
    private PlayerMovement movement;

    [SerializeField] private float attackRadius = 1.5f;
    public float AttackRadius => attackRadius;

    [SerializeField] private int attackDamage = 8;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private LayerMask enemyLayer;
    private float cooldownTimer = 0f;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public void Tick()
    {
        HandleAttack();

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleAttack()
    {
        if (!Input.GetMouseButtonDown(0) || cooldownTimer > 0f) return;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        bool combatTriggered = false;

        foreach (var enemyCol in hitEnemies)
        {
            if (enemyCol.TryGetComponent(out Health health))
            {
                Vector3 dirToEnemy = enemyCol.transform.position - transform.position;
                float angle = Vector3.Angle(dirToEnemy, movement.FacingDirection);

                if (angle < 45f)
                {
                    health.TakeDamage(attackDamage);
                    combatTriggered = true;

                    foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                    {
                        enemy.SetFollowState();
                    }

                    break;
                }
            }
        }

        if (combatTriggered)
        {
            companion.StartCombat();
        }

        cooldownTimer = attackCooldown;
    }
}