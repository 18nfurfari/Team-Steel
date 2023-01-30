using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;
    void Start()
    {
        // TODO: format lobby text correctly...
        _lobbyCodeText.text = $"Lobby code: {GameLobbyManager.Instance.GetLobbyCode()}\n\n\n\n\n\n\n\n\n\n";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
