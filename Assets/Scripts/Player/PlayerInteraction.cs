using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    public void Tick()
    {
        HandleInteract();
    }

    private void HandleInteract()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius, interactableLayer);

        foreach (var col in hits)
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                enemy.StartDialogue();
            }
        }
    }
}
