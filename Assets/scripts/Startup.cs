using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Startup : MonoBehaviour
{
    [SerializeField] public float DelayStartup = 10f;
    public GameObject Camera;
    public GameObject Character;
    
    public bool isStarted { get; private set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > DelayStartup && !isStarted)
        {
            isStarted = true;
            Camera.GetComponent<Camera>().orthographicSize = 5;
            Camera.GetComponent<CameraView>()._target = Character.transform;
            Camera.GetComponent<PostProcessVolume>().enabled = true;
            this.enabled = false;
        }
    }
}
