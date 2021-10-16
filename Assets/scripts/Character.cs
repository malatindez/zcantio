using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingMode  
{ 
    Walking, 
    Running, 
    FastRunning 
}

public class Character : MonoBehaviour
{
    MovingMode MovingMode { get; set; }
    float WalkSpeed { get; set; } = 5;
    float RunSpeed { get; set; } = 7.5f;
    float FastRunSpeed { get; set; } = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float speed = (MovingMode == MovingMode.Walking) ? WalkSpeed : (MovingMode == MovingMode.Running) ? RunSpeed : FastRunSpeed;
        transform.Translate(new Vector3(horizontalInput, 0, 0) * speed * Time.deltaTime);
    }
}
