using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    private async void Start()
    {
        // await UnityServices.InitializeAsync();
        //
        // await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // private async void CreateRelay()
    // {
    //     try
    //     {
    //         Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
    //
    //         string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
    //         
    //         Debug.Log(joinCode);
    //
    //         RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
    //         
    //         NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
    //
    //         NetworkManager.Singleton.StartHost();
    //     }
    //     catch (RelayServiceException e)
    //     {
    //         Debug.Log(e);
    //     }
    // }

    public async Task<string> CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    public static RelayManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
