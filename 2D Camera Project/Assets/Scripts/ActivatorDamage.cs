using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorDamage : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CameraSystem2D.Instance.AssignVignetteValue(damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //CameraSystem2D.Instance.AssignVignetteValue(-damage);
        }
    }
}
