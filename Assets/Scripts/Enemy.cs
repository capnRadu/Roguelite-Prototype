using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float attackCooldown = 0.7f;
    private int attackDamage = 3;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Dialogue dialogueObject;

    [SerializeField] private string generalDialogue;
    [SerializeField] private string firstOption;
    [SerializeField] private string secondOption;
    [SerializeField] private string thirdOption;

    private enum State
    {
        Stand,
        Follow
    }
    private State currentState = State.Stand;

    private void Update()
    {
        FollowOrAttackPlayer();
        AttackCooldown();
    }

    private void FollowOrAttackPlayer()
    {
        if (player == null || currentState == State.Stand)
        {
            return;
        }

        Vector3 destination = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        if (Vector3.Distance(transform.position, destination) > 1f)
        {
            // transform.position = Vector3.MoveTowards(transform.position, destination, 2f * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards(transform.position, destination, 10f * Time.deltaTime));
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (attackCooldown <= 0)
        {
            player.GetComponent<Health>().TakeDamage(attackDamage);
            attackCooldown = 0.7f;
        }
    }

    private void AttackCooldown()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void SetFollowState()
    {
        currentState = State.Follow;

        if (canvas.enabled)
        {
            canvas.enabled = false;
            dialogueObject.enabled = false;
        }   
    }

    public void StartDialogue()
    {
        if (currentState != State.Follow && !canvas.enabled)
        {
            dialogueObject.SetupDialogue(generalDialogue, firstOption, secondOption, thirdOption, this);
            canvas.enabled = true;
            dialogueObject.enabled = true;
        }
    }

    public void ChooseDialogueOption(int option)
    {
        canvas.enabled = false;
        dialogueObject.enabled = false;

        switch (option)
        {
            case 1:
                int random = Random.Range(0, 4);
                if (random == 1)
                {
                    goto case 3;
                }
                else
                {
                    Destroy(gameObject);
                }
                break;

            case 2:
                int random2 = Random.Range(0, 2);
                if (random2 == 0)
                {
                    goto case 3;
                }
                else
                {
                    Destroy(gameObject);
                }
                break;

            case 3:
                foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
                {
                    enemy.SetFollowState();
                }
                break;
        }
    }
}
