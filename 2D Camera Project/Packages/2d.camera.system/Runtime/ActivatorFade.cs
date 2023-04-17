using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorFade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CameraSystem2D.Instance.FadeInOut();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CameraSystem2D.Instance.FadeInOut();
        }
    }
}
