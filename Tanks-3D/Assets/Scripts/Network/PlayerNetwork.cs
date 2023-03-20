using System;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using Quaternion = System.Numerics.Quaternion;
using Random = UnityEngine.Random;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    
    // used to store player input
    // private Vector2 _playerInput;
    private bool _fire;
    
    // store reference to player's character controller to be used to move
    // [SerializeField] private CharacterController controller;

    // values for player's movement speed and rotation speed
    // [SerializeField] private float playerSpeed = 10f;
    // [SerializeField] private float playerRotation = 60f;
    [SerializeField] private float turretRotation = 50f;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float trackSpeed = 0.10f;
    [SerializeField] private GameObject _turret;
    [SerializeField] private GameObject _leftTrack;
    [SerializeField] private GameObject _rightTrack;
    [SerializeField] private GameObject playerCam;
    [SerializeField] private GameObject playHUD;
    [SerializeField] private GameObject camSocket;

    // private PlayerControlActionAsset _playerControlActionAsset;

    // public Transform bulletSpawnPoint;
    //public GameObject bulletPrefab;

    [SerializeField] private GameObject _currentAmmoObject;
    private TextMeshProUGUI _currentAmmoText;
    public int currentAmmo;
    private bool reloading;
    private float reloadTime;
    private float cooldownTime;

    public override void OnNetworkSpawn()
    {
        //_leftTrack = GameObject.Find("Panzer_VI_E_Track_L");
        //_rightTrack = GameObject.Find("Panzer_VI_E_Track_R");
        //_turret = GameObject.Find("Panzer_VI_E_Turret");

        //_currentAmmoObject = GameObject.Find("CurrentAmmo");
        _currentAmmoText = _currentAmmoObject.GetComponent<TextMeshProUGUI>();

        reloading = false;
        reloadTime = 3.0f;
        cooldownTime = 0.5f;
        currentAmmo = 5;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            playerCam.SetActive(false);
            playHUD.SetActive(false);
            camSocket.SetActive(false);
            return;
        }

        // Track Movement
        if (Input.GetKey(KeyCode.W))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = -trackSpeed;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = -trackSpeed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = 0f;
            _rightTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _leftTrack.GetComponent<Scroll_Track>().scrollSpeed = trackSpeed;
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
        
        // // Check for reload
        if (reloading)
        {
            Reload();
        }
        
        // Turret Firing
        if (Input.GetKeyDown(KeyCode.Space) && !reloading)
        {
            // var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position,
            //     bulletSpawnPoint.rotation * Quaternion.Euler(90, 0, 0));
            // bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

            // Transform bulletTransform = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation * UnityEngine.Quaternion.Euler(90, 0, 0));
            // bulletTransform.GetComponent<NetworkObject>().Spawn(true);
            // bulletTransform.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
            
            ShootServerRpc();

            cooldownTime = 0.5f;
            
            if (currentAmmo <= 1)
            {
                currentAmmo--;
                Debug.Log("Reloading!");
                reloading = true;
                reloadTime = 3.0f;
        
            }
            else
            {
                currentAmmo -= 1;
            }
        
            _currentAmmoText.text = currentAmmo + "/5";
        }
        else if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    private void Reload()
    {
        // Wait until waitTime is below or equal to zero.
        if (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Finished Reloading!");
            reloading = false;
            currentAmmo = 5;
            _currentAmmoText.text = currentAmmo + "/5";
        }
    }
    
    [ServerRpc]
    private void ShootServerRpc()
    {
        Transform bulletTransform = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation * UnityEngine.Quaternion.Euler(90, 0, 0));
        bulletTransform.GetComponent<NetworkObject>().Spawn(true);
        bulletTransform.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
    }
}
