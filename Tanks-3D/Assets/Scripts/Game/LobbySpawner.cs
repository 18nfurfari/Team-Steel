using System.Collections.Generic;
using GameFramework.Core.Data;
using UnityEngine;

namespace Game
{
    public class LobbySpawner :MonoBehaviour
    {
        [SerializeField] private List<LobbyPlayer> players;

        private void OnEnable()
        {
            Events.LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            Events.LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }
        
        private void OnLobbyUpdated()
        {
            List<LobbyPlayerData> playerData = GameLobbyManager.Instance.GetPlayers();

            for (int x = 0; x < playerData.Count; x++)
            {
                LobbyPlayerData data = playerData[x];
                players[x].setData(data);
            }
        }
    }
}