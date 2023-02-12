using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSwitching : MonoBehaviour
{
    [SerializeField] GameObject JerryArmature;
    [SerializeField] GameObject TomArmature;
    [SerializeField] CinemachineVirtualCamera PlayerFollowCamera;

    private enum Characters
    {
        Tom,
        Jerry
    }

    [SerializeField] Characters currentCharacter;

    private void Start()
    {
        StartTom();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        if(currentCharacter == Characters.Tom)
        {
            StartJerry();
        }

        else if(currentCharacter == Characters.Jerry)
        {
            StartTom();
        }
    }

    private void StartTom()
    {
        currentCharacter = Characters.Tom;

        JerryArmature.GetComponent<StarterAssets.StarterAssetsInputs>().SetCanInput(false);
        TomArmature.GetComponent<StarterAssets.StarterAssetsInputs>().SetCanInput(true);

        PlayerFollowCamera.Follow = TomArmature.transform.Find("PlayerCameraRoot");

        CinemachineComponentBase componentBase = PlayerFollowCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = 2.46f;
        }
    }

    private void StartJerry()
    {
        currentCharacter = Characters.Jerry;

        JerryArmature.GetComponent<StarterAssets.StarterAssetsInputs>().SetCanInput(true);
        TomArmature.GetComponent<StarterAssets.StarterAssetsInputs>().SetCanInput(false);

        PlayerFollowCamera.Follow = JerryArmature.transform.Find("PlayerCameraRoot");

        CinemachineComponentBase componentBase = PlayerFollowCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow)
        {
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = 1.53f;
        }
    }
}
