using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefabA; //add prefab in inspector
    [SerializeField] private GameObject playerPrefabB; //add prefab in inspector


    public override void OnNetworkSpawn()
    {
        if (IsServer)
            MP_CreatePlayerServerRpc(NetworkManager.Singleton.LocalClientId, 0);
        else
            MP_CreatePlayerServerRpc(NetworkManager.Singleton.LocalClientId, 1);
    }

    [ServerRpc(RequireOwnership = false)] //server owns this object but client can request a spawn
    public void MP_CreatePlayerServerRpc(ulong clientId, int prefabId)
    {
        GameObject newPlayer;

        if (prefabId == 0)
            newPlayer = (GameObject)Instantiate(playerPrefabA);
        else
            newPlayer = (GameObject)Instantiate(playerPrefabB);

        NetworkObject netObj = newPlayer.GetComponent<NetworkObject>();
        newPlayer.SetActive(true);
        netObj.SpawnAsPlayerObject(clientId, true);
    }
}
