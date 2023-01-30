using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameFramework.Manager;
using Unity.VisualScripting;
using UnityEngine;

public class GameLobbyManager : GameFramework.Core.Singleton<GameLobbyManager>
{
    public async Task<bool> CreateLobby()
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            { "PlayerID", "HostPlayer" }
        };
        
        bool success = await LobbyManager.Instance.CreateLobby(2, true, playerData);
        
        return success;
    }
}
