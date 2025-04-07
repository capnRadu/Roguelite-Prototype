using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerCombat), typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerInteraction interaction;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        movement.Tick();
        combat.Tick();
        interaction.Tick();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (combat)
        {
            Gizmos.DrawWireSphere(transform.position, combat.AttackRadius);
        }

        if (movement)
        {
            Gizmos.DrawLine(transform.position, transform.position + movement.FacingDirection * 2);
        }
    }
}