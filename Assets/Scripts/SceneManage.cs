using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{

    #region Singleton

    public static SceneManage instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadSinglePlayerScene()
    {
        SceneManager.LoadScene(2);
    }
}
