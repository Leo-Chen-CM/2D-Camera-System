using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorRotate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CameraSystem2D.Instance.CallRotateCamera(0.5f, 180);
        }
    }
}
