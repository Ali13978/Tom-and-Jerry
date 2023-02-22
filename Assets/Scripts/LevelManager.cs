using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> LevelMaps;

    private void Start()
    {
        if(GetCurrentLevel() <= LevelMaps.Count)
        {
            Instantiate(LevelMaps[GetCurrentLevel() - 1]);
        }

        else
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
            SceneManager.LoadScene(0);
        }
    }

    private int GetCurrentLevel()
    {
        int currentLevel;

        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);

            currentLevel = 1;
        }

        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        }

        Debug.Log(currentLevel);
        return currentLevel;
    }
}
