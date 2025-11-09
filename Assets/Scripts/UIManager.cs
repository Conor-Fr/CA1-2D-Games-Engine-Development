using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//This script is used by the UI in the Main Game to control the Counter on the top left to let the player know how many diamonds still need to be collected.
//It is also used to control the hearts we have on the top right letting the player know how many lives they have left.
public class UIManager : MonoBehaviour
{
    public Text CollectedAmountText;
    public Image[] images;

    public void SetScore(int score)
    {
        CollectedAmountText.text = score.ToString();
    }

    //This code is triggered in the Player script whenever the player is hit
    public void UpdateLives(int lives)
    {
        //If the lives amount is 5 it means all hearts are displayed and the player has not yet been hit.
        if (lives == 5)
        {
            images[0].enabled = true;
            images[1].enabled = true;
            images[2].enabled = true;
            images[3].enabled = true;
            images[4].enabled = true;
        }
        //If the player is hit once decreasing the lives amount 4 of the hearts are shown.
        else if (lives == 4)
        {
            images[0].enabled = true;
            images[1].enabled = true;
            images[2].enabled = true;
            images[3].enabled = true;
            images[4].enabled = false;
        }
        //If the player is hit yet again the lives amount is reduced again by 1 and 3 of the hearts are still displayed
        else if (lives == 3)
        {
            images[0].enabled = true;
            images[1].enabled = true;
            images[2].enabled = true;
            images[3].enabled = false;
            images[4].enabled = false;
        }
        //If the player is hit for the thrd time then the lives counter is reduced again by 1 and 2 hearts remain displayed
        else if (lives == 2)
        {
            images[0].enabled = true;
            images[1].enabled = true;
            images[2].enabled = false;
            images[3].enabled = false;
            images[4].enabled = false;
        }
        //If the player is hit a fourth time they will be down to their last life and only 1 single heart will still be displayed
        else if (lives == 1)
        {
            images[0].enabled = true;
            images[1].enabled = false;
            images[2].enabled = false;
            images[3].enabled = false;
            images[4].enabled = false;
        }
        //Finally the player has been hit for the final time and all the hearts are no longer displayed
        else if (lives == 0)
        {
            images[0].enabled = false;
            images[1].enabled = false;
            images[2].enabled = false;
            images[3].enabled = false;
            images[4].enabled = false;
        }
    }
}
