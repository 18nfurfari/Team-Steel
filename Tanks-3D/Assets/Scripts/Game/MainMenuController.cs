using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _joinScreen;
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _joinButton;
    
        [SerializeField] private Button _submitJoinCodeButton;
        [SerializeField] private TextMeshProUGUI _joinCodeText;
    
        // enabling listeners when scene changes
        void OnEnable()
        {
            _hostButton.onClick.AddListener(OnHostClicked);
            _joinButton.onClick.AddListener(OnJoinClicked);
            _submitJoinCodeButton.onClick.AddListener(OnSubmitJoinCodeClicked);
        }
    
        // disabling listeners after scene changes
        private void OnDisable()
        {
            _hostButton.onClick.RemoveListener(OnHostClicked);
            _joinButton.onClick.RemoveListener(OnJoinClicked);
            _submitJoinCodeButton.onClick.RemoveListener(OnSubmitJoinCodeClicked);
        }

        private async void OnHostClicked()
        {
            // If lobby creation success, send to lobby scene
            bool success = await GameLobbyManager.Instance.CreateLobby();
            if (success)
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
        }
    
        private void OnJoinClicked()
        {
            _mainMenu.SetActive(false);
            _joinScreen.SetActive(true);
        }
    
        private async void OnSubmitJoinCodeClicked()
        {
            string code = _joinCodeText.text;
            
            code = code.Substring(0, code.Length - 1);
    
            bool success = await GameLobbyManager.Instance.JoinLobby(code);
            if (success)
            {
                SceneManager.LoadSceneAsync("Lobby");
            }
        }
    }
}

