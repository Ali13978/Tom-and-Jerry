using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    [SerializeField] LayerMask InteractableObjsLayers;
    [SerializeField] Transform BottomLimit;

    private bool isPressed = false;
    Vector3 ResetPos;

    private void Start()
    {

        ResetPos = transform.position;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Stay");
        if (collision.gameObject.layer == InteractableObjsLayers)
        {
            if(transform.position.y >= BottomLimit.position.y)
            {
                transform.position -= new Vector3(0, 0.1f, 0);
            }

            isPressed = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == InteractableObjsLayers)
        {
            Debug.Log("Exit");
            transform.position = ResetPos;
            isPressed = false;
        }
    }

    public bool GetIsPressed()
    {
        return isPressed;
    }

}
