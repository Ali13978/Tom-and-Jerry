using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSwitching : MonoBehaviour
{

    #region Singleton
    public static CharacterSwitching instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] public GameObject JerryArmature;
    [SerializeField] public GameObject TomArmature;
    [SerializeField] CinemachineVirtualCamera PlayerFollowCamera;

    public enum Characters
    {
        Tom,
        Jerry
    }

    public Characters currentCharacter;
    private bool canSwitch = true;

    private void Start()
    {
        StartTom();
    }

    private void Update()
    {
        if (!canSwitch) return;

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCharacter();
        }
    }

    public void SwitchCharacter()
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
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = 4.12f;
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
            (componentBase as Cinemachine3rdPersonFollow).CameraDistance = 2.24f;
        }
    }

    
    public void SetCanSwitch(bool value)
    {
        canSwitch = value;
    }
}
