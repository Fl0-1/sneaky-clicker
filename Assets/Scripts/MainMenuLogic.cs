using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject optionsMenu;
    private GameObject loading;
    private GameObject levels;

    public AudioSource buttonSound;



    void Start()
    {
        mainMenu = GameObject.Find("MainMenuCanvas");
        levels = GameObject.Find("LevelsCanvas");
        optionsMenu = GameObject.Find("OptionsCanvas");
        loading = GameObject.Find("LoadingCanvas");

        mainMenu.GetComponent<Canvas>().enabled = true;
        levels.GetComponent<Canvas>().enabled = false;
        optionsMenu.GetComponent<Canvas>().enabled = false;
        loading.GetComponent<Canvas>().enabled = false; 
    }

    public void StartButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        levels.GetComponent<Canvas>().enabled = false;
    }

    public void OptionsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        optionsMenu.GetComponent<Canvas>().enabled = true;
    }

    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit();
        Debug.Log("App Has Exited");
    }

    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = true;
        optionsMenu.GetComponent<Canvas>().enabled = false;
    }

    public void LevelsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        levels.GetComponent<Canvas>().enabled = true;
    }

     public void Level1Button()
    {
        buttonSound.Play();
        loading.GetComponent<Canvas>().enabled = false; 
        SceneManager.LoadScene("SampleScene");
    }

    void Update()
    {
        
    }
}
