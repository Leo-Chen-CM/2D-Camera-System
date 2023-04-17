using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem2D : MonoBehaviour
{

    public static CameraSystem2D Instance;

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

    //Camera pan variables
    [SerializeField]
    [Range(1, 50)]
    private int m_cameraOffset = 5;
    private Vector3 m_originalCameraPosition;
    private Vector3 m_mainPlayerPosition;
    Vector3 velocity = Vector3.zero;
    public int m_cameraMoveSpeed;

    //vignette Effects
    [SerializeField]
    [Range(0, 1)]
    private float m_vignetteTransparancey = 0;
    private float m_transparancey = 0;

    public Sprite m_damageScreenSprite;
    private GameObject m_vignetteGameObject;

    //Toggles
    public bool m_toggleSpeedZoomOut;
    public bool m_toggleFadeOut;
    public bool m_toggleCameraFollowPlayer;
    public bool m_toggleCameraPan;
    public bool m_toggleVingetteEffect;
    private bool m_fadeToBlack;

    //Axis locking toggles
    public bool m_lockXAxis;
    private bool m_xAxisLocked;
    public bool m_lockYAxis;
    private bool m_yAxisLocked;
    float m_xAxis;
    float m_yAxis;


    public bool m_toggleCameraZoom;
    public bool m_toggleDebugMode;

    Vector3 m_target ;

    public bool m_looking;


    [SerializeField]
    bool m_usingEffect;
    [SerializeField]
    float m_fieldOfView;
    [SerializeField]
    float m_sensitivity = 10f;
    [SerializeField]
    int m_minZoom = 10;

    [SerializeField]
    int m_maxZoom = 30;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

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
        m_blackOutImage.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        m_blackOutImage.GetComponent<Image>().color = new Color(0,0,0,0);

        m_vignetteGameObject = new GameObject();
        m_vignetteGameObject.transform.SetParent(m_canvas.transform);//Sets the canvas as the parent of the image
        m_vignetteGameObject.transform.localScale = Vector3.one;
        m_vignetteGameObject.AddComponent<Image>();
        m_vignetteGameObject.name = "VignetteScreen";
        m_vignetteGameObject.GetComponent<Image>().sprite = m_damageScreenSprite;
        m_vignetteGameObject.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0f, 0f);
        m_vignetteGameObject.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        m_vignetteGameObject.GetComponent<Image>().color = new Color
            (
            m_vignetteGameObject.GetComponent<Image>().color.r,
            m_vignetteGameObject.GetComponent<Image>().color.g,
            m_vignetteGameObject.GetComponent<Image>().color.b,
            0);

        m_mainPlayer = GameObject.FindGameObjectWithTag("Player");

        m_originalCameraPosition = Camera.main.transform.position;
        m_originalCameraSize = Camera.main.orthographicSize;


        m_mainPlayerPosition = new Vector3(m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
        m_target = new Vector3(m_originalCameraPosition.x, m_cameraOffset + m_originalCameraPosition.y, m_originalCameraPosition.z);
    }

    public GameObject ReturnBlackOutImage()
    {
        return m_blackOutImage;
    }

    // Update is called once per frame
    void Update()
    {

        if (m_toggleCameraFollowPlayer)
        {
            if (m_mainPlayer == null)
            {
                Debug.LogError("Main player can not be found, add the Player tag to the main player.");
            }
            else
            {
                //transform.position = new Vector3(m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
                MoveCameraBackToPlayer();
                if (m_toggleSpeedZoomOut)
                {
                    SpeedZoom();
                }              
            }
        }


        if (m_toggleCameraPan)
        {
            CameraLookKeys();
        }

        if (m_toggleFadeOut)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                FadeInOut();
            }
        }

        if (m_toggleVingetteEffect)
        {
            VignetteEffect();
        }

        if (m_toggleCameraZoom)
        {
            CameraZoom();
        }

        m_mainPlayerPosition = new Vector3(m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
        m_originalCameraPosition = Camera.main.transform.position;

        if (m_lockXAxis)
        {
            LockXAxis();
            Camera.main.transform.position = new Vector3(m_xAxis, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        else
        {
            m_xAxisLocked = false;
        }

        if (m_lockYAxis)
        {
            LockYAxis();
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, m_yAxis, Camera.main.transform.position.z);
        }
        else
        {
            m_yAxisLocked = false;
        }
    }

    void LockXAxis()
    {
        if (!m_xAxisLocked)
        {
            m_xAxis = m_originalCameraPosition.x;
            m_xAxisLocked = true;
        }

    }
    
    void LockYAxis()
    {
        if (!m_yAxisLocked)
        {
            m_yAxis = m_originalCameraPosition.y;
            m_yAxisLocked = true;
        }
    }

    private void VignetteEffect()
    {
        //float transparancey = t_number / 255;

        //m_vignetteTransparancey = transparancey;

        m_vignetteGameObject.GetComponent<Image>().color = new Color
            (
            m_vignetteGameObject.GetComponent<Image>().color.r,
            m_vignetteGameObject.GetComponent<Image>().color.g,
            m_vignetteGameObject.GetComponent<Image>().color.b,
            m_vignetteTransparancey);
    }
    public void AssignVignetteValue(float t_value)
    {
        //float transparancey = t_value;
        //m_vignetteTransparancey = transparancey / 100;

        m_transparancey += t_value;

        m_vignetteTransparancey = m_transparancey / 100;

    }

    /// <summary>
    /// Pressing pre-defined keys will have the camera pan to X position away from the player
    /// </summary>
    private void CameraLookKeys()
    {
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            m_toggleCameraFollowPlayer = false;
            m_usingEffect = true;
            m_target = new Vector3(m_mainPlayer.transform.position.x, m_cameraOffset + m_mainPlayer.transform.position.y, -10);
            transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            m_toggleCameraFollowPlayer = false;
            m_usingEffect = true;
            m_target = new Vector3(m_mainPlayer.transform.position.x, -m_cameraOffset + m_mainPlayer.transform.position.y, -10);
            transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            m_toggleCameraFollowPlayer = false;
            m_usingEffect = true;
            m_target = new Vector3(-m_cameraOffset + m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
            transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            m_toggleCameraFollowPlayer = false;
            m_usingEffect = true;
            m_target = new Vector3(m_cameraOffset + m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
            transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
        }
        else
        {
            m_toggleCameraFollowPlayer = true;
            MoveCameraBackToPlayer();
        }
    }
    private void CameraZoom()
    {
        m_fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * m_sensitivity;
        m_fieldOfView = Mathf.Clamp(m_fieldOfView, m_minZoom, m_maxZoom);
        Camera.main.orthographicSize = m_fieldOfView;
    }

    void MoveCameraBackToPlayer()
    {
        if (m_usingEffect)
        {
            if (Vector2.Distance(transform.position, m_mainPlayerPosition) > 0.5f)
            {
                transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_mainPlayerPosition, ref velocity, (m_cameraMoveSpeed / 2) * Time.deltaTime);
            }
            else
            {
                m_usingEffect = false;
            }
        }
        else
        {
            transform.position = new Vector3(m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
        }
    }

    private void SpeedZoom()
    {
        Vector2 vel = new Vector2( m_mainPlayer.GetComponent<Rigidbody2D>().velocity.x, 0.0f);
        m_currentSpeed = vel.magnitude;
        if (m_currentSpeed > 0.5f && Camera.main.orthographicSize < m_maxSize)
        {
            //Camera.main.orthographicSize += m_originalCameraSize * (m_currentSpeed/2);
            Camera.main.orthographicSize +=  0.01f * (m_currentSpeed / 2);
        }
        else if (Camera.main.orthographicSize > m_maxSize)
        {
            Camera.main.orthographicSize = m_maxSize;
        }
        else
        {
            if (Camera.main.orthographicSize > m_originalCameraSize)
            {
                Camera.main.orthographicSize -= 0.02f;
            }

            if (Camera.main.orthographicSize < m_originalCameraSize)
            {
                Camera.main.orthographicSize = m_originalCameraSize;
            }
        }
    }

    public void FadeInOut()
    {
        StopAllCoroutines();
        m_fadeToBlack = !m_fadeToBlack;
        StartCoroutine(FadeToBlack(m_fadeToBlack, m_fadeoutSpeed));
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
