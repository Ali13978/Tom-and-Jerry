using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class CheckWin : MonoBehaviour
{
    bool canIncrement = true;
    int NumberOfPlayersWon = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered");

            NumberOfPlayersWon++;

            CharacterSwitching.instance.SwitchCharacter();
            CharacterSwitching.instance.SetCanSwitch(false);

            if(NumberOfPlayersWon >= 2)
            {
                Won(1);
            }
        }
    }

    private void Won(int SceneNumber)
    {
        if (!canIncrement) return;

        canIncrement = false;
        PlayerPrefs.SetInt("CurrentLevel", (PlayerPrefs.GetInt("CurrentLevel") + 1));
        Loadingcanvas.instance.StartCanvasTimer(3);
        SceneManager.LoadScene(SceneNumber);
    }
}
