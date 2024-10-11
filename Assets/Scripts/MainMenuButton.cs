using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ReturnToMainMenuButton()
    {     // load scene main menu
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Return to Main Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
