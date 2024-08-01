using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationRig : MonoBehaviour
{

    public float rotationSpeed = 10f;
    [Range(1, 8)]
    public int supersize = 6;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Taking Screenshot!");
            ScreenCapture.CaptureScreenshot("Screenshots/Screenshot_" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "_").Replace(":","-") + ".png", 6);
        }
    }
}
