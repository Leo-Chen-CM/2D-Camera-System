using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSystem2D : MonoBehaviour
{

    public static CameraSystem2D Instance;
    bool m_usingEffect;
    //Speed zoom out related
    private float m_currentSpeed;
    private float m_originalCameraSize;

    public GameObject m_mainPlayer;
    public GameObject m_pointOfFocus;
    //Toggles
    [Header("Camera Toggles")]
    public bool m_toggleSpeedZoomOut;
    public bool m_toggleFadeOut;
    public bool m_toggleCameraFollowPlayer;
    public bool m_toggleCameraPan;
    public bool m_toggleVingetteEffect;
    private bool m_fadeToBlack;
    public bool m_toggleShake;
    public bool m_toggleCameraZoom;
    public bool m_toggleDebugMode;
    public bool m_toggleCameraDirectionalFacing;
    public bool m_toggleCameraHeightZoom;
    public bool m_togglePointOfFocus;
    //Fade out related member variables
    [Header("Camera Fade in/out variables")]
    [SerializeField]
    [Range(1, 10)]
    private int m_fadeoutSpeed = 5;

    private GameObject m_gameObject;
    private Canvas m_canvas;
    private GameObject m_blackOutImage;

    //Camera pan variables
    [Header("Camera Move/Pan Factors")]
    [SerializeField]
    [Range(1, 50)]
    private int m_cameraOffset = 5;
    private Vector3 m_originalCameraPosition;
    private Vector3 m_mainPlayerPosition;
    Vector3 velocity = Vector3.zero;
    public int m_cameraMoveSpeed;

    KeyCode m_lookHold;
    KeyCode m_lookUp;
    KeyCode m_lookDown;
    KeyCode m_lookLeft;
    KeyCode m_lookRight;

    [Header("Camera Vignette Effects")]
    //vignette Effects
    [Tooltip("Transparency Value of the image from 0 to 1")]
    [SerializeField]
    [Range(0, 1)]
    private float m_vignetteTransparancey = 0;
    private float m_transparancey = 0;

    [Tooltip("Damage Overlay Sprite")]
    public Sprite m_damageScreenSprite;
    private GameObject m_vignetteGameObject;


    [Header("Camera Axis Locking")]
    //Axis locking toggles
    public bool m_lockXAxis;
    private bool m_xAxisLocked;
    float m_xAxis;

    public bool m_lockYAxis;
    private bool m_yAxisLocked;
    float m_yAxis;



    [Header("Camera Direction Variables")]
    //Camera direction facing variables
    [SerializeField]
    private float m_facingTargetOffset;
    [SerializeField]
    private float m_cameraSmoothSpeed;
    Vector3 m_target;
    public bool m_lookingRight;
    public bool m_lockDirection;

    [Header("Camera Zoom Variables")]
    [SerializeField]
    float m_fieldOfView = 5;
    [SerializeField]
    float m_sensitivity = 10f;
    [SerializeField]
    int m_minZoom = 5;
    [SerializeField]
    int m_maxZoom = 30;

    KeyCode m_zoomIn;
    KeyCode m_zoomOut;

    [Header("Camera zoom out based on height")]
    [SerializeField]
    float m_fieldOfViewIncrease = 0;
    [SerializeField]
    float m_previousHeight = 0;
    [SerializeField]
    float m_minimumHeightThreshold = 0;
    [SerializeField]
    float m_maximumHeightThreshold = 20;

    [Range(0, 30)]
    [SerializeField]
    float m_FOVIncrease;

    [Header("Camera Shake Factors")]
    //Camera Shake Factors
    [SerializeField]
    float m_smallTrembleForce = 0.05f;
    [SerializeField]
    float m_bigTrembleForce = 0.15f;
    [SerializeField]
    float m_smallTrembleDuration = 0.35f;
    [SerializeField]
    float m_bigTrembleDuration = 0.5f;

    [Header("Camera Random Movement Factors")]
    //Camera Drunk variables
    [SerializeField]
    float m_drunkDuration = 5f;
    [SerializeField]
    float m_drunkIntensity = 2;

    [Header("Camera Rotation Variables")]
    //Camera Drunk variables
    [SerializeField]
    [Range(0,1)]
    float m_rotationSpeed = 0.1f;
    [SerializeField]
    float m_rotationAngle = 90;

    [Header("Camera Flash Variables")]
    [SerializeField]
    Color m_flashColor = new Color(255, 0, 0, 0);
    private GameObject m_colorFlash;
    [SerializeField]
    float m_flashDuration = 1;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        m_previousHeight = m_minimumHeightThreshold;

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

        //Color Flash image
        m_colorFlash = new GameObject();
        m_colorFlash.transform.SetParent(m_canvas.transform);//Sets the canvas as the parent of the image
        m_colorFlash.transform.localScale = Vector3.one;
        m_colorFlash.AddComponent<Image>();
        m_colorFlash.name = "ColorFlash";
        m_colorFlash.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(0f, 0f);
        m_colorFlash.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        m_colorFlash.GetComponent<Image>().color = m_flashColor;

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

        if (m_mainPlayer == null)
        {
            Debug.LogError("Main player can not be found, add the Player tag to the main player.");
        }

        m_originalCameraPosition = Camera.main.transform.position;
        m_originalCameraSize = Camera.main.orthographicSize;


        m_mainPlayerPosition = new Vector3(m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
        m_target = new Vector3(m_originalCameraPosition.x, m_cameraOffset + m_originalCameraPosition.y, m_originalCameraPosition.z);
    }

    /// <summary>
    /// Returns the blackout image
    /// </summary>
    /// <returns></returns>
    public GameObject ReturnBlackOutImage()
    {
        return m_blackOutImage;
    }

    // Update is called once per frame
    void FixedUpdate()
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

        // Evaluate Point og focus
        if (m_togglePointOfFocus == true)
        {
            if (m_pointOfFocus != null)
            {
                EvaluatePointOfFocus();
            }
        }

        if (Input.GetKey(KeyCode.M))
        {
            LockCameraPosition();
        }

        if (m_toggleCameraPan)
        {
            CameraPanKeys();
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

        if (m_toggleCameraDirectionalFacing)
        {
            m_toggleCameraFollowPlayer = false;
            FaceDirection();
        }


        //if (!m_toggleCameraDirectionalFacing)
        //{
        //    m_toggleCameraFollowPlayer = true;
        //}


        if (m_toggleCameraHeightZoom)
        {
            HeightZoom();
        }
    }

    /// <summary>
    /// A function used to tell the camera to shake itself.
    /// </summary>
    public void EnableTremble(bool small)
    {
        if (m_toggleShake)
        {
            if (small)
                StartCoroutine(Tremble(m_smallTrembleDuration, m_smallTrembleForce));
            else
                StartCoroutine(Tremble(m_bigTrembleDuration, m_bigTrembleForce));
        }
    }

    /// <summary>
    /// Shakes the camera for a very short duration around its current position.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Tremble(float duration, float force)
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            // wait till the end of frame
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;

            // pick random position
            float x = Random.Range(-force, force);
            float y = Random.Range(-force, force);

            // translate the camera position
            Vector3 position = transform.position;
            position += new Vector3(x, y, 0);
            transform.position = position;
        }
    }

    /// <summary>
    /// Toggles the X axis lock.
    /// </summary>
    public void ToggleXAxisLock()
    {
        m_lockXAxis = !m_lockXAxis;
    }

    /// <summary>
    /// Locks the X axis so it stays in the same position
    /// </summary>
    public void LockXAxis()
    {
        if (!m_xAxisLocked)
        {
            m_xAxis = m_originalCameraPosition.x;
            m_xAxisLocked = true;
        }

    }

    /// <summary>
    /// Toggles the Y axis lock.
    /// </summary>
    public void ToggleYAxisLock()
    {
        m_lockYAxis = !m_lockYAxis;
    }

    /// <summary>
    /// Locks the Y axis so it stays in the same position
    /// </summary>
    public void LockYAxis()
    {
        if (!m_yAxisLocked)
        {
            m_yAxis = m_originalCameraPosition.y;
            m_yAxisLocked = true;
        }
    }

    /// <summary>
    /// Sets the alpha/transparancey of the image
    /// </summary>
    public void VignetteEffect()
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

    /// <summary>
    /// Assigns the value of the transparancey
    /// </summary>
    /// <param name="t_value">Value of transparancey</param>
    public void AssignVignetteValue(float t_value)
    {
        m_transparancey += t_value;

        m_vignetteTransparancey = m_transparancey / 100;
        VignetteEffect();
    }

    public void LockCameraPosition()
    {
        ToggleXAxisLock();
        ToggleYAxisLock();
    }

    /// <summary>
    /// Sets the camera look/pan keys
    /// </summary>
    /// <param name="t_hold">Hold keybind</param>
    /// <param name="t_up">Look up keybind</param>
    /// <param name="t_down">Look down keybind</param>
    /// <param name="t_left">Look left keybind</param>
    /// <param name="t_right">Look right keybind</param>
    public void SetCameraPanKeyCode(KeyCode t_hold = KeyCode.LeftControl,KeyCode t_up = KeyCode.I, KeyCode t_down = KeyCode.M, KeyCode t_left = KeyCode.J, KeyCode t_right = KeyCode.L)
    {
        m_lookHold = t_hold;
        m_lookUp = t_up;
        m_lookDown = t_down;
        m_lookLeft = t_left;
        m_lookRight = t_right;
    }

    /// <summary>
    /// Pressing pre-defined keys will have the camera pan to X position away from the player
    /// </summary>
    public void CameraPanKeys()
    {
        if (Input.GetKey(m_lookHold))
        {
            if (Input.GetKey(m_lookUp))
            {
                m_toggleCameraFollowPlayer = false;
                m_usingEffect = true;
                m_target = new Vector3(m_mainPlayer.transform.position.x, m_cameraOffset + m_mainPlayer.transform.position.y, -10);
                transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(m_lookDown))
            {
                m_toggleCameraFollowPlayer = false;
                m_usingEffect = true;
                m_target = new Vector3(m_mainPlayer.transform.position.x, -m_cameraOffset + m_mainPlayer.transform.position.y, -10);
                transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(m_lookLeft))
            {
                m_toggleCameraFollowPlayer = false;
                m_usingEffect = true;
                m_target = new Vector3(-m_cameraOffset + m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
                transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(m_lookRight))
            {
                m_toggleCameraFollowPlayer = false;
                m_usingEffect = true;
                m_target = new Vector3(m_cameraOffset + m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
                transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_target, ref velocity, m_cameraMoveSpeed * Time.deltaTime);
            }

        }
        else
        {
            m_target = m_mainPlayerPosition;
            m_toggleCameraFollowPlayer = true;
            MoveCameraBackToPlayer();
        }
    }

    /// <summary>
    /// Sets the camera zoom in keys
    /// </summary>
    /// <param name="t_zoomIn">Key for zooming in</param>
    /// <param name="t_zoomOut">Key for zooming out</param>
    public void SetCameraZoomKeys(KeyCode t_zoomIn = KeyCode.U, KeyCode t_zoomOut = KeyCode.O)
    {
        m_zoomIn = t_zoomIn;
        m_zoomOut = t_zoomOut;
    }

    /// <summary>
    /// Zooms the camera based on the scroll wheel input and by press I or O
    /// </summary>
    private void CameraZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            m_fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * m_sensitivity;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            m_fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * m_sensitivity;
        }

        if (Input.GetKey(m_zoomIn))
        {
            m_fieldOfView -= 0.1f;
        }
        else if (Input.GetKey(m_zoomOut))
        {
            m_fieldOfView += 0.1f;
        }

        m_fieldOfView = Mathf.Clamp(m_fieldOfView, m_minZoom, m_maxZoom);
        Camera.main.orthographicSize = m_fieldOfView;
    }


    /// <summary>
    /// Depending on the horizontal axis input, the camera will face left or right of the object
    /// </summary>
    void FaceDirection()
    {
        float input = Input.GetAxisRaw("Horizontal");

        //Debug.Log(input);

        if (m_lockDirection == false)
        {
            if (input == 1)
            {
                m_lookingRight = true;
            }
            else if (input == -1)
            {
                m_lookingRight = false;
            }

        }

        if (m_lookingRight)
        {
            Vector3 desiredPosition = new Vector3(m_mainPlayerPosition.x + m_facingTargetOffset, m_mainPlayerPosition.y, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_cameraSmoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else
        {
            Vector3 desiredPosition = new Vector3(m_mainPlayerPosition.x - m_facingTargetOffset, m_mainPlayerPosition.y, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_cameraSmoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }


    /// <summary>
    /// Moves camera back to player
    /// </summary>
    void MoveCameraBackToPlayer()
    {
        if (m_usingEffect)
        {
            if (Vector2.Distance(transform.position, m_mainPlayerPosition) > 0.5f)
            {
                //transform.position = Vector3.SmoothDamp(Camera.main.transform.position, m_mainPlayerPosition, ref velocity, (m_cameraMoveSpeed / 2) * Time.deltaTime);
                transform.position = Vector3.Lerp(Camera.main.transform.position, m_mainPlayerPosition,0.1f );
                transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0,0,0,0), 0.01f);
            }
            else
            {
                m_usingEffect = false;
            }
        }
        else
        {
            transform.position = new Vector3(m_mainPlayer.transform.position.x, m_mainPlayer.transform.position.y, -10);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    /// <summary>
    /// The faster the character is going, the wider they'll be able to see.
    /// </summary>
    private void SpeedZoom()
    {
        Vector2 vel = new Vector2( m_mainPlayer.GetComponent<Rigidbody2D>().velocity.x, 0.0f);
        m_currentSpeed = vel.magnitude;
        if (m_currentSpeed > 0.5f && Camera.main.orthographicSize < m_maxZoom)
        {
            //Camera.main.orthographicSize += m_originalCameraSize * (m_currentSpeed/2);
            Camera.main.orthographicSize +=  0.01f * (m_currentSpeed / 2);
        }
        else if (Camera.main.orthographicSize > m_maxZoom)
        {
            Camera.main.orthographicSize = m_maxZoom;
        }
        else
        {
            if (Camera.main.orthographicSize > m_originalCameraSize)
            {
                Camera.main.orthographicSize -= 0.1f;
            }

            if (Camera.main.orthographicSize < m_originalCameraSize)
            {
                Camera.main.orthographicSize = m_originalCameraSize;
            }
        }
    }

    /// <summary>
    /// Camera zooms in/out based on the Y axis of the player
    /// </summary>
    private void HeightZoom()
    {

        if (m_mainPlayerPosition.y > m_minimumHeightThreshold && m_mainPlayerPosition.y < m_maximumHeightThreshold)
        {
            if (m_previousHeight < m_mainPlayerPosition.y)
            {
                m_previousHeight = m_mainPlayerPosition.y;

                if (m_FOVIncrease < m_maxZoom)
                {
                    m_FOVIncrease = m_mainPlayerPosition.y - m_minimumHeightThreshold;
                }
                else
                {
                    m_FOVIncrease = m_maxZoom;
                }

                Camera.main.orthographicSize = m_originalCameraSize + m_FOVIncrease;
            }

            if (m_previousHeight > m_mainPlayerPosition.y)
            {

                m_previousHeight = m_mainPlayerPosition.y;
                if (m_FOVIncrease > 0)
                {
                    m_FOVIncrease = m_mainPlayerPosition.y - m_minimumHeightThreshold;
                }
                else
                {
                    m_FOVIncrease = 0;
                }

                if (m_FOVIncrease < m_maxZoom)
                {
                    Camera.main.orthographicSize = m_originalCameraSize + m_FOVIncrease;
                }
            }
        }
    }


    /// <summary>
    /// Fades the screen in or out
    /// </summary>
    public void FadeInOut()
    {
        StopAllCoroutines();
        m_fadeToBlack = !m_fadeToBlack;
        StartCoroutine(FadeToBlack(m_fadeToBlack, m_fadeoutSpeed));
    }

    /// <summary>
    /// Fades to black or not
    /// </summary>
    /// <param name="t_fadeToBlack">Bool to check if you're either fading in or out of the blackout image</param>
    /// <param name="t_fadeSpeed">How fast it fades out</param>
    /// <returns></returns>
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

    public void EvaluatePointOfFocus()
    {
        Vector3 focusPos = m_pointOfFocus.transform.position;
        Vector3 currentPos = m_mainPlayer.transform.position;
        Vector3 difference = focusPos - currentPos;

        // Size of the ortographic camera
        float height = 2f * GetComponent<Camera>().orthographicSize;
        float width = height * GetComponent<Camera>().aspect;

        Vector3 allowedDistance = new Vector3(width - (0.4f * width), height - (0.4f * height), 0);

        // If the player is close enough to the point of focus
        if (difference.sqrMagnitude < allowedDistance.sqrMagnitude)
        {
            transform.position = new Vector3(m_mainPlayer.transform.position.x + difference.x / 2, m_mainPlayer.transform.position.y + difference.y / 2, transform.position.z);     // Snappy adjustment
        }
    }

    public void RandomMovementCameraEffect(float duration, float intensity, float rotationIntensity)
    {
        StopAllCoroutines();
        m_toggleCameraFollowPlayer = false;
        m_drunkDuration = duration; 
        m_drunkIntensity = intensity;
        m_rotationAngle = rotationIntensity;
        StartCoroutine(DrunkEffect(m_drunkDuration, m_drunkIntensity, m_rotationAngle));
    }

    IEnumerator DrunkEffect(float duration, float intensity, float rotationIntensity)
    {
        float timer = 0.0f;
        bool reachedTarget = false;
        float x = Random.Range(-intensity, intensity);
        float y = Random.Range(-intensity, intensity);
        Vector3 position = new Vector3(x, y, -10);
        m_usingEffect = true;
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-rotationIntensity, rotationIntensity));
        while (timer < duration)
        {
            timer += Time.deltaTime;
            
            if (reachedTarget)
            {
                reachedTarget = false;
                // pick random position
                x = Random.Range(-intensity, intensity);
                y = Random.Range(-intensity, intensity);
                position = new Vector3(x, y, -10);
            }
            else
            {
                transform.position = Vector3.Lerp(Camera.main.transform.position, position + m_mainPlayerPosition, 0.01f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.01f);
            }

            if (Vector3.Distance(transform.position, position + m_mainPlayerPosition) < 0.5f)
            {
                reachedTarget = true;
            }
            yield return new WaitForEndOfFrame();

        }

        m_toggleCameraFollowPlayer = true;
    }

    /// <summary>
    /// Call the rotate camera function
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="angle"></param>
    public void CallRotateCamera(float speed, float angle)
    {
        StopAllCoroutines();
        m_toggleCameraFollowPlayer = false;
        if (speed > 1)
        {
            speed = 1;
        }
        m_rotationSpeed = speed;
        m_rotationAngle = angle;
        StartCoroutine(RotateCamera(m_rotationSpeed, m_rotationAngle));
    }


    //Rotates the camera to said angle
    IEnumerator RotateCamera(float speed, float angle)
    {
        while (transform.rotation != Quaternion.Euler(0, 0, angle))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,angle), speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Activates the flashing of the screen
    /// </summary>
    /// <param name="t_color">The color needed</param>
    /// <param name="t_flashDuration">How long the flash takes to go from 0 to 1 to 0</param>
    public void FlashColour(Color t_color, float t_flashDuration)
    {
        StopAllCoroutines();

        m_flashColor = new Color( t_color.r, t_color.g, t_color.b, 0);
        m_flashDuration = t_flashDuration;
        StartCoroutine(FlashCamera(m_flashColor, m_flashDuration));
    }

    /// <summary>
    /// Flashes between zero alpha to one and back to zero
    /// </summary>
    /// <param name="t_color"></param>
    /// <param name="t_secondsPerFlash"></param>
    /// <returns></returns>
    IEnumerator FlashCamera(Color t_color, float t_secondsPerFlash)
    {
        //bool flashDone = false;
        //float flashAmount;
        Color fadeColor = t_color;
        float flashDuration = t_secondsPerFlash / 2;
        //Flash in
        for (float i = 0; i <= flashDuration; i += Time.deltaTime)
        {
            Color color = fadeColor;
            color.a = Mathf.Lerp(0, 1, i / flashDuration);
            m_colorFlash.GetComponent<Image>().color = color;
            yield return null;
        }

        for (float i = 0; i <= flashDuration; i += Time.deltaTime)
        {
            Color color = fadeColor;
            color.a = Mathf.Lerp(1, 0, i / flashDuration);
            m_colorFlash.GetComponent<Image>().color = color;
            yield return null;
        }

        yield return new WaitForEndOfFrame();

    }
}
