using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGroundPointCollisionDetector : MonoBehaviour
{
    private PlayerController playerController;
    private void Start()
    {
        playerController = gameObject.GetComponentInParent(typeof(PlayerController)) as PlayerController;
    }

    private void OnTriggerStay(Collision collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.CallOnCollisionsWithGround();
        }

    }

    private void OnTriggerExit(Collision collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.CallOnCollisionExitWithGround();
        }
    }
}
