using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public float Running;
    public float FastRunning;
    public float ReachDistance;
    public float MovingSpeed;
    public bool isRightSide;
    public Animator animator;
    public float Delay;

    public Vector2 TargetPosition { get; set; }

    public bool Idling { get; private set; } = true;

    private bool idlingBuffer = true;
    public void UpdateAnimation()
    {
        if (Idling != idlingBuffer)
        {
            animator.SetInteger("MovingState", System.Convert.ToInt32(!Idling));
            idlingBuffer = Idling;
        }
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        MovingSpeed = Running;
        TargetPosition = transform.position;
    }

    private float _stopwatch = 0;
    void Update()
    {
        if(_stopwatch > Time.realtimeSinceStartup)
        {
            if (Time.realtimeSinceStartup - _stopwatch > -Delay)
            {
                Idling = true;
            }
            return;
        }
        float distance = TargetPosition.x - transform.position.x;
        if (Mathf.Abs(distance) > 0.1f)
        {
            isRightSide = distance > 0;
            gameObject.transform.localScale = new Vector3(isRightSide ? 1 : -1, 1, 1);
            int wayToGo = transform.position.x > TargetPosition.x ? -1 : 1;
            transform.Translate(new Vector3(wayToGo, 0, 0) * MovingSpeed * Time.deltaTime);
            Idling = false;
        }
        else
        {
            _stopwatch = Time.realtimeSinceStartup + Delay * 2;
        }
        UpdateAnimation();
    }
}
