using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorFlash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CameraSystem2D.Instance.FlashColour(new Color(1, 0, 0, 0), 1f);
        }
    }
}
