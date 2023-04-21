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

public class EnemyNetwork : NetworkBehaviour
{
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float shootingRange = 10f;



private Transform _playerTransform;
private bool _hasShotPlayer = false;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        _playerTransform = FindObjectOfType<PlayerNetwork>().transform;

        InvokeRepeating(nameof(ShootPlayer), 2f, 2f);
    }


    private void ShootPlayer()
    {
        if (!IsServer) return;

        if (_playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, _playerTransform.position);

              if (distance < shootingRange && !_hasShotPlayer)
              {
                    _hasShotPlayer = true;
                    ShootServerRpc();
              }
              _hasShotPlayer = false;
    }

    [ServerRpc]
        private void ShootServerRpc()
        {
            Transform bulletTransform = Instantiate(bulletPrefab, bulletSpawnPoint.position,
                bulletSpawnPoint.rotation * UnityEngine.Quaternion.Euler(90, 0, 0));
            bulletTransform.GetComponent<NetworkObject>().Spawn(true);
            bulletTransform.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
}
