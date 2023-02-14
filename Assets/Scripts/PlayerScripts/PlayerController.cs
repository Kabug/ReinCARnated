using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction movement;

    private void OnEnable()
    {
        movement = new InputAction("movement", InputActionType.Value, "<Keyboard>/arrowKeys");
        movement.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = movement.ReadValue<Vector2>();
        float horizontal = moveInput.x;
        float vertical = moveInput.y;

        // Move the car
        transform.Translate(horizontal * Time.deltaTime, 0, vertical * Time.deltaTime);
    }
}
