using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem2D : MonoBehaviour
{
    private float m_currentSpeed;
    private float m_maxSize;
    private GameObject m_mainPlayer;
    private float m_originalCameraSize;
    public bool m_toggleSpeedZoomOut;
    // Start is called before the first frame update
    void Start()
    {
        m_maxSize = 20;
        m_mainPlayer = GameObject.FindGameObjectWithTag("Player");
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
}
