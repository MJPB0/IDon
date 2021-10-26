using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(RestartButtonClicked);
        optionsButton.onClick.AddListener(OptionsButtonClicked);
        mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
    }

    private void RestartButtonClicked()
    {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        Debug.Log("Restart level [...]");
    }
    private void MainMenuButtonClicked()
    {
        Debug.Log("Load Main Menu [...]");
    }
    private void OptionsButtonClicked()
    {
        Debug.Log("Toggle options menu [...]");
    }
}
