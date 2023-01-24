using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private Vector2 playerInput;
    [SerializeField] CharacterController controller;
    [SerializeField] private float playerSpeed = 3f;
    [SerializeField] private float playerRotation = 40f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
    }

    private void OnMove(InputValue value)
    {
        playerInput = value.Get<Vector2>();
    }
    
    private void PlayerMovement()
    {
        controller.Move(transform.forward * playerInput.y * playerSpeed * Time.deltaTime);

        transform.Rotate(transform.up, playerRotation * playerInput.x * Time.deltaTime);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
}
