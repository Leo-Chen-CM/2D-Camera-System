using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CameraSystem2D.Instance.SetCameraPanKeyCode();
        //CameraSystem2D.Instance.SetXAxisLock(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
