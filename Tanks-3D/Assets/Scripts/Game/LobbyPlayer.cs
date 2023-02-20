using System.Collections.Generic;
using GameFramework.Core.Data;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI readyState;


        private LobbyPlayerData _data;
        
        public void SetData(LobbyPlayerData data)
        {
            _data = data;
            playerName.text = _data.Gamertag;
            
            gameObject.SetActive(true);
        }
    }
}