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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void LoadSinglePlayerScene()
    {
        Loadingcanvas.instance.UpdateLoadingText("Loading game level");
        Loadingcanvas.instance.StartCanvasTimer(3f);
        SceneManager.LoadScene(1);
    }

    public void LoadPrototyping()
    {
        Loadingcanvas.instance.UpdateLoadingText("Loading prototyping scene");
        Loadingcanvas.instance.StartCanvasTimer(2f);
        SceneManager.LoadScene(2);
    }
}
