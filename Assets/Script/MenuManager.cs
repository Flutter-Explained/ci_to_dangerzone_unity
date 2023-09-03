using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainMenuCanvasGO;
    [SerializeField] private GameObject _settingsMenuCanvasGO;
    private bool isPaused;

    [Header("First Selected Options ")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    [SerializeField] private GameObject _startGameFirst;


    // Start is called before the first frame update
    void Start()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingsMenuCanvasGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.instance.MenuOpenCloseInput)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }

    }

    #region Pause / Unpause
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        CloseAllMenus();
    }
    #endregion

    #region Canvas Activations
    private void OpenMainMenu()
    {
        _mainMenuCanvasGO.SetActive(true);
        _settingsMenuCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    private void CloseAllMenus()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingsMenuCanvasGO.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_startGameFirst);
    }

    private void OpenSettingsMenuHandle()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingsMenuCanvasGO.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
    }
    #endregion

    #region Main Menu Button actions
    public void OnSettingsPress()
    {
        OpenSettingsMenuHandle();
    }

    public void OnResumePress()
    {
        Unpause();
    }

    public void OnExitGame(){
        Application.Quit();
    }
    #endregion

    #region Settings Menu Button actions

    public void OnSettingsBackPress()
    {
        OpenMainMenu();
    }

    #endregion
}
