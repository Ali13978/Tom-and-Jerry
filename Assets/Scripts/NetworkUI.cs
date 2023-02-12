using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button startHostBtn;
    [SerializeField] private Button startServerBtn;
    [SerializeField] private Button startClientBtn;

    private void Awake()
    {
        startHostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        startClientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });

        startServerBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
    }
}
