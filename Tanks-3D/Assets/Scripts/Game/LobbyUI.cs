using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace Game
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lobbyCodeText;
        [SerializeField] private Button _readyButton;

        private void OnEnable()
        {
            //_readyButton.onClick.AddListener(OnReadyPressed);
        }

        private void OnDisable()
        {
            //_readyButton.onClick.RemoveAllListeners();
        }

        void Start()
        {
            // TODO: format lobby text correctly...
            _lobbyCodeText.text = $"Lobby code: {GameLobbyManager.Instance.GetLobbyCode()}";
        }

        private async void OnReadyPressed()
        {
            bool succeed = await GameLobbyManager.Instance.SetPlayerReady();

            if (succeed)
            {
                _readyButton.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

