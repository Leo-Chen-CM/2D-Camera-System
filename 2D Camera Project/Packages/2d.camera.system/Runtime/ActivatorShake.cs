using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorShake : MonoBehaviour
{
    [SerializeField]
    bool m_trembleSmall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int randomInt = Random.Range(0, 1);

            if (randomInt == 0)
            {
                m_trembleSmall = false;
            }
            else
            {
                m_trembleSmall = true;
            }

            CameraSystem2D.Instance.EnableTremble(m_trembleSmall);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }
}
