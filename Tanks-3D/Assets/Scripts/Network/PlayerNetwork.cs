using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject enemyPrefab1;
    [SerializeField] private GameObject enemyPrefab2;
    [SerializeField] private GameObject enemyPrefab3;
    [SerializeField] private GameObject enemyPrefab4;

// used to store player input
    // private Vector2 _playerInput;
    private bool _fire;
    
    // store reference to player's character controller to be used to move
    // [SerializeField] private CharacterController controller;

    // values for player's movement speed and rotation speed
    // [SerializeField] private float playerSpeed = 10f;
    // [SerializeField] private float playerRotation = 60f;
    [SerializeField] private float turretRotation = 60f;
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

    public float playerHealth;

    [SerializeField] private GameObject _currentAmmoObject;
    private TextMeshProUGUI _currentAmmoText;
    public int currentAmmo;
    private bool reloading;
    private float reloadTime;
    private float cooldownTime;

    [SerializeField] private GameObject _blackSmoke;
    

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
        playerHealth = 5.0f;
        //SetSpawnPoints();
        
        GameObject spawnedEnemy1 = Instantiate(enemyPrefab1);
        spawnedEnemy1.GetComponent<NetworkObject>().Spawn(true);
        GameObject spawnedEnemy2 = Instantiate(enemyPrefab2);
        spawnedEnemy2.GetComponent<NetworkObject>().Spawn(true);
        GameObject spawnedEnemy3 = Instantiate(enemyPrefab3);
        spawnedEnemy3.GetComponent<NetworkObject>().Spawn(true);
        GameObject spawnedEnemy4 = Instantiate(enemyPrefab4);
        spawnedEnemy4.GetComponent<NetworkObject>().Spawn(true);
        
        StartCoroutine(SpawnPlayers());
        
    }

    private void Update()
    {
        if (!IsOwner)
        {
            playerCam.SetActive(false);
            playHUD.SetActive(false);
            camSocket.SetActive(false);
            _blackSmoke.SetActive(false);
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
            float rotation = -1 * turretRotation * Time.deltaTime;
            TurretUpdateServerRpc(rotation);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float rotation = turretRotation * Time.deltaTime;
            TurretUpdateServerRpc(rotation);
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

    [ServerRpc]
    private void TurretUpdateServerRpc(float rotation)
    {
        TurretUpdateClientRpc(rotation);
    }

    [ClientRpc]
    private void TurretUpdateClientRpc(float rotation)
    {
        _turret.transform.Rotate(transform.up, rotation);
    }
    
    // [ServerRpc]
    // private void BlackSmokeVisibleServerRpc(bool visible)
    // {
    //     BlackSmokeVisibleClientRpc(visible);
    // }
    //
    // [ClientRpc]
    // private void BlackSmokeVisibleClientRpc(bool visible)
    // {
    //     _blackSmoke.SetActive(visible);
    // }
    
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 1.0f && playerHealth > 0) // if player is 1 shot from death
        {
            // if (IsOwner)
            // {
            //     BlackSmokeVisibleServerRpc(true);
            // }
            // _blackSmoke.SetActive(true);
        }
        else if (playerHealth <= 0) // player health hits 0 and dies
        {
            // Player is dead
            Destroy(gameObject);
            // if (IsOwner)
            // {
            //     BlackSmokeVisibleServerRpc(false);
            // }
            // _blackSmoke.SetActive(false);
        }
    }
    
    // Spawn point list 
    // Vector3(-22.3999996, 0, -100.900002)
    // Vector3(-51.2999992,0,90.3000031)
    // Vector3(48.9000015,0,78.8000031)
    // Vector3(91.3000031,0,-97.6999969)

    public int nextSpawnPointIndex = 0;
    IEnumerator SpawnPlayers()
    {
        // Wait until all players are loaded
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length >= 1);

        // Get an array of all GameObjects tagged as "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Get an array of all GameObjects tagged as "PlayerSpawn"
        GameObject[] spawnPointsObjects = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        Transform[] spawnPoints = new Transform[spawnPointsObjects.Length];

        for (int i = 0; i < spawnPointsObjects.Length; i++)
        {
            spawnPoints[i] = spawnPointsObjects[i].transform;
        }
        
        // Shuffle the spawn points
        // ShuffleArray(spawnPoints);

        // Get the max number of players between 1 and 4
        int maxPlayers = Mathf.Clamp(players.Length, 1, 4);
        
        if (maxPlayers > spawnPoints.Length) {
            Debug.LogError("Not enough spawn points for all players!");
            yield break; // Exit the coroutine early
        }
        
        // Move each player to its corresponding spawn point
        for (int i = 0; i < maxPlayers; i++)
        {
            GameObject player = players[i];

            Transform spawnPoint = spawnPoints[nextSpawnPointIndex];
            
            // Set the spawn point for the player
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;

            nextSpawnPointIndex = (nextSpawnPointIndex + 1) % spawnPoints.Length;
        }
    }

    void ShuffleArray<T>(T[] array)
     {
         // Fisher-Yates shuffle algorithm
         for (int i = array.Length - 1; i > 0; i--)
         {
             int j = UnityEngine.Random.Range(0, i + 1);
             (array[i], array[j]) = (array[j], array[i]);
         }
     }
}
