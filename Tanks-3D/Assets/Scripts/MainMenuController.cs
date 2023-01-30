using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _joinButton;
    
    void Start()
    {
        _hostButton.onClick.AddListener(OnHostClicked);
        _joinButton.onClick.AddListener(OnJoinClicked);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void OnHostClicked()
    {
        await GameLobbyManager.Instance.CreateLobby();
    }

    private void OnJoinClicked()
    {
        Debug.Log("Join Button Clicked");
    }
}
