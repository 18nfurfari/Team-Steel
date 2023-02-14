using GameFramework.Core.Data;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI readyState;
        
        private LobbyPlayerData _data;
        
        public void setData(LobbyPlayerData data)
        {
            _data = data;
            //playerName.text = _data.Gamertag;
            playerName.text = "Player";
            
            if (_data.IsReady)
            {
                readyState.text = "Ready";
            }

            if (!_data.IsReady)
            {
                readyState.text = "Not Ready";
            }

            gameObject.SetActive(true);
        }
    }
}