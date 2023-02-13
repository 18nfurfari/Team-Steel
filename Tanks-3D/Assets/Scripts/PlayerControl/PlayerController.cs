using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport.Utilities;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerController : NetworkBehaviour
{
    // used to store player input
    private Vector2 _playerInput;
    private bool _fire;
    private float _leftTrackSpeed;
    private float _rightTrackSpeed;

    // store reference to player's character controller to be used to move
    [SerializeField] private CharacterController controller;
    
    //[SerializeField] private Rigidbody rb;
    
    // values for player's movement speed and rotation speed
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerRotation = 60f;
    [SerializeField] private float turretRotation = 50f;

    private PlayerControlActionAsset _playerControlActionAsset;
    
    private GameObject _leftTrack;
    private GameObject _rightTrack;

    private GameObject _turret;
    
    private void Awake()
    {
        _playerControlActionAsset = new PlayerControlActionAsset();
        _leftTrack = GameObject.Find("Panzer_VI_E_Track_L");
        _rightTrack = GameObject.Find("Panzer_VI_E_Track_R");
        _turret = GameObject.Find("Panzer_VI_E_Turret");
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
    //     FireInput(value.isPressed);
    // }
    //
    // private void FireInput(bool newFireState)
    // {
    //     _fire = newFireState;
    // }
    
    private void PlayerMovement()
    {
        // playerInput.y only allows forward and backward movement
        controller.Move(transform.forward * (_playerInput.y * playerSpeed * Time.deltaTime));

        // playerInput.x only allows player side to side rotation
        transform.Rotate(transform.up, playerRotation * _playerInput.x * Time.deltaTime);
    }
    
    
    private void Update()
    {
        // Player Gravity
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, -1, 0));
        }
        
        // Track Movement
        if (Input.GetKey(KeyCode.W))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = 0.07f;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = 0.07f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = -0.07f;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = -0.07f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = 0.07f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = 0.07f;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
        }
        else
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
        }
        
        // Turret Movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _turret.transform.Rotate(transform.up, -1 * turretRotation * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _turret.transform.Rotate(transform.up, turretRotation * Time.deltaTime);
        }
        
        // Turret Firing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Fire!");
        }
        PlayerMovement();
    }
}
