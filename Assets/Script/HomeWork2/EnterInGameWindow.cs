using UnityEngine;
using UnityEngine.UI;

public class EnterInGameWindow : MonoBehaviour
{
    [SerializeField] private Button _signInButton;
    [SerializeField] private Button _createAccountButton;
    [SerializeField] private Canvas _enterInGameWindow;
    [SerializeField] private Canvas _createAccountWindow;
    [SerializeField] private Canvas _signInWindow;

    private void Start()
    {
        _signInButton.onClick.AddListener(OpenSignInWindow);
        _createAccountButton.onClick.AddListener(OpenCreateAccountWindow);
    }

    private void OpenCreateAccountWindow()
    {
        _createAccountWindow.enabled = true;
        _enterInGameWindow.enabled = false;
    }

    private void OpenSignInWindow()
    {
        _signInWindow.enabled = true;
        _enterInGameWindow.enabled = false;
    }
}
