using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    // used to store player input
    private Vector2 _playerInput;
    // private bool _fire;
    
    // store reference to player's character controller to be used to move
    [SerializeField] private CharacterController controller;

    // values for player's movement speed and rotation speed
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerRotation = 60f;
    // [SerializeField] private float turretRotation = 50f;
    // [SerializeField] private float bulletSpeed = 30f;
    // [SerializeField] private float trackSpeed = 0.10f;

    private PlayerControlActionAsset _playerControlActionAsset;
    
    // private GameObject _leftTrack;
    // private GameObject _rightTrack;
    // private GameObject _turret;

    // public Transform bulletSpawnPoint;
    // public GameObject bulletPrefab;

    // private GameObject _currentAmmoObject;
    // private TextMeshProUGUI _currentAmmoText;
    // public int currentAmmo;
    // private bool reloading;
    // private float reloadTime;
    // private float cooldownTime;

    [SerializeField] private InputActionReference cameraMove;

    public override void OnNetworkSpawn()
    {
        _playerControlActionAsset = new PlayerControlActionAsset();
        // _leftTrack = GameObject.Find("Panzer_VI_E_Track_L");
        // _rightTrack = GameObject.Find("Panzer_VI_E_Track_R");
        // _turret = GameObject.Find("Panzer_VI_E_Turret");

        // _currentAmmoObject = GameObject.Find("CurrentAmmo");
        // _currentAmmoText = _currentAmmoObject.GetComponent<TextMeshProUGUI>();

        // reloading = false;
        // reloadTime = 3.0f;
        // cooldownTime = 0.5f;
        // currentAmmo = 4;
    }

    private void OnEnable()
    {
        _playerControlActionAsset.Enable();
        // reloadTime = Time.time;
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
        // moving backwards is slower
        if (_playerInput.y < 0)
        {
            controller.Move(transform.forward * (_playerInput.y * (playerSpeed * 0.5f) * Time.deltaTime));
        }
        else
        {
            controller.Move(transform.forward * (_playerInput.y * playerSpeed * Time.deltaTime));
        }


        // playerInput.x only allows player side to side rotation
        transform.Rotate(transform.up, playerRotation * _playerInput.x * Time.deltaTime);
    }
    
    
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        // Player Gravity
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0, -1, 0));
        }
        
        // Track Movement
        // if (Input.GetKey(KeyCode.W))
        // {
        //     _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
        //     _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
        // }
        // else if (Input.GetKey(KeyCode.S))
        // {
        //     _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = -trackSpeed;
        //     _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = -trackSpeed;
        // }
        // else if (Input.GetKey(KeyCode.A))
        // {
        //     _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
        //     _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
        // }
        // else if (Input.GetKey(KeyCode.D))
        // {
        //     _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
        //     _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
        // }
        // else
        // {
        //     _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
        //     _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
        // }
        //
        // // Turret Movement
        // if (Input.GetKey(KeyCode.LeftArrow))
        // {
        //     _turret.transform.Rotate(transform.up, -1 * turretRotation * Time.deltaTime);
        // }
        // if (Input.GetKey(KeyCode.RightArrow))
        // {
        //     _turret.transform.Rotate(transform.up, turretRotation * Time.deltaTime);
        // }
        
        // // Check for reload
        // if (reloading)
        // {
        //     Reload();
        // }
        //
        // // Turret Firing
        // if (cooldownTime > 0)
        // {
        //     cooldownTime -= Time.deltaTime;
        // }
        // else if (Input.GetKeyDown(KeyCode.Space) && !reloading)
        // {
        //     var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position,
        //         bulletSpawnPoint.rotation * Quaternion.Euler(90, 0, 0));
        //     bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        //
        //     cooldownTime = 0.5f;
        //     
        //     if (currentAmmo <= 1)
        //     {
        //         currentAmmo--;
        //         Debug.Log("Reloading!");
        //         reloading = true;
        //         reloadTime = 3.0f;
        //
        //     }
        //     else
        //     {
        //         currentAmmo -= 1;
        //     }
        //
        //     _currentAmmoText.text = currentAmmo + "/5";
        // }
        PlayerMovement();
    }
    
    // Function to wait set amount of time before letting player fire again
    // private void Reload()
    // {
    //     // Wait until waitTime is below or equal to zero.
    //     if (reloadTime > 0)
    //     {
    //         reloadTime -= Time.deltaTime;
    //     }
    //     else
    //     {
    //         Debug.Log("Finished Reloading!");
    //         reloading = false;
    //         currentAmmo = 5;
    //         _currentAmmoText.text = currentAmmo + "/5";
    //     }
    // }
}
