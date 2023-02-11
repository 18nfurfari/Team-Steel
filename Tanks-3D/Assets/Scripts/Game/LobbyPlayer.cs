using GameFramework.Core.Data;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;

        private LobbyPlayerData _data;
        
        public void setData(LobbyPlayerData data)
        {
            _data = data;
            playerName.text = _data.Gamertag;
            gameObject.SetActive(true);
        }
    }
}