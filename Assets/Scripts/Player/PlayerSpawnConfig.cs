using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerSpawnConfig : NetworkBehaviour
{
    CinemachineVirtualCamera PlayerFollowCamera;

    [Tooltip("Distance from virtual camera")]
    [SerializeField] float CameraDistance;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        
        PlayerFollowCamera = FindObjectOfType<CinemachineVirtualCamera>();

        PlayerFollowCamera.Follow = transform.Find("PlayerCameraRoot");

        CinemachineComponentBase componentBase = PlayerFollowCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = CameraDistance;
        }


        Camera.main.gameObject.SetActive(true);
        PlayerFollowCamera.gameObject.SetActive(true);
    }

}
