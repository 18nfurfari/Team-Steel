using System;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform spawnedObjectPrefab;

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
        
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        
    }
}
