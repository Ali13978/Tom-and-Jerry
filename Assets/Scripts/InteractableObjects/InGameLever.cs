using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class InGameLever : MonoBehaviour
{
    private ThirdPersonController ThirdPersonController;
    private Animator Anim;
    [SerializeField] private LayerMask Layers;

    private bool isLeverOn = false;

    private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (CharacterSwitching.instance.currentCharacter == CharacterSwitching.Characters.Tom)
            ThirdPersonController = CharacterSwitching.instance.TomArmature.GetComponent<ThirdPersonController>();

        else if (CharacterSwitching.instance.currentCharacter == CharacterSwitching.Characters.Jerry)
            ThirdPersonController = CharacterSwitching.instance.JerryArmature.GetComponent<ThirdPersonController>();

        if(ThirdPersonController.CastRay(Layers).collider.gameObject == gameObject)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                FlipLever();
            }
        }
    }

    private void FlipLever()
    {
        if (isLeverOn)
        {
            isLeverOn = false;
            Anim.SetBool("LeverOn", false);
        }
        else
        {
            isLeverOn = true;
            Anim.SetBool("LeverOn", true);
        }
    }

    public bool GetLeverValue()
    {
        return isLeverOn;
    }

}
