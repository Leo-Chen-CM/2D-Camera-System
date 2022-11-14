using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem2D : MonoBehaviour
{
    private float m_currentSpeed;
    private float m_maxSize;
    private GameObject m_mainPlayer;
    private float m_originalCameraSize;
    public bool m_toggleSpeedZoomOut;
    public bool m_toggleFadeOut;
    private bool m_fadeToBlack;
    private GameObject m_blackOutScreen;
    // Start is called before the first frame update
    void Start()
    {
        m_maxSize = 20;
        m_mainPlayer = GameObject.FindGameObjectWithTag("Player");
        m_blackOutScreen = GameObject.FindGameObjectWithTag("FadeScreen");
        m_originalCameraSize = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3( m_mainPlayer.transform.position.x , m_mainPlayer.transform.position.y, -10);
        if (m_toggleSpeedZoomOut)
        {
            SpeedZoom();
        }

        if (m_toggleFadeOut)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (true)
                {
                    StopAllCoroutines();
                    m_fadeToBlack = !m_fadeToBlack;
                    StartCoroutine(FadeToBlack(m_fadeToBlack));
                }

            }
        }
    }

    void SpeedZoom()
    {
        Vector2 vel = m_mainPlayer.GetComponent<Rigidbody2D>().velocity;
        m_currentSpeed = vel.magnitude;
        if (m_currentSpeed > 0 && Camera.main.orthographicSize < m_maxSize)
        {
            Camera.main.orthographicSize = m_originalCameraSize + (m_currentSpeed * 0.5f);
        }
        else if (Camera.main.orthographicSize >= m_maxSize)
        {
            Camera.main.orthographicSize = m_maxSize;
        }


        //else
        //{
        //    Camera.main.orthographicSize = m_originalCameraSize;
        //}
    }


    IEnumerator FadeToBlack(bool t_fadeToBlack = true, int t_fadeSpeed = 5)
    {
        Color fadeColor = m_blackOutScreen.GetComponent<Image>().color;
        float fadeAmount;

        if (t_fadeToBlack)
        {
            while (m_blackOutScreen.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = fadeColor.a + (t_fadeSpeed * Time.deltaTime);

                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                m_blackOutScreen.GetComponent<Image>().color = fadeColor;
                yield return null;
            }
        }
        else
        {
            while (m_blackOutScreen.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = fadeColor.a - (t_fadeSpeed * Time.deltaTime);

                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                m_blackOutScreen.GetComponent<Image>().color = fadeColor;
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
