using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGMScript : MonoBehaviour
{
    bool gameHasEnded = false;
    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            //pause the game and show the game over menu.
        }
    }

    void Restart()
    {
       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
