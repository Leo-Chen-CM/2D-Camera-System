using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorDrunk : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CameraSystem2D.Instance.RandomMovementCameraEffect(4,5, 90);
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        CameraSystem2D.Instance.FadeInOut();
    //    }
    //}
}
