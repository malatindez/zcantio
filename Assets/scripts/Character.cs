using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingMode  
{ 
}

public class Character : MonoBehaviour
{
    public float Running = 7.5f;
    public float FastRunning = 20;
    public float ReachDistance = 0.1f;
    public float MovingSpeed;
    public Vector2 TargetPosition { get; set; }
    
    public bool isRightSide = true;
    public Animator animator;

    public void AnimationIdle()
    {
        animator.SetInteger("MovingState", 0);
    }


    public void AnimationRun()
    {
        animator.SetInteger("MovingState", 1);
    }

    public void AnimationFasterRun()
    {
        animator.SetInteger("MovingState", 2);
    }

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        MovingSpeed = Running;
        TargetPosition = transform.position;
    }

    public float Delay = 0.1f;
    float stopwatch = 0f;

    void Update()
    {
        float distance = TargetPosition.x - transform.position.x;
        if (Mathf.Abs(distance) > ReachDistance)
        {
            if (stopwatch < Delay)
            {
                stopwatch += Time.deltaTime;
            }
            else
            {
                isRightSide = distance > 0 ? true : false;
                gameObject.transform.localScale = new Vector3(isRightSide ? 1 : -1, 1, 1);
                int wayToGo = transform.position.x > TargetPosition.x ? -1 : 1;
                transform.Translate(new Vector3(wayToGo, 0, 0) * MovingSpeed * Time.deltaTime);
            }
        }
        else
        {
            AnimationIdle();
            stopwatch = 0f;
        }
    }
}
