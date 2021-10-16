using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingMode  
{ 
}

public class Character : MonoBehaviour
{
    public const float Walking = 5;
    public const float Running = 7.5f;
    public const float FastRunning = 10;
    public const float ReachDistance = 0.3f;
    public float MovingSpeed { get; set; } = Walking;
    public Vector2 TargetPosition { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        TargetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(TargetPosition.x - transform.position.x) > ReachDistance)
        {
            int wayToGo = transform.position.x > TargetPosition.x ? -1 : 1;
            transform.Translate(new Vector3(wayToGo, 0, 0) * MovingSpeed * Time.deltaTime);
        }
    }
}
