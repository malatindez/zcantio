using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Startup : MonoBehaviour
{
    public float DelayStartup = 10f;
    public GameObject Camera;
    public GameObject Character;

    float stopwatch;
    bool isStarted = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stopwatch > DelayStartup && !isStarted)
        {
            print("Test");
            isStarted = true;
            Camera.GetComponent<Camera>().orthographicSize = 5;
            Camera.GetComponent<CameraView>()._target = Character.transform;
            Camera.GetComponent<PostProcessVolume>().enabled = true;
        }
        else
            stopwatch += Time.deltaTime;
    }
}
