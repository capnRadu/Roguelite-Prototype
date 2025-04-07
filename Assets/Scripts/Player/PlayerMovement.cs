using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashForce = 35f;
    [SerializeField] private float dashDuration = 0.2f;

    private Vector3 moveInput;
    private Vector3 facingDirection = Vector3.right;
    private Coroutine dashCoroutine;

    public Vector3 FacingDirection => facingDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Tick()
    {
        HandleMovement();
        HandleDash();
    }

    private void HandleMovement()
    {
        if (dashCoroutine != null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveInput = transform.right * x + transform.forward * z;
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput != Vector3.zero)
        {
            facingDirection = moveInput.normalized;
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && moveInput != Vector3.zero && dashCoroutine == null)
        {
            dashCoroutine = StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.MovePosition(transform.position + facingDirection * dashForce * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        dashCoroutine = null;
    }
}
