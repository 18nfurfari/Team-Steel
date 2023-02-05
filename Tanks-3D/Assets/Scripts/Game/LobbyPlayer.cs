using GameFramework.Core.Data;
using TMPro;
using UnityEngine;

namespace Game
{
    public class LobbyPlayer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerName;

        private LobbyPlayerData data;
        
        public void setData(LobbyPlayerData data)
        {
            this.data = data;
            playerName.text = data.Gamertag;
            gameObject.SetActive(true);
        }
    }
}