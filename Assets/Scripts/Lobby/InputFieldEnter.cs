using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InputFieldEnter : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button StartGame;

    private void Start()
    {
        // Add a listener for the onEndEdit event
        inputField.onValueChanged.AddListener(HandleInputSubmit);
    }

    private void HandleInputSubmit(string inputValue)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame.onClick.Invoke();
        }
    }
}
