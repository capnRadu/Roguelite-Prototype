using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private static readonly Vector3 Offset = new Vector3(-1, 1.5f, 0);
    private static readonly Vector3 EnemyOffset = new Vector3(-0.5f, 1f, 0);
    private const float NormalSpeed = 5f;
    private const float SlowSpeed = 2f;

    private bool isMoving = false;
    private bool isInCombat = false;

    private float speed = NormalSpeed;
    private Coroutine movingCoroutine;

    private Enemy[] allEnemies;
    private List<Enemy> enemiesToTarget = new List<Enemy>();
    private List<Enemy> enemiesCurrentlyTargeted;

    private void Update()
    {
        if (!isInCombat && !isMoving && !HasReachedTarget())
        {
            movingCoroutine = StartCoroutine(MoveToTarget(player.transform));
        }
    }

    private IEnumerator MoveToTarget(Transform target)
    {
        isMoving = true;
        speed = NormalSpeed;

        yield return new WaitForSeconds(Random.Range(0.1f, 1.4f));

        while (!HasReachedTarget())
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position + Offset, speed * Time.deltaTime);
            speed = Vector3.Distance(transform.position, target.position + Offset) < 0.2f ? SlowSpeed : NormalSpeed;
            yield return null;
        }

        isMoving = false;
    }

    private bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, player.transform.position + Offset) < 0.1f;
    }

    public void StartCombat()
    {
        if (isInCombat) return;

        isInCombat = true;
        StopMovement();

        FindEnemies();
        ShuffleEnemies();
        StartCoroutine(CycleThroughEnemies());
    }

    private void StopMovement()
    {
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
            movingCoroutine = null;
        }

        isMoving = false;
        speed = NormalSpeed;
    }

    private void FindEnemies()
    {
        allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        enemiesToTarget = new List<Enemy>(allEnemies);
        enemiesCurrentlyTargeted = new List<Enemy>(enemiesToTarget);
    }

    private void ShuffleEnemies()
    {
        Functions.Shuffle(enemiesCurrentlyTargeted);
    }

    private IEnumerator CycleThroughEnemies()
    {
        while (enemiesCurrentlyTargeted.Count > 0)
        {
            Enemy target = enemiesCurrentlyTargeted[0];

            if (target == null)
            {
                enemiesCurrentlyTargeted.RemoveAt(0);
                continue;
            }

            MoveTowardsTarget(target);

            if (Vector3.Distance(transform.position, target.transform.position + EnemyOffset) <= 0.1f)
            {
                int randomDamageTimes = Random.Range(1, 4);

                for (int i = 0; i < randomDamageTimes; i++)
                {
                    if (target == null) break;

                    target.GetComponent<Health>().TakeDamage(1);

                    float randomWait = Random.Range(0.1f, 0.3f);
                    yield return new WaitForSeconds(randomWait);

                }

                enemiesCurrentlyTargeted.RemoveAt(0);

                float randomWait2 = Random.Range(0.1f, 0.5f);
                yield return new WaitForSeconds(randomWait2);
            }

            yield return null;
        }

        RestartCombatIfNeeded();
    }

    private void MoveTowardsTarget(Enemy target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + EnemyOffset, speed * 2 * Time.deltaTime);
    }

    private void RestartCombatIfNeeded()
    {
        FindEnemies();

        if (allEnemies.Length > 0)
        {
            ShuffleEnemies();
            StartCoroutine(CycleThroughEnemies());
        }
        else
        {
            isInCombat = false;
        }
    }
}
