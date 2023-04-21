using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform[] spawnPoints;

    private void Start()
    {
        if (IsServer)
        {
            // Instantiate 4 player GameObjects at random spawn points
            for (int i = 0; i < 4; i++)
            {
                // Instantiate the player prefab on the server
                GameObject playerGameObject = Instantiate(playerPrefab, spawnPoints[i].position, spawnPoints[i].rotation);

                // Assign the player object to the network
                NetworkObject networkObject = playerGameObject.GetComponent<NetworkObject>();
                networkObject.Spawn();

            }
        }
    }
}
