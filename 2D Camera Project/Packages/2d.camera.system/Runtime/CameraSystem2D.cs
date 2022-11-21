using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem2D : MonoBehaviour
{
    //Speed zoom out related
    private float m_currentSpeed;
    private float m_maxSize;
    private float m_originalCameraSize;

    private GameObject m_mainPlayer;

    //Fade out related member variables
    [SerializeField]
    [Range(1, 10)]
    private int m_fadeoutSpeed = 5;
    private GameObject m_gameObject;
    private Canvas m_canvas;
    private GameObject m_blackOutImage;

    //Toggles
    public bool m_toggleSpeedZoomOut;
    public bool m_toggleFadeOut;


    private bool m_fadeToBlack;
    // Start is called before the first frame update
    void Start()
    {
        m_maxSize = 20;

        //Canvas
        m_gameObject = new GameObject();
        m_gameObject.name = "Canvas";
        m_gameObject.transform.SetParent(Camera.main.transform);
        m_gameObject.AddComponent<Canvas>();
        m_gameObject.AddComponent<CanvasScaler>();
        m_gameObject.AddComponent<GraphicRaycaster>();
        m_canvas = m_gameObject.GetComponent<Canvas>();
        m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //Fade out image
        m_blackOutImage = new GameObject();
        m_blackOutImage.transform.SetParent(m_canvas.transform);//Sets the canvas as the parent of the image
        m_blackOutImage.transform.localScale = Vector3.one;
        m_blackOutImage.AddComponent<Image>();
        m_blackOutImage.name = "FadeOutScreen";
        m_blackOutImage.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0f, 0f);
        m_blackOutImage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(1200, 600);
        m_blackOutImage.GetComponent<Image>().color = new Color(0,0,0,0);

        m_mainPlayer = GameObject.FindGameObjectWithTag("Player");

        m_originalCameraSize = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_mainPlayer == null)
        {
            Debug.LogError("Main player can not be found");
        }
        else
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
                        StartCoroutine(FadeToBlack(m_fadeToBlack,m_fadeoutSpeed));
                    }

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
            Camera.main.orthographicSize += m_originalCameraSize * (m_currentSpeed/2);
            //Camera.main.orthographicSize +=  0.01f;
        }
        //else if (Camera.main.orthographicSize > m_maxSize)
        //{
        //    Camera.main.orthographicSize = m_maxSize;
        //}
        else
        {
            if (Camera.main.orthographicSize > m_originalCameraSize)
            {
                Camera.main.orthographicSize -= 0.01f;
            }
        }
    }


    IEnumerator FadeToBlack(bool t_fadeToBlack = true, int t_fadeSpeed = 5)
    {
        Color fadeColor = m_blackOutImage.GetComponent<Image>().color;
        float fadeAmount;

        if (t_fadeToBlack)
        {
            while (m_blackOutImage.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = fadeColor.a + (t_fadeSpeed * Time.deltaTime);

                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                m_blackOutImage.GetComponent<Image>().color = fadeColor;
                yield return null;
            }
        }
        else
        {
            while (m_blackOutImage.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = fadeColor.a - (t_fadeSpeed * Time.deltaTime);

                fadeColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, fadeAmount);
                m_blackOutImage.GetComponent<Image>().color = fadeColor;
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
