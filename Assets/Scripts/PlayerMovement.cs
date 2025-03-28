using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Companion companion;

    private float speed = 5f;
    private Vector3 move;

    private Vector3 facingDirection = Vector3.right;
    private Coroutine dashing;
    private float dashForce = 35f;

    private float attackRadius = 1.5f;
    [SerializeField] LayerMask enemyLayer;
    private float attackCooldown = 0.2f;
    private int attackDamage = 8;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Dash();
        Attack();
        AttackCooldown();
        Interact();
    }

    private void Move()
    {
        if (dashing == null)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            move = transform.right * x + transform.forward * z;
            rb.linearVelocity = move * speed;

            if (move != Vector3.zero)
            {
                facingDirection = move.normalized;
            }
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && move != Vector3.zero && dashing == null)
        {
            // rb.AddForce(facingDirection * dashForce, ForceMode.Impulse);
            dashing = StartCoroutine(DashCoroutine());
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && attackCooldown <= 0)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out Health enemyHealth))
                {
                    Vector3 direction = enemy.transform.position - transform.position;
                    float angle = Vector3.Angle(direction, facingDirection);

                    if (angle < 45f)
                    {
                        enemyHealth.TakeDamage(attackDamage);

                        foreach (Enemy enemy2 in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                        {
                            enemy2.SetFollowState();
                        }

                        if (hitEnemies[0] == enemy)
                        {
                            companion.StartCombat();
                        }
                    }
                }
            }

            attackCooldown = 0.3f;
        }
    }

    private void AttackCooldown()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().StartDialogue();
            }
        }
    }

    private IEnumerator DashCoroutine()
    {
        var endOfFrame = new WaitForEndOfFrame();
        var rigidbody = GetComponent<Rigidbody>();

        for (float timer = 0; timer < 0.2f; timer += Time.deltaTime)
        {
            rigidbody.MovePosition(transform.position + (facingDirection * (dashForce * Time.deltaTime)));
            yield return endOfFrame;
        }

        dashing = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + facingDirection * 2);
    }
}
