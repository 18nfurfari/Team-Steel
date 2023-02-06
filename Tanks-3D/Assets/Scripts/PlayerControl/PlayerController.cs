using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport.Utilities;
using Unity.Services.Lobbies.Models;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : NetworkBehaviour
{
    // used to store player input
    private Vector2 _playerInput;

    // store reference to player's character controller to be used to move
    [SerializeField] private CharacterController controller;
    
    // values for player's movement speed and rotation speed
    [SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float playerRotation = 60f;

    private PlayerControlActionAsset _playerControlActionAsset;

    private void Awake()
    {
        _playerControlActionAsset = new PlayerControlActionAsset();
    }

    private void OnEnable()
    {
        _playerControlActionAsset.Enable();
    }

    private void OnDisable()
    {
        _playerControlActionAsset.Disable();
    }

    private void OnMove(InputValue value)
    {
        // store value received from input either keyboard or controller
        _playerInput = value.Get<Vector2>();
        
    }

    // private void OnFire(InputValue value)
    // {
    //     
    // }

    private void PlayerMovement()
    {
        // playerInput.y only allows forward and backward movement
        controller.Move(transform.forward * _playerInput.y * playerSpeed * Time.deltaTime);
        
        // playerInput.x only allows player side to side rotation
        transform.Rotate(transform.up, playerRotation * _playerInput.x * Time.deltaTime);

    }
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Fire!");
        }
        PlayerMovement();
    }
}
