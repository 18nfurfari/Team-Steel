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
            List<LobbyPlayerData> playersData = GameLobbyManager.Instance.GetPlayers();

            for (int x = 0; x < playersData.Count; x++)
            {
                LobbyPlayerData data = playersData[x];
                players[x].SetData(data);
            }
        }
    }
}