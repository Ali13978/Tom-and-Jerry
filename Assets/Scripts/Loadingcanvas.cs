using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loadingcanvas : MonoBehaviour
{

    #region Singleton
    public static Loadingcanvas instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] private TMP_Text LoadingText;

    private float Counter = 0;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Counter >= 0)
        {
            Counter -= Time.deltaTime;

            if(Counter < 0)
            {
                gameObject.SetActive(false);
                ResetLoadingText();
            }
        }
    }

    private void ResetLoadingText()
    {
        LoadingText.text = "Loading...";
    }

    public void UpdateLoadingText(string Message)
    {
        LoadingText.text = Message;
    }

    public void StartCanvasTimer(float Timer)
    {
        Counter = Timer;
        gameObject.SetActive(true);

        Debug.Log("Canvas On");
    }


}
