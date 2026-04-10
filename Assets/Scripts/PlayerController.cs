using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {
    public event EventHandler onLaneChange;
    [SerializeField] private GameInputManager inputManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Rigidbody rb;
    private float laneDiastance = 2.5f;
    private float laneChangeSpeed = 6f;
    private float jumpForce = 5f;
    private float radiusSphereCast = 0.3f;
    private float maxSphereCastDistance = 0.6f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] Animator animator;
    private int currentLane = 0;
    private bool isOnGround = true;
    private bool canChangeLane = true;


    // System
    private void OnEnable() {
        inputManager.moveLeft += InputManager_moveLeft;
        inputManager.moveRight += InputManager_moveRight;
        inputManager.jump += InputManager_jump;
    }
    private void OnDisable() {
        inputManager.moveLeft -= InputManager_moveLeft;
        inputManager.moveRight -= InputManager_moveRight;
        inputManager.jump -= InputManager_jump;
    }

    private void FixedUpdate() {
        PlayerRun();
        MoveToLane();
        CheckGround();
        
    }
    private void InputManager_moveLeft(object sender, EventArgs e) {
        if (!CanChangeLane(-1)) return;
        ChangeLane(-1);
    }
    private void InputManager_moveRight(object sender, EventArgs e) {
        if (!CanChangeLane(1)) return;
        ChangeLane(1);
    }
    private void InputManager_jump(object sender, EventArgs e) {
        if (!isOnGround) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Jump();
    }
    private void ChangeLane(int x) {
        currentLane += x;
        onLaneChange?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerRun() {
        float speed = gameManager.GetSpeed();
        Vector3 velocity = rb.linearVelocity;
        velocity.z = speed;
        rb.linearVelocity = velocity;
    }
    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.ResetTrigger("Jump");
        animator.SetTrigger("Jump");
    }
    private void MoveToLane() {
        float targetX = currentLane * laneDiastance;

        Vector3 position = rb.position;
        float newX = Mathf.Lerp(position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        rb.MovePosition(new Vector3(newX, position.y, position.z));
    }
    private void CheckGround() {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.SphereCast(origin, radiusSphereCast, Vector3.down, out RaycastHit hit, maxSphereCastDistance, groundLayer)) {
            isOnGround = true;
        } else {
            isOnGround = false;
        }

    }

    private bool CanChangeLane(int i) {
        if (currentLane+i >= -1 & currentLane+i <= 1) canChangeLane = true;
        else canChangeLane = false;
        return canChangeLane; 
    }

    public bool IsOnGround() {
        return isOnGround;
    }
}

