using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float laneDiastance = 3f;
    [SerializeField] private float laneChangeSpeed = 6f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float groundCheckDistance = 1.55f;
    [SerializeField] LayerMask groundLayer;
    private int currentLane = 0;
    private bool isOnGround = true;


    // System
    private void Update() {
        Debug.Log(isOnGround);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
        CheckGround();
        if (Input.GetKeyDown(KeyCode.A)) {
            currentLane--;
            currentLane = Mathf.Clamp(currentLane, -1, 1);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            currentLane++;
            currentLane = Mathf.Clamp(currentLane, -1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround) {    
            Jump();
        }
    }

    private void FixedUpdate() {
        PlayerRun();
        MoveToLane();
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void PlayerRun() {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, playerSpeed);
    }

    private void MoveToLane() {
    
        float targetX = currentLane * laneDiastance;

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);
    
    }
    private void CheckGround() {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);

        if (Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer))
            isOnGround = true;
        else
            isOnGround = false;
    }
}

