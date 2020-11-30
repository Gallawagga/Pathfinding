using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GOMenu : MonoBehaviour
{

    bool gameHasEnded = false;
    public GameObject GOMenuUI;

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            GOMenuUI.SetActive(true); //setting the UI for the menu to be visible. 
            Time.timeScale = 0f;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGameFromGO()
    {
        Debug.Log("Quitted");
        Application.Quit();
    }

}
