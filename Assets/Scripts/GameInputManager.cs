using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputManager : MonoBehaviour {
    public event EventHandler moveLeft;
    public event EventHandler moveRight;
    public event EventHandler jump;
    private PlayerInputActionMap playerInputActionMap;

    private void Awake() {
        playerInputActionMap = new PlayerInputActionMap();
    }

    private void OnEnable() {
        playerInputActionMap.Enable();
        playerInputActionMap.Player.MoveLeft.performed += playerInputActionMap_Player_MoveLeft;
        playerInputActionMap.Player.MoveRight.performed += playerInputActionMap_Player_MoveRight;
        playerInputActionMap.Player.JUMP.performed += playerInputActionMap_Player_Jump;
    }
    private void OnDisable() {
        playerInputActionMap.Disable();
        playerInputActionMap.Player.MoveLeft.performed -= playerInputActionMap_Player_MoveLeft;
        playerInputActionMap.Player.MoveRight.performed -= playerInputActionMap_Player_MoveRight;
        playerInputActionMap.Player.JUMP.performed -= playerInputActionMap_Player_Jump;
    }

    private void playerInputActionMap_Player_Jump(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        jump?.Invoke(this, EventArgs.Empty);
    }
    private void playerInputActionMap_Player_MoveRight(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        moveRight?.Invoke(this, EventArgs.Empty);
    }
    private void playerInputActionMap_Player_MoveLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        moveLeft?.Invoke(this, EventArgs.Empty);
    }
}