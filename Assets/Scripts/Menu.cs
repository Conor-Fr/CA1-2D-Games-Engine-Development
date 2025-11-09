using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script is used by the Main menu Scene to start the game when the play button is pressed and quit when the cancel button is pressed
public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Main Game");
    }
    public void Quit()
    {
       Application.Quit(); 
    }
}
