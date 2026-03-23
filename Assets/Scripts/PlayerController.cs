using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {
    [SerializeField] private GameInputManager inputManager;
    [SerializeField] private Rigidbody rb;
    private float playerSpeed = 5f;
    private float laneDiastance = 2.5f;
    private float laneChangeSpeed = 6f;
    private float jumpForce = 5f;
    private float groundCheckDistance = 0.5f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Animator animator;
    private int currentLane = 0;
    private bool isOnGround = true;


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
        ChangeLane(-1);
    }
    private void InputManager_moveRight(object sender, EventArgs e) {
        ChangeLane(1);
    }
    private void InputManager_jump(object sender, EventArgs e) {
        if (!isOnGround) return;
        Jump();
    }
    private void ChangeLane(int x) {
        currentLane += x;
        currentLane = Mathf.Clamp(currentLane, -1, 1);
    }
    private void PlayerRun() {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, playerSpeed);
    }
    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.ResetTrigger("Jump");
        animator.SetTrigger("Jump");
    }
    private void MoveToLane() {
    
        float targetX = currentLane * laneDiastance;

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);
    
    }
    private void CheckGround() {
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer))
            isOnGround = true;
        else
            isOnGround = false;
    }
}

