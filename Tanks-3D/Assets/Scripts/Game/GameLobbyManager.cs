using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameFramework.Core.Data;
using GameFramework.Events;
using GameFramework.Manager;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;


namespace Game
{
    public class GameLobbyManager : GameFramework.Core.Singleton<GameLobbyManager>
    {
        private List<LobbyPlayerData> lobbyPlayerData = new List<LobbyPlayerData>();
        private LobbyPlayerData localLobbyPlayerData;
    
        private void OnEnable()
        {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }
    
        private void OnDisable()
        {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }
    
        private void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.Instance.GetPlayersData();
            
            lobbyPlayerData.Clear();
    
            foreach (Dictionary<string, PlayerDataObject> data in playerData)
            {
                LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Initialize(data);
    
                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    localLobbyPlayerData = lobbyPlayerData;
                }
    
                this.lobbyPlayerData.Add(localLobbyPlayerData);
            }
            
            Events.LobbyEvents.OnLobbyUpdated?.Invoke();
        }
    
        public async Task<bool> CreateLobby()
        {
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, gamertag: "HostPlayer");
            bool success = await LobbyManager.Instance.CreateLobby(4, true, playerData.Serialize());
            return success;
        }
    
        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }
    
        public async Task<bool> JoinLobby(string code)
        {
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, gamertag: "JoinPlayer");
            bool success = await LobbyManager.Instance.JoinLobby(code, playerData.Serialize());
            return success;
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return lobbyPlayerData;
        }

        public async Task<bool> SetPlayerReady()
        {
            localLobbyPlayerData.IsReady = true;
            return await LobbyManager.Instance.UpdatePlayerData(localLobbyPlayerData.Id, localLobbyPlayerData.Serialize());
        }
    }
}

