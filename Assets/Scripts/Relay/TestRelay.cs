using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{

    #region Singleton

    public static TestRelay Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    private void Start()
    {
        string relayCode = LobbyController.instance.hostLobby.Data["RelayCode"].Value;

        if (LobbyController.instance.isLobbyHost() &&  relayCode == "0")
        {
            StartRelay();
        }

        else if(!LobbyController.instance.isLobbyHost() || relayCode != "0")
        {
            JoinRelay(LobbyController.instance.GetRelayCode());
        }
        
    }

    private async void StartRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Relay Code" + relayCode);

            LobbyController.instance.UpdateRelayCode(relayCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            LobbyController.instance.LeaveLobby();

        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    
    private async void JoinRelay(string JoinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(JoinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            LobbyController.instance.LeaveLobby();
        }

        catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
